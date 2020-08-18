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
            GradjevinskeDozvole = new List<GradjevinskeDozvole>();
            OstalaDokumentacija = new List<OstalaDokumentacija>();
            Pokazatelji = new List<Pokazatelji>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv projekta je obavezan.")]
        [StringLength(250)]
        [Display(Name = "Naziv projekta")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Lokacija je obavezna.")]
        [StringLength(150)]
        public string Lokacija { get; set; }

        public int? OrganizacijaId { get; set; }
        [ForeignKey("OrganizacijaId")]
        public virtual Organizacije Organizacija { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status je obavezan.")]
        [StringLength(25)]
        public string StatusProjekta { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Početak projekta")]
        public DateTime? Pocetak { get; set; }

        [Display(Name = "Vrsta razvojnog projekta")]
        [StringLength(50)]
        public string VrstaProjekta { get; set; }

        [Display(Name = "Proglašen strateškim projektom RH")]
        public bool? ProglasenStrateskim { get; set; }

        [Required(ErrorMessage = "Faza projekta je obavezna.")]
        [StringLength(25)]
        [Display(Name = "Faza projekta")]
        public string Faza { get; set; }

        [StringLength(25)]
        [Display(Name = "Vlasnička dokumentacija")]
        public string VlasnickaDokumentacija { get; set; }

        [StringLength(25)]
        [Display(Name = "Studija izvodivosti")]
        public string StudijaIzvodivosti { get; set; }

        [StringLength(25)]
        [Display(Name = "Investicijska studija")]
        public string InvesticijskaStudija { get; set; }

        [StringLength(25)]
        [Display(Name = "Idejno rješenje")]
        public string IdejnoRjesenje { get; set; }

        [StringLength(25)]
        [Display(Name = "Lokacijska dozvola")]
        public string LokacijskaDozvola { get; set; }

        [Required(ErrorMessage = "Upravno područje je obavezno.")]
        [StringLength(150)]
        [Display(Name = "Upravno područje")]
        public string UpravnoPodrucje { get; set; }

        [StringLength(150)]
        public string Sektor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<GradjevinskeDozvole> GradjevinskeDozvole { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<OstalaDokumentacija> OstalaDokumentacija { get; set; }

        [Required(ErrorMessage = "Opis projekta je obavezan.")]
        [Display(Name = "Opis projekta")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Rezultati projekta su obavezni.")]
        [Display(Name = "Rezultati projekta")]
        public string Rezultati { get; set; }

        [Required(ErrorMessage = "Opći i specifični ciljevi projekta su obavezni.")]
        [Display(Name = "Opći i specifični ciljevi projekta")]
        public string OpciSpecificniCiljevi { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Kraj projekta")]
        public DateTime? Kraj { get; set; }

        [Required(ErrorMessage = "Izvor financiranja projekta je obavezan.")]
        [StringLength(250)]
        [Display(Name = "Izvor financiranja projekta")]
        public string IzvorFinanciranja { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage ="Procijenjena vrijednost mora biti veća od nule.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Procijenjena vrijednost projekta je obavezna.")]
        [Display(Name = "Procijenjena vrijednost projekta (HRK)")]
        public decimal ProcijenjenaVrijednost { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Procijenjeni troškovi moraju biti veći od nule.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Procijenjeni troškovi pripreme su obavezni.")]
        [Display(Name = "Procijenjeni troškovi pripreme (HRK)")]
        public decimal ProcijenjeniTroskoviPripreme { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Procijenjeni troškovi moraju biti veći od nule.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Procijenjeni troškovi provedbe su obavezni.")]
        [Display(Name = "Procijenjeni troškovi provedbe (HRK)")]
        public decimal ProcijenjeniTroskoviProvedbe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Aktivnosti> Aktivnosti { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Financiranja> Financiranja { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Dionici> Dionici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Pokazatelji> Pokazatelji { get; set; }

        public int? KontaktId { get; set; }
        [ForeignKey("KontaktId")]
        public virtual Kontakti Kontakt { get; set; }

        public bool? Upisano { get; set; }

        public bool? Ispravno { get; set; }

        [Display(Name = "Datum i vrijeme predaje")]
        public DateTime? DatumPredaje { get; set; }

        [Display(Name = "Korisnik")]
        public string UserId { get; set; }
    }
}
