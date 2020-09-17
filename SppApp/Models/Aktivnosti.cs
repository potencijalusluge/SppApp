using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("AKTIVNOSTI")]
    public class Aktivnosti
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Display(Name = "Opis aktivnosti")]
        public string Opis { get; set; }

        
        [StringLength(25)]
        [Display(Name = "Vrsta aktivnosti")]
        public string Vrsta { get; set; }

        [StringLength(25)]
        [Display(Name = "Jedinica mjere")]
        public string JedinicaMjere { get; set; }

        [Display(Name = "Broj jedinica")]
        public decimal? BrojJedinica { get; set; }

        [Display(Name = "Jedinična cijena")]
        [DataType(DataType.Currency)]
        public decimal? JedinicnaCijena { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Očekivani datum završetka")]
        public DateTime? DatumZavrsetka { get; set; }

        public bool? Zavrseno { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}