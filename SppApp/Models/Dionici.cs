using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("DIONICI")]
    public class Dionici
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Naziv subjekta")]
        [StringLength(250)]
        public string Naziv { get; set; }

        [Display(Name = "Vrsta dionika")]
        [StringLength(50)]
        public string Vrsta { get; set; }

        [Display(Name = "Uloga na projektu")]
        [StringLength(250)]
        public string Uloga { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}