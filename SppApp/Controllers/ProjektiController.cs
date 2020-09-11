using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using SppApp.Models;
using SppApp.Helpers;
using System.Data.Entity.Validation;
using Rotativa;

namespace SppApp.Controllers
{
    public class ProjektiController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: Projekti
        public ActionResult Index()
        {
            var Projekti = db.Projekti.Include(p => p.Kontakt).Include(p => p.Organizacija);
            ViewBag.CurrentUserID = User.Identity.GetUserId();
            return View(Projekti.ToList());
        }

        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("Index");
            return report;
        }

        public ActionResult PrintPartialViewToPdf(int id)
        {
            Projekti projekt = db.Projekti.FirstOrDefault(c => c.Id == id);

            var report = new PartialViewAsPdf("~/Views/Projekti/Details.cshtml", projekt);
            return report;

        }
        // GET: Projekti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Ako user nije ulogiran i već je bio na stranici
            if (Session["projektID"] == null && !Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["projektID"] != null)
            {
                ViewBag.Zaprimljena = "Vaša prijava je zaprimljena!";
            }

            Projekti projekti = db.Projekti.Find(id);
            if (projekti == null)
            {
                return HttpNotFound();
            }

            //User ne može vidjeti projekte koje nije napravio
            if (projekti.UserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                //To do - za sada vidi samo admin
                return RedirectToAction("Index", "Projekti");
            }

            Session["ProjektID"] = null;
            return View(projekti);

        }

        // GET: Projekti/Create
        public ActionResult Create()
        {
            ViewBag.KontaktId = new SelectList(db.Kontakti, "ID", "Ime");
            ViewBag.Organizacije = new SelectList(db.Organizacije, "ID", "Naziv");
            ViewBag.OrganizacijaId = new SelectList(db.Organizacije, "ID", "Naziv");
            ViewBag.OrganizacijaId = new SelectList(db.Organizacije, "ID", "Naziv");
            ViewBag.OrganizacijaId = new SelectList(db.Organizacije, "ID", "Naziv");
            Projekti projekt = new Projekti();
            projekt.Aktivnosti = new List<Aktivnosti>();
            projekt.Aktivnosti.Add(new Aktivnosti());
            projekt.Dionici = new List<Dionici>();
            projekt.Dionici.Add(new Dionici());
            projekt.Financiranja = new List<Financiranja>();
            projekt.Financiranja.Add(new Financiranja());
            projekt.GradjevinskeDozvole = new List<GradjevinskeDozvole>();
            projekt.Kontakt = new Kontakti();
            projekt.Kontakt.Organizacija = new Organizacije();
            projekt.Organizacija = new Organizacije();
            projekt.OstalaDokumentacija = new List<OstalaDokumentacija>();
            projekt.Pokazatelji = new List<Pokazatelji>();
            projekt.Pokazatelji.Add(new Pokazatelji());

            return View(projekt);
        }

        // POST: Projekti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Projekti projekt, FormCollection form, HttpPostedFileBase[] dozvolaDatoteke, HttpPostedFileBase[] ostaleDatoteke, string submitButton)
        {
            projekt = HelperMethods.DodajCustomLokaciju(projekt, form);

            projekt = HelperMethods.DodajUsera(projekt, User.Identity.GetUserId());

            projekt = HelperMethods.DodajAktivnosti(projekt, form);

            projekt = HelperMethods.DodajFinanciranja(projekt, form);

            projekt = HelperMethods.DodajDionike(projekt, form);

            projekt = HelperMethods.DodajPokazatelje(projekt, form);

            try
            {
                if (ModelState.IsValid)
                {
                    projekt = HelperMethods.DodajGradjevinskeDozvole(projekt, dozvolaDatoteke);

                    projekt = HelperMethods.DodajOstaluDokumentaciju(projekt, ostaleDatoteke);

                    Session["projektID"] = projekt.Id;

                    if (submitButton.Equals("Pošalji"))
                    {
                        projekt.DatumPredaje = DateTime.Now;
                        projekt.Upisano = true;
                        db.Projekti.Add(projekt);
                        db.SaveChanges();

                        HelperMethods.SendEmailNotification(projekt);

                        return RedirectToAction("Details", "Projekti", new { id = projekt.Id });
                    }
                    else
                    {
                        db.Projekti.Add(projekt);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            ViewBag.KontaktId = new SelectList(db.Kontakti, "Id", "Ime", projekt.KontaktId);
            ViewBag.OrganizacijaId = new SelectList(db.Organizacije, "Id", "Naziv", projekt.OrganizacijaId);
            return View(projekt);
        }

        [Authorize]
        // GET: Projekti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projekti projekti = db.Projekti.Find(id);

            if (projekti == null)
            {
                return HttpNotFound();
            }

            if (projekti.Lokacija != "Grad" && projekti.Lokacija != "Općina")
            {
                ViewBag.Lokacija = projekti.Lokacija;
            }

            ViewBag.KontaktId = new SelectList(db.Kontakti, "Id", "Ime", projekti.KontaktId);
            ViewBag.OrganizacijaId = new SelectList(db.Organizacije, "Id", "Naziv", projekti.OrganizacijaId);
            return View(projekti);
        }

        [Authorize]
        // POST: Projekti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Projekti projekt, FormCollection form, HttpPostedFileBase[] dozvolaDatoteke, HttpPostedFileBase[] ostaleDatoteke, string submitButton)
        {
            
            if (submitButton.Equals("Odustani"))
            {
                return RedirectToAction("Index", "Projekti");
            }

            projekt = HelperMethods.DodajCustomLokaciju(projekt, form);

            if (ModelState.IsValid)
            {

                if (!form["UserId"].IsNullOrWhiteSpace())
                {
                    projekt = HelperMethods.DodajUsera(projekt, form["UserId"]);
                }

                projekt = HelperMethods.DodajAktivnosti(projekt, form);

                projekt = HelperMethods.DodajFinanciranja(projekt, form);

                projekt = HelperMethods.DodajDionike(projekt, form);

                projekt = HelperMethods.DodajPokazatelje(projekt, form);

                projekt = HelperMethods.DodajGradjevinskeDozvole(projekt, dozvolaDatoteke);

                projekt = HelperMethods.DodajOstaluDokumentaciju(projekt, ostaleDatoteke);

                //if (HttpContext.Request.Files.AllKeys.Any())
                //{
                    //var test = HttpContext.Request.Files.AllKeys.ToArray(); // remove this after test

                    //// Get the uploaded image from the Files collection
                    //var httpPostedFile = HttpContext.Request.Files[0];

                    //if (httpPostedFile != null)
                    //{
                    //    // Validate the uploaded image(optional)

                    //    // Get the complete file path
                    //    var fileSavePath = (HttpContext.Server.MapPath("~/UploadedFiles") + httpPostedFile.FileName.Substring(httpPostedFile.FileName.LastIndexOf(@"\")));

                    //    // Save the uploaded file to "UploadedFiles" folder
                    //    httpPostedFile.SaveAs(fileSavePath);
                    //}
                //}

                Session["projektID"] = projekt.Id;

                if (submitButton.Equals("Pošalji"))
                {
                    projekt.DatumPredaje = DateTime.Now;
                    projekt.Upisano = true;
                    db.Entry(projekt).State = EntityState.Modified;

                    db.SaveChanges();

                    HelperMethods.SendEmailNotification(projekt);

                    return RedirectToAction("Details", "Projekti", new { id = projekt.Id });
                }
                else
                {
                    //var dbProjekt = db.Projekti
                    //  .Include(x => x.Kontakt)
                    //  .Include(x => x.Organizacija)
                    //  .Include(x => x.Organizacija.Kontakt)
                    //  .Single(c => c.Id == projekt.Id);

                    //db.Entry(dbProjekt).CurrentValues.SetValues(projekt);
                    //db.Entry(dbProjekt.Kontakt).CurrentValues.SetValues(projekt.Kontakt);
                    //db.Entry(dbProjekt.Organizacija).CurrentValues.SetValues(projekt.Organizacija);
                    //db.Entry(dbProjekt.Organizacija.Kontakt).CurrentValues.SetValues(projekt.Organizacija.Kontakt);

                    //db.SaveChanges();

                    db.Entry(projekt).State = EntityState.Modified;
                    db.Entry(projekt.Kontakt).State = EntityState.Modified;
                    db.Entry(projekt.Organizacija).State = EntityState.Modified;
                    db.Entry(projekt.Kontakt.Organizacija).State = EntityState.Modified;

                    foreach (var item in projekt.Aktivnosti)
                    {
                        if (item.Id != 0)
                        {
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            db.Entry(item).State = EntityState.Added;
                        }
                    }
                    foreach (var item in projekt.Dionici)
                    {
                        if (item.Id != 0)
                        {
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            db.Entry(item).State = EntityState.Added;
                        }
                    }
                    foreach (var item in projekt.Financiranja)
                    {
                        if (item.Id != 0)
                        {
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            db.Entry(item).State = EntityState.Added;
                        }
                    }
                    foreach (var item in projekt.Pokazatelji)
                    {
                        if (item.Id != 0)
                        {
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            db.Entry(item).State = EntityState.Added;
                        }
                    }

                    foreach (var item in projekt.GradjevinskeDozvole)
                    {
                        if (item.Id != 0)
                        {
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            db.Entry(item).State = EntityState.Added;
                        }
                    }
                    foreach (var item in projekt.OstalaDokumentacija)
                    {
                        if (item.Id != 0)
                        {
                            db.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            db.Entry(item).State = EntityState.Added;
                        }
                    }
                    //foreach (var aktivnost in projekt.Aktivnosti.ToList())
                    //    if (!projekt.Aktivnosti.Any(s => s.Id == aktivnost.Id))
                    //        db.Aktivnosti.Remove(aktivnost);

                    //foreach (var aktivnost in projekt.Aktivnosti)
                    //{
                    //    var dbAktivnost = projekt.Aktivnosti.SingleOrDefault(s => s.Id == aktivnost.Id);
                    //    if (dbAktivnost != null)
                    //        // Update subFoos that are in the newFoo.SubFoo collection
                    //        db.Entry(dbAktivnost).CurrentValues.SetValues(aktivnost);
                    //    else
                    //        // Insert subFoos into the database that are not
                    //        // in the dbFoo.subFoo collection
                    //        projekt.Aktivnosti.Add(aktivnost);
                    //}

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            ViewBag.KontaktId = new SelectList(db.Kontakti, "Id", "Ime", projekt.KontaktId);
            ViewBag.OrganizacijaId = new SelectList(db.Organizacije, "Id", "Naziv", projekt.OrganizacijaId);
            return View(projekt);
        }

        [Authorize]
        // GET: Projekti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projekti projekti = db.Projekti.Find(id);
            if (projekti == null)
            {
                return HttpNotFound();
            }
            return View(projekti);
        }

        [Authorize]
        // POST: Projekti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projekti projekt = db.Projekti.Find(id);
            List<Aktivnosti> lsAktivnosti = db.Aktivnosti.Where(x => x.ProjektId == projekt.Id).ToList();
            List<int> lsAktivnostiIDs = lsAktivnosti.Select(x => x.Id).ToList();
            projekt.Aktivnosti = lsAktivnosti;
            List<Dionici> lsDionici = db.Dionici.Where(x => x.ProjektId == projekt.Id).ToList();
            List<int> lsDioniciIDs = lsDionici.Select(x => x.Id).ToList();
            projekt.Dionici = lsDionici;
            List<Financiranja> lsFinanciranja = db.Financiranja.Where(x => x.ProjektId == projekt.Id).ToList();
            List<int> lsFinanciranjaIDs = lsFinanciranja.Select(x => x.Id).ToList();
            projekt.Financiranja = lsFinanciranja;
            List<Pokazatelji> lsPokazatelji = db.Pokazatelji.Where(x => x.ProjektId == projekt.Id).ToList();
            List<int> lsPokazateljiIDs = lsPokazatelji.Select(x => x.Id).ToList();
            projekt.Pokazatelji = lsPokazatelji;
            //To do: Add file deletion
            List<GradjevinskeDozvole> lsGradjevinskeDozvole = db.GradjevinskeDozvole.Where(x => x.ProjektId == projekt.Id).ToList();
            List<int> lsGradjevinskeDozvoleIDs = lsGradjevinskeDozvole.Select(x => x.Id).ToList();
            projekt.GradjevinskeDozvole = lsGradjevinskeDozvole;
            List<OstalaDokumentacija> lsOstalaDokumentacija = db.OstalaDokumentacija.Where(x => x.ProjektId == projekt.Id).ToList();
            List<int> lsOstalaDokumentacijaIDs = lsOstalaDokumentacija.Select(x => x.Id).ToList();
            projekt.OstalaDokumentacija = lsOstalaDokumentacija;

            foreach (var itemDozvole in lsGradjevinskeDozvole)
            {
                if (System.IO.File.Exists(itemDozvole.Putanja))
                {
                    System.IO.File.Delete(itemDozvole.Putanja);
                }
            }

            foreach (var itemDokumentacija in lsOstalaDokumentacija)
            {
                if (System.IO.File.Exists(itemDokumentacija.Putanja))
                {
                    System.IO.File.Delete(itemDokumentacija.Putanja);
                }
            }


            db.Projekti.Remove(projekt);
            db.SaveChanges();

            foreach (var aktivnostId in lsAktivnostiIDs)
            {
                db.Aktivnosti.Remove(db.Aktivnosti.FirstOrDefault(x => x.Id == aktivnostId));
            }
            foreach (var dionikId in lsDioniciIDs)
            {
                db.Dionici.Remove(db.Dionici.FirstOrDefault(x => x.Id == dionikId));
            }
            foreach (var financiranjeId in lsFinanciranjaIDs)
            {
                db.Financiranja.Remove(db.Financiranja.FirstOrDefault(x => x.Id == financiranjeId));
            }
            foreach (var pokazateljId in lsPokazateljiIDs)
            {
                db.Pokazatelji.Remove(db.Pokazatelji.FirstOrDefault(x => x.Id == pokazateljId));
            }
            foreach (var dozvolaId in lsGradjevinskeDozvoleIDs)
            {
                db.GradjevinskeDozvole.Remove(db.GradjevinskeDozvole.FirstOrDefault(x => x.Id == dozvolaId));
            }
            foreach (var ostalaId in lsOstalaDokumentacijaIDs)
            {
                db.OstalaDokumentacija.Remove(db.OstalaDokumentacija.FirstOrDefault(x => x.Id == ostalaId));
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DodajAktivnost(string sFirst, string sLast)
        {
            Aktivnosti Aktivnost = new Aktivnosti();
            return PartialView("AktivnostiPartial", Aktivnost);
        }
        public ActionResult DodajFinanciranje(string sFirst, string sLast)
        {
            Financiranja Financiranje = new Financiranja();
            return PartialView("FinanciranjaPartial", Financiranje);
        }

        public ActionResult DodajDionika(string sFirst, string sLast)
        {
            Dionici Dionik = new Dionici();
            return PartialView("DioniciPartial", Dionik);
        }
        public ActionResult DodajPokazatelja(string sFirst, string sLast)
        {
            Pokazatelji Pokazatelj = new Pokazatelji();
            return PartialView("PokazateljiPartial", Pokazatelj);
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "Datoteka je uspješno spremljena!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "Greška kod spremanja datoteke!";
                return View();
            }
        }

        public ActionResult Download(string putanja, string idModela)
        {
            if (System.IO.File.Exists(putanja))
            {
                string FileName = Path.GetFileName(putanja);

                return File(putanja, "application/force-download", FileName.Substring(15));
            }
            return RedirectToAction(idModela);
        }
    }
}
