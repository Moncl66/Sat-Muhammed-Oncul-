// Controllers/DairelerController.cs - GÜNCELLENMİŞ
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AidatTakipSistemi.Data;
using AidatTakipSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // YENİ EKLENDİ (Kullanıcıları çekmek için)
using Microsoft.AspNetCore.Mvc.Rendering; // YENİ EKLENDİ (SelectList için)

namespace AidatTakipSistemi.Controllers
{
    [Authorize(Roles = "Yönetici")] // Bu zaten vardı
    public class DairelerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // YENİ EKLENDİ

        // YENİ EKLENDİ: Artık DbContext'e ek olarak UserManager'ı da çağırıyoruz
        public DairelerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; // YENİ EKLENDİ
        }

        // GET: /Daireler (Değişiklik yok)
        public async Task<IActionResult> Index()
        {
            return View(await _context.Daireler.ToListAsync());
        }

        // GET: /Daireler/Create (Değişiklik yok)
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Daireler/Create (Değişiklik yok)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Blok,Kat,DaireNo,SahipAdi,SakinAdi")] Daire daire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(daire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(daire);
        }

        // --- DÜZENLEME METOTLARI GÜNCELLENDİ ---

        // GET: Daireler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var daire = await _context.Daireler.FindAsync(id);
            if (daire == null)
            {
                return NotFound();
            }

            // --- YENİ EKLENDİ: KULLANICI LİSTESİNİ ÇEKME ---
            // 1. "Daire Sakini" rolündeki tüm kullanıcıları bul.
            var sakinler = await _userManager.GetUsersInRoleAsync("Daire Sakini");

            // 2. Bu listeyi (Id, Email) SelectList'e dönüştür ve View'e (sayfaya) gönder.
            //    Dairenin mevcut sakini (daire.ApplicationUserId) otomatik olarak seçili gelsin.
            ViewData["ApplicationUserId"] = new SelectList(sakinler, "Id", "Email", daire.ApplicationUserId);
            // --- BİTTİ ---

            return View(daire);
        }

        // POST: Daireler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // YENİ EKLENDİ: [Bind(...)] listesine "ApplicationUserId" eklendi
        public async Task<IActionResult> Edit(int id, [Bind("Id,Blok,Kat,DaireNo,SahipAdi,SakinAdi,ApplicationUserId")] Daire daire)
        {
            if (id != daire.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // "ApplicationUserId" alanı boş bir string gelirse, onu "null" yap
                    // Bu, "Sakin Yok" seçeneğinin düzgün çalışması için gereklidir.
                    if (string.IsNullOrEmpty(daire.ApplicationUserId))
                    {
                        daire.ApplicationUserId = null;
                    }

                    _context.Update(daire); // Veritabanında güncelle (Yeni ApplicationUserId dahil)
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // ... (hata yönetimi)
                }
                return RedirectToAction(nameof(Index));
            }

            // --- YENİ EKLENDİ: HATA DURUMUNDA KULLANICI LİSTESİNİ TEKRAR GÖNDER ---
            var sakinler = await _userManager.GetUsersInRoleAsync("Daire Sakini");
            ViewData["ApplicationUserId"] = new SelectList(sakinler, "Id", "Email", daire.ApplicationUserId);
            // --- BİTTİ ---

            return View(daire);
        }

        // --- SİLME VE DETAY METOTLARI GÜNCELLENDİ ---

        // GET: Daireler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // YENİ EKLENDİ: Daireyi silerken, sakinin kim olduğunu da göster
            var daire = await _context.Daireler
                .Include(d => d.ApplicationUser) // İlişkili kullanıcıyı da çek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (daire == null)
            {
                return NotFound();
            }

            return View(daire);
        }

        // POST: Daireler/Delete/5 (Değişiklik yok)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var daire = await _context.Daireler.FindAsync(id);
            if (daire != null)
            {
                _context.Daireler.Remove(daire);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Daireler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // YENİ EKLENDİ: Daire detayını gösterirken, sakinin kim olduğunu da göster
            var daire = await _context.Daireler
                .Include(d => d.ApplicationUser) // İlişkili kullanıcıyı da çek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (daire == null)
            {
                return NotFound();
            }

            return View(daire);
        }
    }
}