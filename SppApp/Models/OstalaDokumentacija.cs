using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("OSTALA_DOKUMENTACIJA")]
    public class OstalaDokumentacija
    {        
        [Key]
        public int Id { get; set; }

        [Display(Name = "Ostala dokumentacija")]
        [StringLength(500)]
        public string Putanja { get; set; }

        [NotMapped]
        public HttpPostedFileBase Datoteka { get; set; }

        [StringLength(250)]
        public string Opis { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}