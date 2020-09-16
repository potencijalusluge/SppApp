using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Exchange.WebServices.Data;
using SppApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace SppApp.Helpers
{
    public class HelperMethods
    {
        public static void SendEmailNotification(Projekti projekt)
        {
            ExchangeService service = new ExchangeService();
            service.Credentials = new WebCredentials("obavijest@vpc.pohrana.com.hr", "1029384756abc");
            service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            EmailMessage message = new EmailMessage(service);
            message.Subject = "SPUR - novi  projekt";
            message.Body = "<b>Naziv projekta:</b> " + projekt.Naziv + "<br>" + "<b>Naziv nositelja projekta:</b> " + projekt.Organizacija.Naziv + "<br>" + "<b>Status:</b> " + projekt.StatusProjekta + "<br>" + "<b>Modul:</b> " + projekt.Modul + "<br>" + "<b>Upravno područje:</b> " + projekt.UpravnoPodrucje + "<br>" + "<b>Ime:</b> " + projekt.Kontakt.Ime + "<br>" + "<b>Prezime:</b> " + projekt.Kontakt.Prezime + "<br>" + "<b>Naziv organizacije:</b> " + projekt.Kontakt.Organizacija.Naziv;
            message.ToRecipients.Add("adrijana.jurilj@vpc.hr");

            //To do: Uncomment this
            message.SendAndSaveCopy();
        }

        public static List<Uskladjenosti> UcitajUskladjenosti()
        {
            string putanja = Path.Combine(HttpContext.Current.Server.MapPath("~/Helpers/") + "Uskladjenosti.xml");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            List<Uskladjenosti> lsUskladjenosti = new List<Uskladjenosti>();
            try
            {
                using (XmlReader reader = XmlReader.Create(putanja, settings))
                {
                    while (reader.Read())
                    {

                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName.Contains("Razina"))
                        {
                            Uskladjenosti uskladjenost = new Uskladjenosti()
                            {
                                Naziv = reader.GetAttribute("Naziv"),
                                Dubina = reader.Depth,
                                Odabrano = false
                            };
                            lsUskladjenosti.Add(uskladjenost);
                        };

                    }
                }
            }
            catch (Exception)
            {
            }
            return lsUskladjenosti;
        }

        public static string KreirajUskladjenostiPartial(List<Uskladjenosti> lsUskladjenosti)
        {
            string sPartial = "<ul id='myUL'><li><span class='box'>@Html.CheckBoxFor(model => model.Uskladjenosti[445].Odabrano) &nbsp; @Model.Uskladjenosti[445].Naziv</span> <ul class='nested'>";
            List<int> lsPocni = new List<int>();

            //Od prvog do predzadnjeg 

            // prvi set: 0 - 444
            //drugi set: 445 - 754
            // treći set: 755 - 1031
            for (int i = 756; i < 1030; i++)
            {
                string sClosed = "</ul></li>";
                if (lsUskladjenosti[i].Dubina < lsUskladjenosti[i - 1].Dubina)
                {
                    for (int j = 0; j < lsUskladjenosti[i-1].Dubina - lsUskladjenosti[i].Dubina; j++)
                    {
                        sPartial += sClosed;
                        
                    }
                    if (lsUskladjenosti[i].Dubina < lsUskladjenosti[i + 1].Dubina)
                    {
                        sPartial += "<li><span class='box'>@Html.CheckBoxFor(model => model.Uskladjenosti[" + i + "].Odabrano) &nbsp; @Model.Uskladjenosti[" + i + "].Naziv</span> <ul class='nested'>";
                    }
                    else
                    {
                        sPartial += "<li> @Html.CheckBoxFor(model => model.Uskladjenosti[" + i + "].Odabrano) &nbsp; @Model.Uskladjenosti[" + i + "].Naziv </li >";
                    }
                }
                else if (lsUskladjenosti[i].Dubina < lsUskladjenosti[i + 1].Dubina)
                {
                    sPartial += "<li><span class='box'>@Html.CheckBoxFor(model => model.Uskladjenosti[" + i + "].Odabrano) &nbsp; @Model.Uskladjenosti[" + i + "].Naziv</span> <ul class='nested'>";
                }
                else if (lsUskladjenosti[i].Dubina == lsUskladjenosti[i + 1].Dubina || lsUskladjenosti[i].Dubina == lsUskladjenosti[i - 1].Dubina)
                {
                    sPartial += "<li> @Html.CheckBoxFor(model => model.Uskladjenosti[" + i + "].Odabrano) &nbsp; @Model.Uskladjenosti[" + i + "].Naziv </li >";
                }
                
                //if (lsUskladjenosti[i-1].Dubina < lsUskladjenosti[i].Dubina)
                //{
                //    sPartial += "<ul class='nested'>";
                //}
                //if (lsUskladjenosti[i].Dubina > lsUskladjenosti[i-1].Dubina)
                //{
                //    sPartial += "<li>@Model.Uskladjenosti[" + i + "].Naziv</li>";
                //    //sPartial += "<li><span class='box'> @Model.Uskladjenosti[" + i + "].Naziv</span>";
                //}
                //else if (lsUskladjenosti[i].Dubina == lsUskladjenosti[i - 1].Dubina)
                //{
                //    sPartial += "<li>" + "@Model.Uskladjenosti[" + i + "].Naziv</li>";
                //}
                //else
                //{
                //    for (int j = 0; j < lsUskladjenosti[i-1].Dubina - lsUskladjenosti[i].Dubina; j++)
                //    {
                //        sPartial += "</ul>";
                //        sPartial += "</li>";
                //    }
                //    sPartial += "<li><span class='box'>" + "@Model.Uskladjenosti[" + i + "].Naziv</span></li>";
                //}
            }

            sPartial += "</li>";
            sPartial += "</ul>";

            return sPartial;
        }
        public static Projekti UcitajDokumentaciju(Projekti projekt)
        {
            List<string> lsDokumentacija = new List<string> { "Planirana lokacija u prostornom planu", "Vlasnička dokumentacija", "Master plan", "Studija predizvodivosti", "Studija izvodivosti", "Cost/benefit analiza (analiza omjera troškova i korisnosti projekta)", "Rješenje o prihvatljivosti za okoliš", "Idejno rješenje", "Idejni projekt", "Glavni projekt", "Izvedbeni projekt", "Lokacijska dozvola", "Građevinska dozvola", "Poslovni plan", "Investicijska studija", "Mišljenje o uskladivosti s Naturom 2000", "Uporabna dozvola", "Natječajna dokumentacija", "Ostalo" };
            foreach (string sNazivDokumenta in lsDokumentacija)
            {
                Dokumentacija dokument = new Dokumentacija()
                {
                    Naziv = sNazivDokumenta
                };
                projekt.Dokumentacija.Add(dokument);
            }
            return projekt;
        }

        public static Projekti DodajCustomLokaciju(Projekti projekt, FormCollection form)
        {
            if (!form["lokacija-input"].IsNullOrWhiteSpace())
            {
                projekt.Lokacija = form["lokacija-input"];
            }

            return projekt;
        }

        public static Projekti DodajUsera(Projekti projekt, string user)
        {
            if (user != null)
            {
                projekt.UserId = user;
                projekt.Kontakt.UserId = user;
                projekt.Organizacija.UserId = user;
                projekt.Kontakt.Organizacija.UserId = user;
            }

            return projekt;
        }

        public static Projekti DodajAktivnosti(Projekti projekt, FormCollection form)
        {
            List<string> lsAktivnostiOpis = form.AllKeys.Where(x => x.StartsWith("AktivnostiLista[") && x.EndsWith("].Opis")).Distinct().ToList();
            foreach (var opis in lsAktivnostiOpis)
            {
                if (!form[opis].IsNullOrWhiteSpace())
                {
                    string sKlasa = opis.Replace("Opis", "");
                    Aktivnosti aktivnost = new Aktivnosti();
                    if (form.AllKeys.Contains(sKlasa + "Id"))
                    {
                        aktivnost.Id = int.Parse(form[sKlasa + "Id"]);
                        aktivnost.ProjektId = int.Parse(form[sKlasa + "ProjektId"]);
                    }
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
                    aktivnost.ProjektId = projekt.Id;

                    projekt.Aktivnosti.Add(aktivnost);
                }
            }

            return projekt;
        }

        public static Projekti DodajFinanciranja(Projekti projekt, FormCollection form)
        {
            List<string> lsFinanciranjaNazivIzvora = form.AllKeys.Where(x => x.StartsWith("FinanciranjaLista[") && x.EndsWith("].NazivIzvora")).Distinct().ToList();
            foreach (var nazivIzvora in lsFinanciranjaNazivIzvora)
            {
                if (!form[nazivIzvora].IsNullOrWhiteSpace())
                {
                    string sKlasa = nazivIzvora.Replace("NazivIzvora", "");
                    Financiranja financiranje = new Financiranja();
                    if (form.AllKeys.Contains(sKlasa + "Id"))
                    {
                        financiranje.Id = int.Parse(form[sKlasa + "Id"]);
                        financiranje.ProjektId = int.Parse(form[sKlasa + "ProjektId"]);
                    }
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
                    financiranje.ProjektId = projekt.Id;

                    projekt.Financiranja.Add(financiranje);
                }
            }

            return projekt;
        }

        public static Projekti DodajDionike(Projekti projekt, FormCollection form)
        {
            List<string> lsDioniciNaziv = form.AllKeys.Where(x => x.StartsWith("DioniciLista[") && x.EndsWith("].Naziv")).Distinct().ToList();
            foreach (var naziv in lsDioniciNaziv)
            {
                if (!form[naziv].IsNullOrWhiteSpace())
                {
                    string sKlasa = naziv.Replace("Naziv", "");
                    Dionici dionik = new Dionici();
                    if (form.AllKeys.Contains(sKlasa + "Id"))
                    {
                        dionik.Id = int.Parse(form[sKlasa + "Id"]);
                        dionik.ProjektId = int.Parse(form[sKlasa + "ProjektId"]);
                    }
                    dionik.Naziv = form[naziv];
                    dionik.Vrsta = form[sKlasa + "Vrsta"];
                    dionik.Uloga = form[sKlasa + "Uloga"];
                    dionik.ProjektId = projekt.Id;

                    projekt.Dionici.Add(dionik);
                }
            }

            return projekt;
        }

        public static Projekti DodajPokazatelje(Projekti projekt, FormCollection form)
        {
            List<string> lsPokazateljiNaziv = form.AllKeys.Where(x => x.StartsWith("PokazateljiLista[") && x.EndsWith("].Naziv")).Distinct().ToList();
            foreach (var pokazateljNaziv in lsPokazateljiNaziv)
            {
                if (!form[pokazateljNaziv].IsNullOrWhiteSpace())
                {
                    string sKlasa = pokazateljNaziv.Replace("Naziv", "");
                    Pokazatelji pokazatelj = new Pokazatelji();
                    if (form.AllKeys.Contains(sKlasa + "Id"))
                    {
                        pokazatelj.Id = int.Parse(form[sKlasa + "Id"]);
                        pokazatelj.ProjektId = int.Parse(form[sKlasa + "ProjektId"]);
                    }
                    pokazatelj.Naziv = form[pokazateljNaziv];
                    pokazatelj.JedinicaMjere = form[sKlasa + "JedinicaMjere"];
                    if (!form[sKlasa + "BrojJedinica"].IsNullOrWhiteSpace())
                    {
                        pokazatelj.BrojJedinica = Decimal.Parse(form[sKlasa + "BrojJedinica"]);
                    }
                    pokazatelj.NacinOstvarenja = form[sKlasa + "NacinOstvarenja"];
                    pokazatelj.ProjektId = projekt.Id;

                    projekt.Pokazatelji.Add(pokazatelj);
                }
            }

            return projekt;
        }

        public static Projekti DodajGradjevinskeDozvole(Projekti projekt, HttpPostedFileBase[] dozvolaDatoteke)
        {
            if (dozvolaDatoteke.Any(x => x != null))
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

                        dozvola.Putanja = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/") + FileName);

                        dozvola.Datoteka = itemDozvole;

                        dozvola.ProjektId = projekt.Id;

                        projekt.GradjevinskeDozvole.Add(dozvola);

                        dozvola.Datoteka.SaveAs(dozvola.Putanja);

                    }
                }
            }
            return projekt;
        }

        public static Projekti DodajOstaluDokumentaciju(Projekti projekt, HttpPostedFileBase[] ostaleDatoteke)
        {
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

                        ostala.Putanja = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/") + FileName);

                        ostala.Datoteka = itemOstale;

                        ostala.ProjektId = projekt.Id;

                        projekt.OstalaDokumentacija.Add(ostala);

                        ostala.Datoteka.SaveAs(ostala.Putanja);
                    }
                }
            }
            return projekt;
        }

    }
}