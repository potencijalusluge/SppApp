using Microsoft.AspNet.Identity;
using Microsoft.Exchange.WebServices.Data;
using SppApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            message.Body = "<b>Naziv projekta:</b> " + projekt.Naziv + "<br>" + "<b>Naziv nositelja projekta:</b> " + projekt.Organizacija.Naziv + "<br>" + "<b>Status:</b> " + projekt.StatusProjekta + "<br>" + "<b>Faza projekta:</b> " + projekt.Faza + "<br>" + "<b>Upravno područje:</b> " + projekt.UpravnoPodrucje + "<br>" + "<b>Ime:</b> " + projekt.Kontakt.Ime + "<br>" + "<b>Prezime:</b> " + projekt.Kontakt.Prezime + "<br>" + "<b>Naziv organizacije:</b> " + projekt.Kontakt.Organizacija.Naziv;
            message.ToRecipients.Add("adrijana.jurilj@vpc.hr");

            //To do: Uncomment this
            //message.SendAndSaveCopy();
        }
        
        public static void GeneratePDF()
        {

        }
    }
}