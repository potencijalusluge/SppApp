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
        public ActionResult Create(Projekti projekti, FormCollection form, HttpPostedFileBase[] dozvolaDatoteke, HttpPostedFileBase[] ostaleDatoteke, string submitButton)
        {
            if (!form["lokacija-input"].IsNullOrWhiteSpace())
            {
                projekti.Lokacija = form["lokacija-input"];
            }
            if (dozvolaDatoteke.Any(x => x!= null))
            {
                foreach (var itemDozvole in dozvolaDatoteke)
                {
                    if (itemDozvole != null)
                    {
                        GradjevinskeDozvole dozvola = new GradjevinskeDozvole();
                        string FileName = Path.GetFileNameWithoutExtension(itemDozvole.FileName);
                        string FileExtension = Path.GetExtension(itemDozvole.FileName);
                        FileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + FileName.Trim() + FileExtension;

                        dozvola.Naziv = itemDozvole.FileName;

                        dozvola.Putanja = Path.Combine(Server.MapPath("~/Files/") + FileName); 

                        dozvola.Datoteka = itemDozvole;
                        projekti.GradjevinskeDozvole.Add(dozvola);

                        dozvola.Datoteka.SaveAs(dozvola.Putanja);

                    }
                }
            }
            if (ostaleDatoteke.Any(x => x != null))
            {
                foreach (var itemOstale in ostaleDatoteke)
                {
                    if (itemOstale != null)
                    {
                        OstalaDokumentacija ostala = new OstalaDokumentacija();
                        string FileName = Path.GetFileNameWithoutExtension(itemOstale.FileName);
                        string FileExtension = Path.GetExtension(itemOstale.FileName);
                        FileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + FileName.Trim() + FileExtension;

                        ostala.Naziv = itemOstale.FileName;

                        ostala.Putanja = Path.Combine(Server.MapPath("~/Files/") + FileName);

                        ostala.Datoteka = itemOstale;
                        projekti.OstalaDokumentacija.Add(ostala);

                        ostala.Datoteka.SaveAs(ostala.Putanja);
                    }
                }
            }
            List<string> lsAktivnostiOpis = form.AllKeys.Where(x => x.StartsWith("AktivnostiLista[") && x.EndsWith("].Opis")).Distinct().ToList();
            foreach (var opis in lsAktivnostiOpis)
            {
                if (!form[opis].IsNullOrWhiteSpace())
                {
                    string sKlasa = opis.Replace("Opis", "");
                    Aktivnosti aktivnost = new Aktivnosti();
                    aktivnost.Opis = form[opis];
                    aktivnost.Vrsta = form[sKlasa + "Vrsta"];
                    aktivnost.JedinicaMjere = form[sKlasa + "JedinicaMjere"];
                    if (!form[sKlasa + "BrojJedinica"].IsNullOrWhiteSpace())
                    {
                        aktivnost.BrojJedinica = Decimal.Parse(form[sKlasa + "BrojJedinica"]);
                    }
                    if (!form[sKlasa + "JedinicnaCijena"].IsNullOrWhiteSpace())
                    {
                        aktivnost.JedinicnaCijena = Decimal.Parse(form[sKlasa + "JedinicnaCijena"]);
                    }
                    if (!form[sKlasa + "DatumZavrsetka"].IsNullOrWhiteSpace())
                    {
                        aktivnost.DatumZavrsetka = DateTime.Parse(form[sKlasa + "DatumZavrsetka"]);
                    }
                    projekti.Aktivnosti.Add(aktivnost);
                }
            }

            List<string> lsFinanciranjaNazivIzvora = form.AllKeys.Where(x => x.StartsWith("FinanciranjaLista[") && x.EndsWith("].NazivIzvora")).Distinct().ToList();
            foreach (var nazivIzvora in lsFinanciranjaNazivIzvora)
            {
                if (!form[nazivIzvora].IsNullOrWhiteSpace())
                {
                    string sKlasa = nazivIzvora.Replace("NazivIzvora", "");
                    Financiranja financiranje = new Financiranja();
                    financiranje.NazivIzvora = form[nazivIzvora];
                    financiranje.IzvorFinanciranja = form[sKlasa + "IzvorFinanciranja"];
                    if (!form[sKlasa + "IznosHRK"].IsNullOrWhiteSpace())
                    {
                        financiranje.IznosHRK = Decimal.Parse(form[sKlasa + "IznosHRK"]);
                    }
                    if (!form[sKlasa + "IznosEUR"].IsNullOrWhiteSpace())
                    {
                        financiranje.IznosEUR = Decimal.Parse(form[sKlasa + "IznosEUR"]);
                    }
                    financiranje.IzvorSufinanciranja = form[sKlasa + "IzvorSufinanciranja"];
                    projekti.Financiranja.Add(financiranje);
                }
            }

            List<string> lsDioniciNaziv = form.AllKeys.Where(x => x.StartsWith("DioniciLista[") && x.EndsWith("].Naziv")).Distinct().ToList();
            foreach (var naziv in lsDioniciNaziv)
            {
                if (!form[naziv].IsNullOrWhiteSpace())
                {
                    string sKlasa = naziv.Replace("Naziv", "");
                    Dionici dionik = new Dionici();
                    dionik.Naziv = form[naziv];
                    dionik.Vrsta = form[sKlasa + "Vrsta"];
                    dionik.Uloga = form[sKlasa + "Uloga"];

                    projekti.Dionici.Add(dionik);
                }
            }

            List<string> lsPokazateljiNaziv = form.AllKeys.Where(x => x.StartsWith("PokazateljiLista[") && x.EndsWith("].Naziv")).Distinct().ToList();
            foreach (var pokazateljNaziv in lsPokazateljiNaziv)
            {
                if (!form[pokazateljNaziv].IsNullOrWhiteSpace())
                {
                    string sKlasa = pokazateljNaziv.Replace("Naziv", "");
                    Pokazatelji pokazatelj = new Pokazatelji();
                    pokazatelj.Naziv = form[pokazateljNaziv];
                    pokazatelj.JedinicaMjere = form[sKlasa + "JedinicaMjere"];
                    if (!form[sKlasa + "BrojJedinica"].IsNullOrWhiteSpace())
                    {
                        pokazatelj.BrojJedinica = Decimal.Parse(form[sKlasa + "BrojJedinica"]);
                    }
                    pokazatelj.NacinOstvarenja = form[sKlasa + "NacinOstvarenja"];

                    projekti.Pokazatelji.Add(pokazatelj);
                }
            }

            string user = User.Identity.GetUserId();

            if (user!=null)
            {
                projekti.UserId = user;
                projekti.Kontakt.UserId = user;
                projekti.Organizacija.UserId = user;
                projekti.Kontakt.Organizacija.UserId = user;
            }            
            
            try
            {
                if (ModelState.IsValid)
                {
                    Session["projektID"] = projekti.Id;

                    if (submitButton.Equals("Pošalji"))
                    {
                        projekti.DatumPredaje = DateTime.Now;
                        projekti.Upisano = true; 
                        db.Projekti.Add(projekti);
                        db.SaveChanges();

                        HelperMethods.SendEmailNotification(projekti);

                        return RedirectToAction("Details", "Projekti", new { id = projekti.Id });
                    }
                    else
                    {
                        db.Projekti.Add(projekti);
                        db.SaveChanges();

                        return RedirectToAction("Index", "Projekti", new { id = projekti.Id });
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


            ViewBag.KontaktId = new SelectList(db.Kontakti, "Id", "Ime", projekti.KontaktId);
            ViewBag.OrganizacijaId = new SelectList(db.Organizacije, "Id", "Naziv", projekti.OrganizacijaId);
            return View(projekti);
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
            
            if (projekti.Lokacija != "Grad" && projekti.Lokacija !="Općina")
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
        public ActionResult Edit([Bind(Include = "Id,Naziv,Lokacija,OrganizacijaId,StatusProjekta,Pocetak,VrstaProjekta,ProglasenStrateskim,Faza,VlasnickaDokumentacija,StudijaIzvodivosti,InvesticijskaStudija,IdejnoRjesenje,LokacijskaDozvola,UpravnoPodrucje,Sektor,Opis,Rezultati,OpciSpecificniCiljevi,Kraj,IzvorFinanciranja,ProcijenjenaVrijednost,ProcijenjeniTroskoviPripreme,ProcijenjeniTroskoviProvedbe,KontaktId,Upisano,Ispravno")] Projekti projekti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projekti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KontaktId = new SelectList(db.Kontakti, "Id", "Ime", projekti.KontaktId);
            ViewBag.OrganizacijaId = new SelectList(db.Organizacije, "Id", "Naziv", projekti.OrganizacijaId);
            return View(projekti);
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

        public FileResult Download(string putanja)
        {
            string FileName = Path.GetFileName(putanja);

            return File(putanja, "application/force-download", FileName.Substring(15));
        }
    }
}
