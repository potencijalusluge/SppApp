using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("RIZICI")]
    public class Rizici
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Display(Name = "Naziv rizika")]
        public string Naziv { get; set; }

        [StringLength(25)]
        [Display(Name = "Vrsta rizika")]
        public string Vrsta { get; set; }

        [StringLength(25)]
        [Display(Name = "Vjerojatnost rizika")]
        public string Vjerojatnost { get; set; }

        [Display(Name = "Odgovorna osoba")]
        public int? KontaktId { get; set; }
        [ForeignKey("KontaktId")]
        public virtual Kontakti Kontakt { get; set; }

        [StringLength(500)]
        [Display(Name = "Napomena")]
        public string Napomena { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}