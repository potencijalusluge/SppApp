using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("FINANCIRANJA")]
    public class Financiranja
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Naziv izvora")]
        [StringLength(250)]
        public string NazivIzvora { get; set; }

        [Display(Name = "Izvor financiranja")]
        [StringLength(150)]
        public string IzvorFinanciranja { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Iznos (HRK)")]
        public decimal? IznosHRK { get; set; }

        [Display(Name = "Izvor sufinaciranja")]
        [StringLength(150)]
        public string IzvorSufinanciranja { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}