using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("JAVNE_NABAVE")]
    public class JavneNabave
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Display(Name = "Naziv postupka")]
        public string NazivPostupka { get; set; }

        [StringLength(50)]
        [Display(Name = "Vrsta ugovora")]
        public string VrstaUgovora { get; set; }

        [StringLength(250)]
        [Display(Name = "Vrsta postupka")]
        public string VrstaPostupka { get; set; }

        [Display(Name = "Vrijednost ugovora")]
        [DataType(DataType.Currency)]
        public decimal? VrijednostUgovora { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Planirana objava")]
        public DateTime? PlaniranaObjava { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Objava")]
        public DateTime? Objava { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Planirani rok dostave")]
        public DateTime? PlaniraniRok { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Rok dostave")]
        public DateTime? Rok { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Planirani datum ugovaranja")]
        public DateTime? PlaniraniDatum { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum ugovaranja")]
        public DateTime? Datum { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}