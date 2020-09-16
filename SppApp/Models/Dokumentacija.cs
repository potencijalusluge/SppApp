using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("DOKUMENTACIJA")]
    public class Dokumentacija
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Naziv")]
        [StringLength(500)]
        public string Naziv { get; set; }

        [Display(Name = "Status")]
        [StringLength(50)]
        public string Status { get; set; }

        [Display(Name = "Klasa")]
        [StringLength(50)]
        public string Klasa { get; set; }

        [Display(Name = "Urudžbeni broj")]
        [StringLength(50)]
        public string UrBroj { get; set; }

        [Display(Name = "Tijelo koje izdaje dokument")]
        [StringLength(50)]
        public string Tijelo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Očekivani datum završetka")]
        public DateTime? DatumZavrsetka { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Procijenjena vrijednost")]
        public decimal? Vrijednost { get; set; }

        public string ImeDatoteke { get; set; } //Treba ili ne?

        public string Putanja { get; set; }

        [NotMapped]
        public HttpPostedFileBase Datoteka { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}