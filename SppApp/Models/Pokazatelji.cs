using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("POKAZATELJI")]
    public class Pokazatelji
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Pokazatelj")]
        [StringLength(250)]
        public string Naziv { get; set; }

        [Display(Name = "Jedinica mjere")]
        [StringLength(25)]
        public string JedinicaMjere { get; set; }

        [Display(Name = "Broj jedinica")]
        public decimal? BrojJedinica { get; set; }

        [Display(Name = "Način ostvarenja")]
        [StringLength(500)]
        public string NacinOstvarenja { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}