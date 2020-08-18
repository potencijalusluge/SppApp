using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("GRADJEVINSKE_DOZVOLE")]
    public class GradjevinskeDozvole
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Građevinska dozvola")]
        [StringLength(500)]
        public string Putanja { get; set; }

        public int? ProjektId { get; set; }
        [ForeignKey("ProjektId")]
        public virtual Projekti Projekt { get; set; }

        //To do
        [NotMapped]
        public HttpPostedFileBase Datoteka { get; set; }

    }
}