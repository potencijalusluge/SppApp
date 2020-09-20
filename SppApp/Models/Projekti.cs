using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SppApp.Models
{
    [Table("PROJEKTI")]
    public class Projekti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Projekti()
        {
            Aktivnosti = new List<Aktivnosti>();
            Dionici = new List<Dionici>();
            Financiranja = new List<Financiranja>();
            Dokumentacija = new List<Dokumentacija>();
            Pokazatelji = new List<Pokazatelji>();
            Uskladjenosti = new List<Uskladjenosti>();
            JavneNabave = new List<JavneNabave>();
            Rizici = new List<Rizici>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv projekta je obavezan.")]
        [StringLength(500)]
        [Display(Name = "Naziv projekta")]
        public string Naziv { get; set; }

        [StringLength(250)]
        [Display(Name = "Akronim / Skraćeni naziv projekta")]
        public string Akronim { get; set; }

        [StringLength(500)]
        [Display(Name = "Dio većeg projekta")]
        public string DioVecegProjekta { get; set; }

        [Display(Name = "Lokacija provedbe projekta")]
        [Required(ErrorMessage = "Lokacija provedbe projekta je obavezna.")]
        [StringLength(250)]
        public string Lokacija { get; set; }

        [Required(ErrorMessage = "Modul je obavezan.")]
        [StringLength(25)]
        public string Modul { get; set; }

        [Display(Name = "Status projekta")]
        [Required(ErrorMessage = "Status je obavezan.")]
        [StringLength(25)]
        public string StatusProjekta { get; set; }

        [Required(ErrorMessage = "Upravno područje je obavezno.")]
        [StringLength(150)]
        [Display(Name = "Područje")]
        public string UpravnoPodrucje { get; set; }

        [Display(Name = "Vrsta razvojnog projekta")]
        [StringLength(50)]
        public string VrstaProjekta { get; set; }

        [Display(Name = "Tip razvojnog projekta")]
        [StringLength(50)]
        public string TipProjekta { get; set; }

        [Display(Name = "Proglašen strateškim projektom RH")]
        public bool? ProglasenStrateskim { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Planirani početak provedbe")]
        public DateTime? Pocetak { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Planirani završetak provedbe")]
        public DateTime? Kraj { get; set; }

        //[Range(double.Epsilon, double.MaxValue, ErrorMessage = "Procijenjena vrijednost mora biti veća od nule.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Procijenjena vrijednost projekta je obavezna.")]
        [Display(Name = "Procijenjena vrijednost projekta (HRK)")]
        public decimal ProcijenjenaVrijednostHRK { get; set; }

        //[Range(double.Epsilon, double.MaxValue, ErrorMessage = "Procijenjeni troškovi moraju biti veći od nule.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Procijenjeni troškovi pripreme su obavezni.")]
        [Display(Name = "Procijenjeni troškovi pripreme (HRK)")]
        public decimal ProcijenjeniTroskoviPripremeHRK { get; set; }

        //[Range(double.Epsilon, double.MaxValue, ErrorMessage = "Procijenjeni troškovi moraju biti veći od nule.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Procijenjeni troškovi provedbe su obavezni.")]
        [Display(Name = "Procijenjeni troškovi provedbe (HRK)")]
        public decimal ProcijenjeniTroskoviProvedbeHRK { get; set; }

        public int? OrganizacijaId { get; set; }
        [ForeignKey("OrganizacijaId")]
        public virtual Organizacije Organizacija { get; set; }

        public int? KontaktId { get; set; }
        [ForeignKey("KontaktId")]
        public virtual Kontakti Kontakt { get; set; }

        public int? OdgovornaOsobaId { get; set; }
        [ForeignKey("OdgovornaOsobaId")]
        public virtual Kontakti OdgovornaOsoba { get; set; }

        [Display(Name = "Prekogranični projekt")]
        public bool? PrekogranicniProjekt { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Opis projekta je obavezan.")]
        [Display(Name = "Opis projekta")]
        public string Opis { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Rezultati projekta su obavezni.")]
        [Display(Name = "Rezultati projekta")]
        public string Rezultati { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Opći i specifični ciljevi projekta su obavezni.")]
        [Display(Name = "Opći i specifični ciljevi projekta")]
        public string OpciSpecificniCiljevi { get; set; }

        [StringLength(500)]
        [Display(Name = "Dodatne napomene")]
        public string DodatneNapomene { get; set; }

        [StringLength(500)]
        [Display(Name = "Web stranica")]
        public string WebStranica { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Dokumentacija> Dokumentacija { get; set; }

        [Display(Name = "Prijavljen kao strateški projekt RH")]
        public bool? PrijavljenStrateski { get; set; } 

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Uskladjenosti> Uskladjenosti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Aktivnosti> Aktivnosti { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Financiranja> Financiranja { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Dionici> Dionici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]        
        public virtual List<Pokazatelji> Pokazatelji { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]        
        public virtual List<JavneNabave> JavneNabave { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]        
        public virtual List<Rizici> Rizici { get; set; }

        public bool? Poslano { get; set; }

        public bool? UpisanoSPUR { get; set; }

        public bool? Doraditi { get; set; }

        [Display(Name = "Datum i vrijeme zadnje izmjene")]
        public DateTime? DatumIzmjene { get; set; }

        [Display(Name = "Datum i vrijeme predaje")]
        public DateTime? DatumPredaje { get; set; }

        [Display(Name = "Korisnik")]
        public string UserId { get; set; }
    }
}
