using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("KONTAKTI")]
    public class Kontakti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kontakti()
        {
            Projekti = new HashSet<Projekti>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno.")]
        [StringLength(150)]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [StringLength(150)]
        public string Prezime { get; set; }

        public int? OrganizacijaID { get; set; }
        [ForeignKey("OrganizacijaID")]
        public virtual Organizacije Organizacija { get; set; }

        [Required(ErrorMessage = "E-mail je obavezan.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Vaš E-mail kontakt")]
        [StringLength(250)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Broj telefona je obavezan.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Broj telefona")]
        [StringLength(25)]
        public string BrojTelefona { get; set; }

        [Display(Name = "Korisnik")]
        public string UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Projekti> Projekti { get; set; }
    }
}