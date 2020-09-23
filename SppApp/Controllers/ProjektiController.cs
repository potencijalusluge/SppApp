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
            string currentUserId = User.Identity.GetUserId();

            Projekti projekt = new Projekti();
            //ViewBag.KontaktiLista = new SelectList(db.Kontakti, "Id", "Ime");
            //ViewBag.OrganizacijeLista = new SelectList(db.Organizacije, "ID", "Naziv");
            if (User.IsInRole("Admin"))
            {
                ViewBag.KontaktiLista = new SelectList(db.Kontakti, "Id", "Ime", projekt.KontaktId);
                ViewBag.OrganizacijeLista = new SelectList(db.Organizacije, "Id", "Naziv", projekt.OrganizacijaId);
            }
            else
            {
                ViewBag.KontaktiLista = new SelectList(db.Kontakti.Where(x => x.UserId == currentUserId), "Id", "Ime", projekt.KontaktId);
                ViewBag.OrganizacijeLista = new SelectList(db.Organizacije.Where(x => x.UserId == currentUserId), "Id", "Naziv", projekt.OrganizacijaId);
            }
            projekt.Aktivnosti = new List<Aktivnosti>();
            projekt.Aktivnosti.Add(new Aktivnosti());
            projekt.Dionici = new List<Dionici>();
            projekt.Dionici.Add(new Dionici());
            projekt.Financiranja = new List<Financiranja>();
            projekt.Financiranja.Add(new Financiranja());
            projekt.Kontakt = new Kontakti();
            projekt.Organizacija = new Organizacije();
            projekt.Pokazatelji = new List<Pokazatelji>();
            projekt.Pokazatelji.Add(new Pokazatelji());
            projekt.JavneNabave = new List<JavneNabave>();
            projekt.JavneNabave.Add(new JavneNabave());
            projekt.Rizici = new List<Rizici>();
            projekt.Rizici.Add(new Rizici());
            projekt.Dokumentacija = new List<Dokumentacija>();
            projekt.Uskladjenosti = new List<Uskladjenosti>();
            projekt.Uskladjenosti = HelperMethods.UcitajUskladjenosti();
            projekt = HelperMethods.UcitajDokumentaciju(projekt);
            //string sUskladjenosti = HelperMethods.KreirajUskladjenostiPartial(projekt.Uskladjenosti);
            return View(projekt);
        }

        // POST: Projekti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Projekti projekt, FormCollection form, HttpPostedFileBase[] Datoteke, string submitButton)
        {
            string currentUserId = User.Identity.GetUserId();

            projekt = HelperMethods.DodajUsera(projekt, currentUserId);

            projekt = HelperMethods.UsporediKontakte(projekt);

            projekt = HelperMethods.DodajAktivnosti(projekt, form);

            projekt = HelperMethods.DodajFinanciranja(projekt, form);

            projekt = HelperMethods.DodajDionike(projekt, form);

            projekt = HelperMethods.DodajPokazatelje(projekt, form);

            projekt.Uskladjenosti = new List<Uskladjenosti>();
            projekt = HelperMethods.DodajUskladjenosti(projekt, form);

            projekt = HelperMethods.DodajJavneNabave(projekt, form);

            projekt = HelperMethods.DodajRizike(projekt, form);

            if (!ModelState.IsValid)
            {
                projekt.Uskladjenosti = HelperMethods.UcitajUskladjenosti();

                //decimal dBroj;
                //if (this.ModelState["ProcijenjenaVrijednostHRK"].Errors.Count > 0)
                //{
                //    decimal.TryParse(form["ProcijenjenaVrijednostHRK"].ToString().Replace(".", ""), out dBroj);
                //    projekt.ProcijenjenaVrijednostHRK = dBroj;
                //}
                //if (this.ModelState["ProcijenjeniTroskoviPripremeHRK"].Errors.Count > 0)
                //{
                //    decimal.TryParse(form["ProcijenjeniTroskoviPripremeHRK"].ToString().Replace(".", ""), out dBroj);
                //    projekt.ProcijenjeniTroskoviPripremeHRK = dBroj;
                //}
                //if (this.ModelState["ProcijenjeniTroskoviProvedbeHRK"].Errors.Count > 0)
                //{
                //    decimal.TryParse(form["ProcijenjeniTroskoviProvedbeHRK"].ToString().Replace(".", ""), out dBroj);
                //    projekt.ProcijenjeniTroskoviProvedbeHRK = dBroj;
                //}
            }
            try
            {
                if (ModelState.IsValid)
                {
                    projekt.Dokumentacija = new List<Dokumentacija>();
                    projekt = HelperMethods.DodajDatoteke(projekt, Datoteke, form);


                    Session["projektID"] = projekt.Id;

                    projekt.DatumIzmjene = DateTime.Now;

                    if (submitButton.Equals("Pošalji"))
                    {
                        projekt.DatumPredaje = DateTime.Now;
                        projekt.Poslano = true;
                        db.Projekti.Add(projekt);
                        db.SaveChanges();

                        var partialView = PrintPartialViewToPdf(projekt.Id);

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

            //ViewBag.KontaktiLista = new SelectList(db.Kontakti, "Id", "Ime");
            //ViewBag.OrganizacijeLista = new SelectList(db.Organizacije, "ID", "Naziv");
            //ViewBag.KontaktId = new SelectList(db.Kontakti, "Id", "Ime", projekt.KontaktId); //old
            //ViewBag.OrganizacijaId = new SelectList(db.Organizacije, "Id", "Naziv", projekt.OrganizacijaId); //old
            if (!User.IsInRole("Admin"))
            {
                ViewBag.KontaktiLista = new SelectList(db.Kontakti, "Id", "Ime", projekt.KontaktId);
            }
            else
            {
                ViewBag.KontaktiLista = new SelectList(db.Kontakti.Where(x => x.UserId == currentUserId), "Id", "Ime", projekt.KontaktId);
            }
            ViewBag.OrganizacijeLista = new SelectList(db.Organizacije, "Id", "Naziv", projekt.OrganizacijaId);
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
            Projekti projekt = db.Projekti.Find(id);

            if (projekt == null)
            {
                return HttpNotFound();
            }

            string currentUserId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                ViewBag.KontaktiLista = new SelectList(db.Kontakti, "Id", "Ime", projekt.KontaktId);
                ViewBag.OrganizacijeLista = new SelectList(db.Organizacije, "Id", "Naziv", projekt.OrganizacijaId);
            }
            else
            {
                ViewBag.KontaktiLista = new SelectList(db.Kontakti.Where(x => x.UserId == currentUserId), "Id", "Ime", projekt.KontaktId);
                ViewBag.OrganizacijeLista = new SelectList(db.Organizacije.Where(x => x.UserId == currentUserId), "Id", "Naziv", projekt.OrganizacijaId);
            }

            projekt = HelperMethods.DopuniUskladjenosti(projekt);
            projekt = HelperMethods.DopuniDokumentaciju(projekt);

            return View(projekt);
        }

        [Authorize]
        // POST: Projekti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Projekti projekt, FormCollection form, HttpPostedFileBase[] Datoteke, string submitButton)
        {

            if (submitButton.Equals("Odustani"))
            {
                return RedirectToAction("Index", "Projekti");
            }

            //projekt = HelperMethods.DodajCustomLokaciju(projekt, form);
            if (ModelState.IsValid)
            {
                projekt.Dokumentacija = new List<Dokumentacija>();
                projekt = HelperMethods.DodajDatoteke(projekt, Datoteke, form);

                Session["projektID"] = projekt.Id;

                if (!form["UserId"].IsNullOrWhiteSpace())
                {
                    projekt = HelperMethods.DodajUsera(projekt, form["UserId"]);
                }
                if (projekt.Kontakt.Id != null)
                {
                    projekt.KontaktId = projekt.Kontakt.Id;
                }
                if (projekt.OdgovornaOsoba.Id != null)
                {
                    projekt.OdgovornaOsobaId = projekt.OdgovornaOsoba.Id;
                }

                projekt = HelperMethods.UsporediKontakte(projekt);

                if (projekt.Organizacija.Id != null)
                {
                    projekt.OrganizacijaId = projekt.Organizacija.Id;
                }
                projekt = HelperMethods.DodajAktivnosti(projekt, form);

                projekt = HelperMethods.DodajFinanciranja(projekt, form);

                projekt = HelperMethods.DodajDionike(projekt, form);

                projekt = HelperMethods.DodajPokazatelje(projekt, form);

                projekt.Uskladjenosti = new List<Uskladjenosti>();
                projekt = HelperMethods.DodajUskladjenosti(projekt, form);

                projekt = HelperMethods.DodajJavneNabave(projekt, form);

                projekt = HelperMethods.DodajRizike(projekt, form);

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

                projekt.DatumIzmjene = DateTime.Now;
                                
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
                foreach (var item in projekt.Rizici)
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
                foreach (var item in projekt.JavneNabave)
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
                foreach (var item in projekt.Uskladjenosti)
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

                foreach (var item in projekt.Dokumentacija)
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

                if (projekt.Kontakt.Id != 0 && projekt.Kontakt.Id != null)
                {
                    db.Entry(projekt.Kontakt).State = EntityState.Modified;
                }
                else
                {
                    db.Entry(projekt.Kontakt).State = EntityState.Added;
                }
                if (projekt.OdgovornaOsoba.Id != 0 && projekt.OdgovornaOsoba.Id != null)
                {
                    db.Entry(projekt.OdgovornaOsoba).State = EntityState.Modified;
                }
                else
                {
                    db.Entry(projekt.OdgovornaOsoba).State = EntityState.Added;
                }
                if (projekt.Organizacija.Id != 0 && projekt.Organizacija.Id != null)
                {
                    db.Entry(projekt.Organizacija).State = EntityState.Modified;
                }
                else
                {
                    db.Entry(projekt.Organizacija).State = EntityState.Added;
                }

                db.Entry(projekt).State = EntityState.Modified;

                if (submitButton.Equals("Pošalji"))
                {
                    projekt.DatumPredaje = DateTime.Now;
                    projekt.Poslano = true;
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
            List<Rizici> lsRizici = db.Rizici.Where(x => x.ProjektId == projekt.Id).ToList();
            List<int> lsRiziciIDs = lsRizici.Select(x => x.Id).ToList();
            projekt.Rizici = lsRizici;
            List<JavneNabave> lsJavneNabave = db.JavneNabave.Where(x => x.ProjektId == projekt.Id).ToList();
            List<int> lsJavneNabaveIDs = lsJavneNabave.Select(x => x.Id).ToList();
            projekt.JavneNabave = lsJavneNabave;
            List<Uskladjenosti> lsUskladjenosti = db.Uskladjenosti.Where(x => x.ProjektId == projekt.Id).ToList();
            List<int> lsUskladjenostiIDs = lsUskladjenosti.Select(x => x.Id).ToList();
            projekt.Uskladjenosti = lsUskladjenosti;
            //To do: Add file deletion

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
            foreach (var rizikId in lsRiziciIDs)
            {
                db.Rizici.Remove(db.Rizici.FirstOrDefault(x => x.Id == rizikId));
            }
            foreach (var javnaNabavaId in lsJavneNabaveIDs)
            {
                db.JavneNabave.Remove(db.JavneNabave.FirstOrDefault(x => x.Id == javnaNabavaId));
            }
            foreach (var uskladjenostId in lsUskladjenostiIDs)
            {
                db.Uskladjenosti.Remove(db.Uskladjenosti.FirstOrDefault(x => x.Id == uskladjenostId));
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

        public ActionResult DodajRizik(string sFirst, string sLast)
        {
            Rizici Rizik = new Rizici();
            return PartialView("RiziciPartial", Rizik);
        }

        public ActionResult DodajJavnuNabavu(string sFirst, string sLast)
        {
            JavneNabave JavnaNabava = new JavneNabave();
            return PartialView("JavneNabavePartial", JavnaNabava);
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

        public ActionResult GetKontaktiValues(string id)
        {
            int kontaktId = Convert.ToInt32(id);
            Kontakti kontakt = db.Kontakti.FirstOrDefault(x => x.Id == kontaktId);
            List<string> KontaktiValues = new List<string>();
            KontaktiValues.Add(kontakt.Ime);
            KontaktiValues.Add(kontakt.Email);
            KontaktiValues.Add(kontakt.BrojTelefona);
            KontaktiValues.Add(kontakt.Faks);
            return Json(KontaktiValues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrganizacijeValues(string id)
        {
            int organizacijaId = Convert.ToInt32(id);
            Organizacije organizacija = db.Organizacije.FirstOrDefault(x => x.Id == organizacijaId);
            List<string> Organizacije = new List<string>();
            Organizacije.Add(organizacija.Naziv);
            Organizacije.Add(organizacija.OIB);
            Organizacije.Add(organizacija.Adresa);
            Organizacije.Add(organizacija.Email);
            Organizacije.Add(organizacija.BrojTelefona);
            Organizacije.Add(organizacija.Faks);
            return Json(Organizacije, JsonRequestBehavior.AllowGet);
        }

       
        //public ActionResult StandardPDF(Projekti projekt)
        //{

        //    var makeCvSession = Session["makeCV"];

        //    var root = Server.MapPath("~/PDF/");
        //    var pdfname = String.Format("{0}.pdf", Guid.NewGuid().ToString());
        //    var path = Path.Combine(root, pdfname);
        //    path = Path.GetFullPath(path);

        //    var something = new Rotativa.ViewAsPdf("Details", new { id = projekt.Id }) { FileName = "cv.pdf", SaveOnServerPath = path };
        //    return something;

        //}
    }
}
