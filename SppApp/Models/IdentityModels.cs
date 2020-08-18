using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SppApp.Models;

namespace  SppApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Aktivnosti> Aktivnosti { get; set; }
        public virtual DbSet<Dionici> Dionici { get; set; }
        public virtual DbSet<Financiranja> Financiranja { get; set; }
        public virtual DbSet<GradjevinskeDozvole> GradjevinskeDozvole { get; set; }
        public virtual DbSet<Kontakti> Kontakti { get; set; }
        public virtual DbSet<Organizacije> Organizacije { get; set; }
        public virtual DbSet<OstalaDokumentacija> OstalaDokumentacija { get; set; }
        public virtual DbSet<Pokazatelji> Pokazatelji { get; set; }
        public virtual DbSet<Projekti> Projekti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Aktivnosti>()
                .Property(e => e.BrojJedinica)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Aktivnosti>()
                .Property(e => e.JedinicnaCijena)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Financiranja>()
                .Property(e => e.IznosHRK)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Financiranja>()
                .Property(e => e.IznosEUR)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Kontakti>()
                .HasMany(e => e.Projekti)
                .WithOptional(e => e.Kontakt)
                .HasForeignKey(e => e.KontaktId);

            modelBuilder.Entity<Organizacije>()
                .HasMany(e => e.Kontakt)
                .WithOptional(e => e.Organizacija)
                .HasForeignKey(e => e.OrganizacijaID);

            modelBuilder.Entity<Organizacije>()
                .HasMany(e => e.Projekt)
                .WithOptional(e => e.Organizacija)
                .HasForeignKey(e => e.OrganizacijaId);

            modelBuilder.Entity<Pokazatelji>()
                .Property(e => e.BrojJedinica)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Projekti>()
                .Property(e => e.ProcijenjenaVrijednost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Projekti>()
                .Property(e => e.ProcijenjeniTroskoviPripreme)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Projekti>()
                .Property(e => e.ProcijenjeniTroskoviProvedbe)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Projekti>()
                .HasMany(e => e.Aktivnosti)
                .WithOptional(e => e.Projekt)
                .HasForeignKey(e => e.ProjektId);

            modelBuilder.Entity<Projekti>()
                .HasMany(e => e.Dionici)
                .WithOptional(e => e.Projekt)
                .HasForeignKey(e => e.ProjektId);

            modelBuilder.Entity<Projekti>()
                .HasMany(e => e.Financiranja)
                .WithOptional(e => e.Projekt)
                .HasForeignKey(e => e.ProjektId);

            modelBuilder.Entity<Projekti>()
                .HasMany(e => e.GradjevinskeDozvole)
                .WithOptional(e => e.Projekt)
                .HasForeignKey(e => e.ProjektId);

            modelBuilder.Entity<Projekti>()
                .HasMany(e => e.OstalaDokumentacija)
                .WithOptional(e => e.Projekt)
                .HasForeignKey(e => e.ProjektId);

            modelBuilder.Entity<Projekti>()
                .HasMany(e => e.Pokazatelji)
                .WithOptional(e => e.Projekt)
                .HasForeignKey(e => e.ProjektId);
        }
    }
}