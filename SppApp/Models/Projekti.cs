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

        //Dodana polja - početak
        [StringLength(250)]
        [Display(Name = "Akronim / Skraćeni naziv projekta")]
        public string Akronim { get; set; }

        //string ToDo
        [StringLength(500)]
        [Display(Name = "Dio većeg projekta")]
        public string DioVecegProjekta { get; set; }

        [Display(Name = "Prekogranični projekt")]
        public bool? PrekogranicniProjekt { get; set; }
        //Dodana polja - kraj

        [Display(Name = "Lokacija provedbe projekta")]
        [Required(ErrorMessage = "Lokacija provedbe projekta je obavezna.")]
        [StringLength(250)]
        public string Lokacija { get; set; }

        public int? OrganizacijaId { get; set; }
        [ForeignKey("OrganizacijaId")]
        public virtual Organizacije Organizacija { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status je obavezan.")]
        [StringLength(25)]
        public string StatusProjekta { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Planirani početak provedbe")]
        public DateTime? Pocetak { get; set; }

        [Display(Name = "Vrsta razvojnog projekta")]
        [StringLength(50)]
        public string VrstaProjekta { get; set; }

        [Display(Name = "Tip razvojnog projekta")]
        [StringLength(50)]
        public string TipProjekta { get; set; }

        [Display(Name = "Proglašen strateškim projektom RH")]
        public bool? ProglasenStrateskim { get; set; }

        //Dodana polja - početak
        [Display(Name = "Prijavljen kao strateški projekt RH")]
        public bool? PrijavljenStrateski { get; set; }

        [Display(Name = "Uvršten na indikativnu listu NSR 2030")]
        public bool? UvrstenNSR { get; set; }

        [Display(Name = "Odobren u okviru NSR 2030")]
        public bool? OdobrenNSR { get; set; }

        [Display(Name = "Odobrenja JPP-a")]
        public DateTime? OdobrenjaJPP { get; set; }

        [Display(Name = "Datum ugovora JPP-a")]
        public DateTime? DatumUgovoraJPP { get; set; }

        [Display(Name = "Prijavljen na Epp-u")]
        public bool? PrijavljenEPP { get; set; }

        [Display(Name = "Objavljen na EPP-u")]
        public bool? ObjavljenEPP { get; set; }

        [Display(Name = "Prijavljen na EIB-u")]
        public bool? PrijavljenEIB { get; set; }

        [Display(Name = "Predodobren od EIB-a")]
        public bool? PredodobrenEIB { get; set; }

        [Display(Name = "Datum prijave velikog projekta")]
        public DateTime? DatumPrijaveVelikog { get; set; }

        [Display(Name = "Odobrenje velikog projekta")]
        public DateTime? OdobrenjeVelikog { get; set; }

        [Display(Name = "Potvrda JASPERS-a")]
        public bool? PotvrdaJASPERS { get; set; }
        //Dodana polja - kraj

        [Required(ErrorMessage = "Modul je obavezan.")]
        [StringLength(25)]
        public string Modul { get; set; }

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
        [Display(Name = "Područje")]
        public string UpravnoPodrucje { get; set; }

        [StringLength(150)]
        public string Sektor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Dokumentacija> Dokumentacija { get; set; }

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

        [DataType(DataType.Date)]
        [Display(Name = "Planirani završetak provedbe")]
        public DateTime? Kraj { get; set; }

        [StringLength(250)]
        [Display(Name = "Izvor financiranja projekta")]
        public string IzvorFinanciranja { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage ="Procijenjena vrijednost mora biti veća od nule.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Procijenjena vrijednost projekta je obavezna.")]
        [Display(Name = "Procijenjena vrijednost projekta (HRK)")]
        public decimal ProcijenjenaVrijednostHRK { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Procijenjeni troškovi moraju biti veći od nule.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Procijenjeni troškovi pripreme su obavezni.")]
        [Display(Name = "Procijenjeni troškovi pripreme (HRK)")]
        public decimal ProcijenjeniTroskoviPripremeHRK { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Procijenjeni troškovi moraju biti veći od nule.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Procijenjeni troškovi provedbe su obavezni.")]
        [Display(Name = "Procijenjeni troškovi provedbe (HRK)")]
        public decimal ProcijenjeniTroskoviProvedbeHRK { get; set; }

        //Dodana polja - početak
        
        [StringLength(500)]
        [Display(Name = "Dodatne napomene")]
        public string DodatneNapomene { get; set; }

        [StringLength(500)]
        [Display(Name = "Web stranica")]
        public string WebStranica { get; set; }

        //USKLAĐENOST

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Uskladjenosti> Uskladjenosti { get; set; }
        //Dodana polja - kraj

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

        public int? KontaktId { get; set; }
        [ForeignKey("KontaktId")]
        public virtual Kontakti Kontakt { get; set; }

        public int? OdgovornaOsobaId { get; set; }
        [ForeignKey("KontaktId")]
        public virtual Kontakti OdgovornaOsoba { get; set; }

        public bool? Upisano { get; set; }

        public bool? Ispravno { get; set; }

        [Display(Name = "Datum i vrijeme predaje")]
        public DateTime? DatumPredaje { get; set; }

        [Display(Name = "Korisnik")]
        public string UserId { get; set; }
    }
}
