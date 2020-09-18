using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("ORGANIZACIJE")]
    public class Organizacije
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Organizacije()
        {
            Projekt = new HashSet<Projekti>();
        }
        [Key]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Naziv nositelja projekta je obavezan.")]
        [Display(Name = "Naziv organizacije")]
        [StringLength(150)]
        public string Naziv { get; set; }

        //Dodana polja - početak
        [Required(ErrorMessage = "OIB nositelja projekta je obavezan.")]
        [Display(Name = "OIB")]
        [StringLength(15)]
        public string OIB { get; set; }

        [Required(ErrorMessage = "Telefonski broj nositelja projekta je obavezan.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefonski broj")]
        [StringLength(25)]
        public string BrojTelefona { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Faks")]
        [StringLength(25)]
        public string Faks { get; set; }
        
        [Required(ErrorMessage = "E-mail nositelja projekta je obavezan.")]

        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        [StringLength(250)]
        public string Email { get; set; }
        //Dodana polja - kraj

        [Required(ErrorMessage = "Adresa nositelja projekta je obavezna.")]
        [StringLength(250)]
        public string Adresa { get; set; }

        [Display(Name = "Korisnik")]
        public string UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Projekti> Projekt { get; set; }
    }
}