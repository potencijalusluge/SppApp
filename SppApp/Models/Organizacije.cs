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
            Kontakt = new HashSet<Kontakti>();
            Projekt = new HashSet<Projekti>();
        }
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv organizacije je obavezan.")]
        [Display(Name = "Naziv organizacije")]
        [StringLength(150)]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Adresa je obavezna.")]
        [StringLength(250)]
        public string Adresa { get; set; }

        [Required(ErrorMessage = "Mjesto je obavezno.")]
        [StringLength(150)]
        public string Mjesto { get; set; }

        [Display(Name = "Država")]
        [StringLength(50)]
        public string Drzava { get; set; }

        [Display(Name = "Korisnik")]
        public string UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kontakti> Kontakt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Projekti> Projekt { get; set; }
    }
}