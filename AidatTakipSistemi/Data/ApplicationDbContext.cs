// Data/ApplicationDbContext.cs - GÜNCELLENMİŞ
using AidatTakipSistemi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AidatTakipSistemi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Daire> Daireler { get; set; }
        public DbSet<Aidat> Aidatlar { get; set; }
        public DbSet<Odeme> Odemeler { get; set; }

        // YENİ EKLENDİ: Bire-Bir İlişkiyi Tanımlayan Metot
        // Bu metot, veritabanı tabloları oluşturulurken EF Core'a talimat verir.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ÖNEMLİ: Identity tablolarının (Users, Roles) düzgün çalışması için
            // bu satır HER ZAMAN en üstte olmalıdır!
            base.OnModelCreating(modelBuilder);

            // Şimdi kendi ilişkimizi tanımlayalım:
            modelBuilder.Entity<ApplicationUser>()      // Bir "ApplicationUser"
                .HasOne(a => a.Daire)                   // bir "Daire"ye sahiptir
                .WithOne(d => d.ApplicationUser)        // O "Daire" de bir "ApplicationUser"a sahiptir
                .HasForeignKey<Daire>(d => d.ApplicationUserId); // Ve "Daire" tablosundaki Foreign Key "ApplicationUserId"dir.
        }
    }
}