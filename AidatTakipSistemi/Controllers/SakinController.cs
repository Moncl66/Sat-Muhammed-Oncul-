// Controllers/SakinController.cs
using AidatTakipSistemi.Data;
using AidatTakipSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AidatTakipSistemi.Controllers
{
    // DİKKAT: Bu kontrolcüyü SADECE "Daire Sakini" rolündeki kişiler açabilir
    [Authorize(Roles = "Daire Sakini")]
    public class SakinController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SakinController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Sakin/Index (yani "Borçlarım" sayfası)
        public async Task<IActionResult> Index()
        {
            // 1. Giriş yapan kullanıcının kim olduğunu bul
            var kullaniciId = _userManager.GetUserId(User);

            // 2. Bu kullanıcının atandığı daireyi veritabanından bul
            //    (Include ile Daire'nin bilgilerini de çekiyoruz)
            var daire = await _context.Daireler
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.ApplicationUserId == kullaniciId);

            // 3. Eğer kullanıcı hiçbir daireye atanmamışsa (Yönetici atamamışsa)
            if (daire == null)
            {
                // Ona "Yönetici ile iletişime geçin" diyen bir hata sayfası göster
                return View("DaireAtanmamis");
            }

            // 4. Eğer daire bulunduysa, SADECE o daireye ait aidatları bul
            var aidatlar = await _context.Aidatlar
                .Where(a => a.DaireId == daire.Id)
                .OrderByDescending(a => a.Donem) // En yeni borç en üstte olsun
                .ToListAsync();

            // 5. Bu aidat listesini "Borçlarım" sayfasına (Index.cshtml) gönder
            return View(aidatlar);
        }
    }
}