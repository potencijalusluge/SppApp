﻿using System;
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
        public int? Id { get; set; }

        [Display(Name = "Ime i prezime")]
        [Required]
        [StringLength(150)]
        public string Ime { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail adresa nije u dobrom formatu.")]
        [Display(Name = "E-mail")]
        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefonski broj")]
        [StringLength(25)]
        public string BrojTelefona { get; set; }

        //Dodana polja - početak
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Faks")]
        [StringLength(25)]
        public string Faks { get; set; }
        //Dodana polja - kraj

        [Display(Name = "Korisnik")]
        public string UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Projekti> Projekti { get; set; }
    }
}