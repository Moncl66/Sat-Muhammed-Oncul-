// Data/DbInitializer.cs
using AidatTakipSistemi.Models;
using Microsoft.AspNetCore.Identity;

namespace AidatTakipSistemi.Data
{
    // Proje her çalıştığında bu sınıf çağrılacak
    public static class DbInitializer
    {
        // Bu metot, rollerin ve ilk admin kullanıcısının
        // veritabanında var olmasını garanti eder.
        public static async Task InitializeAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // ---- 1. Rolleri Oluştur ----

            // Eğer "Yönetici" rolü yoksa, oluştur.
            if (!await roleManager.RoleExistsAsync("Yönetici"))
            {
                await roleManager.CreateAsync(new IdentityRole("Yönetici"));
            }

            // Eğer "Daire Sakini" rolü yoksa, oluştur.
            if (!await roleManager.RoleExistsAsync("Daire Sakini"))
            {
                await roleManager.CreateAsync(new IdentityRole("Daire Sakini"));
            }

            // ---- 2. İlk Yönetici Kullanıcısını Oluştur ----

            // Bu e-postaya sahip bir kullanıcı yoksa...
            if (await userManager.FindByEmailAsync("admin@admin.com") == null)
            {
                // Yeni bir kullanıcı oluştur
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true // E-posta onayı gerektirmesin
                };

                // Kullanıcıyı veritabanına kaydet (Şifre: Admin123!)
                var result = await userManager.CreateAsync(user, "Admin123!");

                if (result.Succeeded)
                {
                    // Eğer kullanıcı başarıyla oluşturulduysa,
                    // o kullanıcıyı "Yönetici" rolüne ata.
                    await userManager.AddToRoleAsync(user, "Yönetici");
                }
            }
        }
    }
}