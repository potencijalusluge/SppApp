using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("USKLADJENOSTI")]
    public class Uskladjenosti
    {
        [Key]
        public int Id { get; set; }

        public int Dubina { get; set; }

        [StringLength(500)]
        public string Naziv { get; set; }

        public bool Odabrano { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }
    }
}