using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Exchange.WebServices.Data;
using SppApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Drawing;
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
            service.Credentials = new WebCredentials(System.Configuration.ConfigurationManager.AppSettings["mailAccount"].ToString(), System.Configuration.ConfigurationManager.AppSettings["mailPassword"].ToString());
            service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            EmailMessage message = new EmailMessage(service);
            message.Subject = "SPUR - novi  projekt";
            message.Body = "<b>Naziv projekta:</b> " + projekt.Naziv + "<br>" + "<b>Naziv nositelja projekta:</b> " + projekt.Organizacija.Naziv + "<br>" + "<b>Status:</b> " + projekt.StatusProjekta + "<br>" + "<b>Modul:</b> " + projekt.Modul + "<br>" + "<b>Upravno područje:</b> " + projekt.UpravnoPodrucje + "<br>" + "<b>Ime i prezime:</b> " + projekt.Kontakt.Ime + "<br>" + "<br>" + "<b>Naziv organizacije:</b> " + projekt.Organizacija.Naziv;
            message.ToRecipients.Add("adrijana.jurilj@vpc.hr");


            //message.Attachments.AddFileAttachment();

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
                int i = 0;
                using (XmlReader reader = XmlReader.Create(putanja, settings))
                {
                    while (reader.Read())
                    {

                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName.Contains("Razina"))
                        {
                            Uskladjenosti uskladjenost = new Uskladjenosti()
                            {
                                Naziv = reader.GetAttribute("Naziv"),
                                XmlId = i,
                                Dubina = reader.Depth,
                                Odabrano = false
                            };
                            lsUskladjenosti.Add(uskladjenost);
                            i++;
                        };

                    }
                }
            }
            catch (Exception)
            {
            }
            return lsUskladjenosti;
        }

        public static Projekti DopuniUskladjenosti(Projekti projekt)
        {
            string putanja = Path.Combine(HttpContext.Current.Server.MapPath("~/Helpers/") + "Uskladjenosti.xml");
            List<int> lsPostojeceUskladjenosti = projekt.Uskladjenosti.Select(x => x.XmlId).ToList();
            List<Uskladjenosti> lsUskladjenosti = UcitajUskladjenosti();
            try
            {
                foreach (var uskladjenost in lsUskladjenosti)
                {
                    if (!lsPostojeceUskladjenosti.Contains(uskladjenost.XmlId))
                    {
                        projekt.Uskladjenosti.Add(uskladjenost);
                    }
                }                
                    
                    //using (XmlReader reader = XmlReader.Create(putanja, settings))
                //{
                //    int i = 0;
                //    while (reader.Read())
                //    {

                //        if (reader.NodeType == XmlNodeType.Element && reader.LocalName.Contains("Razina"))
                //        {
                //            Uskladjenosti uskladjenost = new Uskladjenosti()
                //            {
                //                Naziv = reader.GetAttribute("Naziv"),
                //                XmlId = i,
                //                Dubina = reader.Depth,
                //                Odabrano = false
                //            };
                //            if (lsPostojeceUskladjenosti.Contains(uskladjenost.XmlId))
                //            {
                //                uskladjenost.Odabrano = true;
                //            }
                //            lsUskladjenosti.Add(uskladjenost);
                //            i++;
                //        };

                //    }
                //}
                //projekt.Uskladjenosti = lsUskladjenosti;
            }
            catch (Exception)
            {
            }
            return projekt;
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
                    for (int j = 0; j < lsUskladjenosti[i - 1].Dubina - lsUskladjenosti[i].Dubina; j++)
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


            }

            sPartial += "</li>";
            sPartial += "</ul>";

            return sPartial; //Ne vrati indekse: 15, 17, 24, 26, 48, 50, 75, 109, 232, 396; 462, 476, 495, 706, 722 759, 765, 767, 
            //785, 788, 790, 799, 809, 811, 813, 819, 827, 845, 853, 856, 858, 872, 884, 891, 897, 921, 923, 926, 941, 953, 964, 
            //967, 1001, 1003
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

        public static Projekti DopuniDokumentaciju(Projekti projekt)
        {
            List<string> lsDokumentacija = new List<string> { "Planirana lokacija u prostornom planu", "Vlasnička dokumentacija", "Master plan", "Studija predizvodivosti", "Studija izvodivosti", "Cost/benefit analiza (analiza omjera troškova i korisnosti projekta)", "Rješenje o prihvatljivosti za okoliš", "Idejno rješenje", "Idejni projekt", "Glavni projekt", "Izvedbeni projekt", "Lokacijska dozvola", "Građevinska dozvola", "Poslovni plan", "Investicijska studija", "Mišljenje o uskladivosti s Naturom 2000", "Uporabna dozvola", "Natječajna dokumentacija", "Ostalo" };
            List<string> lsPostojecaDokumentacija = projekt.Dokumentacija.Select(x => x.Naziv).ToList();
            foreach (string sNazivDokumenta in lsDokumentacija)
            {
                if (!lsPostojecaDokumentacija.Contains(sNazivDokumenta))
                {
                    Dokumentacija dokument = new Dokumentacija()
                    {
                        Naziv = sNazivDokumenta
                    };
                    projekt.Dokumentacija.Add(dokument);
                }

            }
            return projekt;
        }

        public static Projekti DodajUsera(Projekti projekt, string user)
        {
            if (user != null)
            {
                if (projekt.UserId == null)
                {
                    projekt.UserId = user;
                }
                if (projekt.Kontakt.UserId == null)
                {
                    projekt.Kontakt.UserId = user;
                }
                if (projekt.OdgovornaOsoba.UserId == null)
                {
                    projekt.OdgovornaOsoba.UserId = user;
                }
                if (projekt.Organizacija.UserId == null)
                {
                    projekt.Organizacija.UserId = user;
                }
            }

            return projekt;
        }

        public static Projekti DodajAktivnosti(Projekti projekt, FormCollection form, out List<int> lsObrisaneAktivnosti)
        {
            lsObrisaneAktivnosti = new List<int>();
            List<string> lsAktivnostiOpis = form.AllKeys.Where(x => x.StartsWith("AktivnostiLista[") && x.EndsWith("].Opis")).Distinct().ToList();
            List<string> lsAktivnostiPredodabrano = form.AllKeys.Where(x => x.StartsWith("Aktivnosti[") && x.EndsWith("].Id")).Distinct().ToList();

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
                    decimal dBroj;
                    if (!form[sKlasa + "BrojJedinica"].IsNullOrWhiteSpace())
                    {
                        decimal.TryParse(form[sKlasa + "BrojJedinica"].ToString().Replace(".", ""), out dBroj);
                        aktivnost.BrojJedinica = dBroj;
                    }
                    if (!form[sKlasa + "JedinicnaCijena"].IsNullOrWhiteSpace())
                    {
                        decimal.TryParse(form[sKlasa + "JedinicnaCijena"].ToString().Replace(".", ""), out dBroj);
                        aktivnost.JedinicnaCijena = dBroj;
                    }
                    if (!form[sKlasa + "DatumZavrsetka"].IsNullOrWhiteSpace())
                    {
                        aktivnost.DatumZavrsetka = DateTime.Parse(form[sKlasa + "DatumZavrsetka"]);
                    }
                    aktivnost.ProjektId = projekt.Id;
                    
                    projekt.Aktivnosti.Add(aktivnost);
                }
            }
            projekt.Aktivnosti = projekt.Aktivnosti.Where(x => x.ProjektId != null).ToList();

            int iCount = projekt.Aktivnosti.Count;

            for (int i = 0; i < iCount; i++)
            {
                if (projekt.Aktivnosti[i].ProjektId == null)
                {
                    projekt.Aktivnosti.Remove(projekt.Aktivnosti[i]);
                }
            }

            foreach (var item in lsAktivnostiPredodabrano)
            {
                int iId;
                int.TryParse(form[item], out iId);
                if (!projekt.Aktivnosti.Any(x => x.Id == iId))
                {
                    lsObrisaneAktivnosti.Add(iId);
                }
            }
            
            return projekt;
        }

        public static Projekti DodajFinanciranja(Projekti projekt, FormCollection form, out List<int> lsObrisanaFinanciranja)
        {
            lsObrisanaFinanciranja = new List<int>();
            List<string> lsFinanciranjaNazivIzvora = form.AllKeys.Where(x => x.StartsWith("FinanciranjaLista[") && x.EndsWith("].NazivIzvora")).Distinct().ToList();
            List<string> lsFinanciranjaPredodabrano = form.AllKeys.Where(x => x.StartsWith("Financiranja[") && x.EndsWith("].Id")).Distinct().ToList();
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

                    decimal dBroj;
                    if (!form[sKlasa + "IznosHRK"].IsNullOrWhiteSpace())
                    {
                        decimal.TryParse(form[sKlasa + "IznosHRK"].ToString().Replace(".", ""), out dBroj);
                        financiranje.IznosHRK = dBroj;
                    }
                    financiranje.IzvorSufinanciranja = form[sKlasa + "IzvorSufinanciranja"];
                    financiranje.ProjektId = projekt.Id;

                    projekt.Financiranja.Add(financiranje);
                }
            }
            projekt.Financiranja = projekt.Financiranja.Where(x => x.ProjektId != null).ToList();

            int iCount = projekt.Financiranja.Count;

            for (int i = 0; i < iCount; i++)
            {
                if (projekt.Financiranja[i].ProjektId == null)
                {
                    projekt.Financiranja.Remove(projekt.Financiranja[i]);
                }
            }

            foreach (var item in lsFinanciranjaPredodabrano)
            {
                int iId;
                int.TryParse(form[item], out iId);
                if (!projekt.Financiranja.Any(x => x.Id == iId))
                {
                    lsObrisanaFinanciranja.Add(iId);
                }
            }
            return projekt;
        }

        public static Projekti DodajDionike(Projekti projekt, FormCollection form, out List<int> lsObrisaniDionici)
        {
            lsObrisaniDionici = new List<int>();
            List<string> lsDioniciNaziv = form.AllKeys.Where(x => x.StartsWith("DioniciLista[") && x.EndsWith("].Naziv")).Distinct().ToList();
            List<string> lsDioniciPredodabrano = form.AllKeys.Where(x => x.StartsWith("Dionici[") && x.EndsWith("].Id")).Distinct().ToList();
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
            projekt.Dionici = projekt.Dionici.Where(x => x.ProjektId != null).ToList();

            int iCount = projekt.Dionici.Count;

            for (int i = 0; i < iCount; i++)
            {
                if (projekt.Dionici[i].ProjektId == null)
                {
                    projekt.Dionici.Remove(projekt.Dionici[i]);
                }
            }

            foreach (var item in lsDioniciPredodabrano)
            {
                int iId;
                int.TryParse(form[item], out iId);
                if (!projekt.Dionici.Any(x => x.Id == iId))
                {
                    lsObrisaniDionici.Add(iId);
                }
            }
            return projekt;
        }

        public static Projekti DodajPokazatelje(Projekti projekt, FormCollection form, out List<int> lsObrisaniPokazatelji)
        {
            lsObrisaniPokazatelji = new List<int>();
            List<string> lsPokazateljiNaziv = form.AllKeys.Where(x => x.StartsWith("PokazateljiLista[") && x.EndsWith("].Naziv")).Distinct().ToList();
            List<string> lsPokazateljiPredodabrano = form.AllKeys.Where(x => x.StartsWith("Pokazatelji[") && x.EndsWith("].Id")).Distinct().ToList();
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

                    decimal dBroj;
                    if (!form[sKlasa + "IznosHRK"].IsNullOrWhiteSpace())
                    {
                        decimal.TryParse(form[sKlasa + "BrojJedinica"].ToString().Replace(".", ""), out dBroj);
                        pokazatelj.BrojJedinica = dBroj;
                    }
                    pokazatelj.NacinOstvarenja = form[sKlasa + "NacinOstvarenja"];
                    pokazatelj.ProjektId = projekt.Id;

                    projekt.Pokazatelji.Add(pokazatelj);
                }
            }
            projekt.Pokazatelji = projekt.Pokazatelji.Where(x => x.ProjektId != null).ToList();

            int iCount = projekt.Pokazatelji.Count;

            for (int i = 0; i < iCount; i++)
            {
                if (projekt.Pokazatelji[i].ProjektId == null)
                {
                    projekt.Pokazatelji.Remove(projekt.Pokazatelji[i]);
                }
            }

            foreach (var item in lsPokazateljiPredodabrano)
            {
                int iId;
                int.TryParse(form[item], out iId);
                if (!projekt.Pokazatelji.Any(x => x.Id == iId))
                {
                    lsObrisaniPokazatelji.Add(iId);
                }
            }

            return projekt;
        }

        public static Projekti DodajJavneNabave(Projekti projekt, FormCollection form, out List<int> lsObrisaneNabave)
        {
            lsObrisaneNabave = new List<int>();
            List<string> lsJavneNabaveNaziv = form.AllKeys.Where(x => x.StartsWith("JavneNabaveLista[") && x.EndsWith("].NazivPostupka")).Distinct().ToList();
            List<string> lsNabavePredodabrano = form.AllKeys.Where(x => x.StartsWith("JavneNabave[") && x.EndsWith("].Id")).Distinct().ToList();
            foreach (var javnaNabavaNaziv in lsJavneNabaveNaziv)
            {
                if (!form[javnaNabavaNaziv].IsNullOrWhiteSpace())
                {
                    string sKlasa = javnaNabavaNaziv.Replace("NazivPostupka", "");
                    JavneNabave javnaNabava = new JavneNabave();
                    if (form.AllKeys.Contains(sKlasa + "Id"))
                    {
                        javnaNabava.Id = int.Parse(form[sKlasa + "Id"]);
                        javnaNabava.ProjektId = int.Parse(form[sKlasa + "ProjektId"]);
                    }
                    javnaNabava.NazivPostupka = form[javnaNabavaNaziv];
                    javnaNabava.VrstaUgovora = form[sKlasa + "VrstaUgovora"];
                    javnaNabava.VrstaPostupka = form[sKlasa + "VrstaPostupka"];
                    decimal dBroj;
                    if (!form[sKlasa + "VrijednostUgovora"].IsNullOrWhiteSpace())
                    {
                        decimal.TryParse(form[sKlasa + "VrijednostUgovora"].ToString().Replace(".", ""), out dBroj);
                        javnaNabava.VrijednostUgovora = dBroj;
                    }
                    if (!form[sKlasa + "PlaniranaObjava"].IsNullOrWhiteSpace())
                    {
                        javnaNabava.PlaniranaObjava = DateTime.Parse(form[sKlasa + "PlaniranaObjava"]);
                    }
                    if (!form[sKlasa + "PlaniraniDatum"].IsNullOrWhiteSpace())
                    {
                        javnaNabava.PlaniraniDatum = DateTime.Parse(form[sKlasa + "PlaniraniDatum"]);
                    }
                    if (!form[sKlasa + "PlaniraniRok"].IsNullOrWhiteSpace())
                    {
                        javnaNabava.PlaniraniRok = DateTime.Parse(form[sKlasa + "PlaniraniRok"]);
                    }
                    if (!form[sKlasa + "Objava"].IsNullOrWhiteSpace())
                    {
                        javnaNabava.Objava = DateTime.Parse(form[sKlasa + "Objava"]);
                    }
                    if (!form[sKlasa + "Datum"].IsNullOrWhiteSpace())
                    {
                        javnaNabava.Datum = DateTime.Parse(form[sKlasa + "Datum"]);
                    }
                    if (!form[sKlasa + "Rok"].IsNullOrWhiteSpace())
                    {
                        javnaNabava.Rok = DateTime.Parse(form[sKlasa + "Rok"]);
                    }
                    javnaNabava.ProjektId = projekt.Id;

                    projekt.JavneNabave.Add(javnaNabava);
                }
            }
            projekt.JavneNabave = projekt.JavneNabave.Where(x => x.ProjektId != null).ToList();

            int iCount = projekt.JavneNabave.Count;

            for (int i = 0; i < iCount; i++)
            {
                if (projekt.JavneNabave[i].ProjektId == null)
                {
                    projekt.JavneNabave.Remove(projekt.JavneNabave[i]);
                }
            }

            foreach (var item in lsNabavePredodabrano)
            {
                int iId;
                int.TryParse(form[item], out iId);
                if (!projekt.JavneNabave.Any(x => x.Id == iId))
                {
                    lsObrisaneNabave.Add(iId);
                }
            }
            return projekt;
        }

        public static Projekti DodajRizike(Projekti projekt, FormCollection form, out List<int> lsObrisaniRizici)
        {
            lsObrisaniRizici = new List<int>();
            List<string> lsRiziciNaziv = form.AllKeys.Where(x => x.StartsWith("RiziciLista[") && x.EndsWith("].Naziv")).Distinct().ToList();
            List<string> lsRiziciPredodabrano = form.AllKeys.Where(x => x.StartsWith("Rizici[") && x.EndsWith("].Id")).Distinct().ToList();
            foreach (var rizikNaziv in lsRiziciNaziv)
            {
                if (!form[rizikNaziv].IsNullOrWhiteSpace())
                {
                    string sKlasa = rizikNaziv.Replace("Naziv", "");
                    Rizici rizik = new Rizici();
                    if (form.AllKeys.Contains(sKlasa + "Id"))
                    {
                        rizik.Id = int.Parse(form[sKlasa + "Id"]);
                        rizik.ProjektId = int.Parse(form[sKlasa + "ProjektId"]);
                    }
                    rizik.Naziv = form[rizikNaziv];
                    rizik.Vrsta = form[sKlasa + "Vrsta"];
                    rizik.Vjerojatnost = form[sKlasa + "Vjerojatnost"];
                    rizik.Napomena = form[sKlasa + "Napomena"];

                    rizik.ProjektId = projekt.Id;

                    projekt.Rizici.Add(rizik);
                }
            }
            projekt.Rizici = projekt.Rizici.Where(x => x.ProjektId != null).ToList();

            int iCount = projekt.Rizici.Count;

            for (int i = 0; i < iCount; i++)
            {
                if (projekt.Rizici[i].ProjektId == null)
                {
                    projekt.Rizici.Remove(projekt.Rizici[i]);
                }
            }

            foreach (var item in lsRiziciPredodabrano)
            {
                int iId;
                int.TryParse(form[item], out iId);
                if (!projekt.Rizici.Any(x => x.Id == iId))
                {
                    lsObrisaniRizici.Add(iId);
                }
            }
            return projekt;
        }

        public static Projekti DodajDatoteke(Projekti projekt, HttpPostedFileBase[] dokumentacija, FormCollection form)
        {
            List<string> lsDokumentacija = new List<string> { "Planirana lokacija u prostornom planu", "Vlasnička dokumentacija", "Master plan", "Studija predizvodivosti", "Studija izvodivosti", "Cost/benefit analiza (analiza omjera troškova i korisnosti projekta)", "Rješenje o prihvatljivosti za okoliš", "Idejno rješenje", "Idejni projekt", "Glavni projekt", "Izvedbeni projekt", "Lokacijska dozvola", "Građevinska dozvola", "Poslovni plan", "Investicijska studija", "Mišljenje o uskladivosti s Naturom 2000", "Uporabna dozvola", "Natječajna dokumentacija", "Ostalo" };
            List<string> lsDokumentacijaStatus = form.AllKeys.Where(x => x.StartsWith("DokumentacijaLista[") && x.EndsWith("].Status")).Distinct().ToList();

            for (int i = 0; i < lsDokumentacija.Count; i++)
            {
                Dokumentacija dokument = new Dokumentacija();
                dokument.Naziv = lsDokumentacija[i];
                if (lsDokumentacijaStatus.Count == i && i > 0 || lsDokumentacijaStatus.Count > i)
                {
                    string status = lsDokumentacijaStatus[i];
                    if (form[status] != "Nije potrebno" && !form[status].IsNullOrWhiteSpace())
                    {
                        string sKlasa = status.Replace("Status", "");
                        if (form.AllKeys.Contains(sKlasa + "Id"))
                        {
                            dokument.Id = int.Parse(form[sKlasa + "Id"]);
                            dokument.ProjektId = int.Parse(form[sKlasa + "ProjektId"]);
                        }
                        dokument.Status = form[status];
                        dokument.Klasa = form[sKlasa + "Klasa"];
                        dokument.UrBroj = form[sKlasa + "UrBroj"];
                        dokument.Tijelo = form[sKlasa + "Tijelo"];

                        decimal dBroj;
                        if (!form[sKlasa + "DatumZavrsetka"].IsNullOrWhiteSpace())
                        {
                            dokument.DatumZavrsetka = DateTime.Parse(form[sKlasa + "DatumZavrsetka"]);
                        }
                        if (!form[sKlasa + "Vrijednost"].IsNullOrWhiteSpace())
                        {
                            decimal.TryParse(form[sKlasa + "Vrijednost"].ToString().Replace(".", ""), out dBroj);
                            dokument.Vrijednost = dBroj;
                        }

                        dokument.ProjektId = projekt.Id;
                    }
                }

                if (dokumentacija != null)
                {

                    if (dokumentacija[i] != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(dokumentacija[i].FileName);
                        string FileExtension = Path.GetExtension(dokumentacija[i].FileName);
                        FileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + FileName.Trim() + FileExtension;

                        dokument.ImeDatoteke = dokumentacija[i].FileName;

                        dokument.Putanja = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/") + FileName);

                        dokument.Datoteka = dokumentacija[i];

                        dokument.ProjektId = projekt.Id;

                        dokument.Datoteka.SaveAs(dokument.Putanja);
                    }
                }

                if (dokument.ProjektId != null)
                {

                    projekt.Dokumentacija.Add(dokument);
                }

            }
            return projekt;
        }


        public static Projekti DodajUskladjenosti(Projekti projekt, FormCollection form)
        {
            List<Uskladjenosti> lsUskladjenosti = UcitajUskladjenosti();
            List<string> lsUskladjenostiOdabrano = form.AllKeys.Where(x => x.StartsWith("Uskladjenosti[") && x.EndsWith("].Odabrano")).Distinct().ToList();
            List<Uskladjenosti> lsUskladjenostiProjekt = new List<Uskladjenosti>();
            List<int> lsUskladjenostiIdProjekt = new List<int>();

            // prvi set: 0 - 444
            //drugi set: 445 - 754
            // treći set: 755 - 1031

            for (int i = 0; i < lsUskladjenostiOdabrano.Count; i++)
            {
                if (form[lsUskladjenostiOdabrano[i]].Contains("true"))
                {
                    Uskladjenosti uskladjenost = new Uskladjenosti()
                    {
                        Naziv = lsUskladjenosti[i].Naziv,
                        XmlId = i,
                        Dubina = lsUskladjenosti[i].Dubina,
                        Odabrano = true,
                        ProjektId = projekt.Id,
                    };
                    lsUskladjenostiProjekt.Add(uskladjenost);
                    lsUskladjenostiIdProjekt.Add(i);
                }
            }
            projekt.Uskladjenosti = lsUskladjenostiProjekt;

            return projekt;
        }

        public static Projekti AzurirajUskladjenosti(Projekti projekt, FormCollection form, out List<int> lsObrisaneUskladjenosti)
        {
            lsObrisaneUskladjenosti = new List<int>();
            List<string> lsUskladjenostiOdabrano = form.AllKeys.Where(x => x.StartsWith("Uskladjenosti[") && x.EndsWith("].Id")).Distinct().ToList();

            // prvi set: 0 - 444
            //drugi set: 445 - 754
            // treći set: 755 - 1031

            for (int i = 0; i < lsUskladjenostiOdabrano.Count; i++)
            {
                if (!string.IsNullOrEmpty(form[lsUskladjenostiOdabrano[i]]) && !string.IsNullOrEmpty(lsUskladjenostiOdabrano[i]))
                {
                    int iId;
                    int iXmlId;
                    int.TryParse(lsUskladjenostiOdabrano[i].Substring(lsUskladjenostiOdabrano[i].IndexOf('[') + 1, lsUskladjenostiOdabrano[i].IndexOf(']') - lsUskladjenostiOdabrano[i].IndexOf('[') - 1), out iXmlId);
                    int.TryParse(form[lsUskladjenostiOdabrano[i]], out iId);
                    if (projekt.Uskladjenosti.ElementAtOrDefault(iXmlId) != null)
                    {
                        projekt.Uskladjenosti[iXmlId].Id = iId;
                    }
                    else
                    {
                        lsObrisaneUskladjenosti.Add(iId);
                    }
                }
            }

            return projekt;
        }
        /// <summary>
        /// Compares contacts to avoid surplus in database
        /// </summary>
        /// <param name="projekt"></param>
        /// <returns>Projekt object with distinct contact</returns>
        public static Projekti UsporediKontakte(Projekti projekt)
        {
            if (projekt.Kontakt.Ime == projekt.OdgovornaOsoba.Ime && projekt.Kontakt.Email == projekt.OdgovornaOsoba.Email && projekt.Kontakt.BrojTelefona == projekt.OdgovornaOsoba.BrojTelefona && projekt.Kontakt.Faks == projekt.OdgovornaOsoba.Faks)
            {
                projekt.Kontakt = projekt.OdgovornaOsoba;
            }

            return projekt;
        }
    }
}