using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("LOKACIJE")]
    public class Lokacije
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "NUTS2")]
        [StringLength(250)]
        public string NUTS2 { get; set; }

        [Display(Name = "žUPANIJA")]
        [StringLength(250)]
        public string Zupanija { get; set; }

        [Display(Name = "JLS")]
        [StringLength(250)]
        public string JLS { get; set; }

        [Display(Name = "Naselje")]
        [StringLength(250)]
        public string Naselje { get; set; }

        [Display(Name = "Adresa")]
        [StringLength(250)]
        public string Adresa { get; set; }

        [Display(Name = "Koordinatna širina")]
        [StringLength(250)]
        public string KoordSirina { get; set; }

        [Display(Name = "Koordinatna dužina")]
        [StringLength(250)]
        public string KoordDuzina { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}