// Controllers/AidatlarController.cs
using AidatTakipSistemi.Data;
using AidatTakipSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList (açılır liste) için bu gerekli
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AidatTakipSistemi.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class AidatlarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AidatlarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Aidatlar
        // DİKKAT: Burada .Include(a => a.Daire) komutunu ekledik.
        // Bu, aidat listesini çekerken, her aidatın DaireId'sine karşılık gelen
        // Daire'nin bilgilerini de (Blok, No vs.) getirmesini sağlar.
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Aidatlar.Include(a => a.Daire);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Aidatlar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aidat = await _context.Aidatlar
                .Include(a => a.Daire) // Detayları gösterirken de Daire bilgisini çek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aidat == null)
            {
                return NotFound();
            }

            return View(aidat);
        }

        // GET: Aidatlar/Create
        // DİKKAT: Yeni aidat oluşturma formunu GÖSTERİRKEN
        // "Hangi daire için?" diye sormak için Daire listesini forma göndermeliyiz.
        public IActionResult Create()
        {
            // Daireleri veritabanından çek ve "Id" (değer) ve "Blok - DaireNo" (görünen metin)
            // olarak bir liste oluştur.
            var dairelerListesi = _context.Daireler
                .Select(d => new {
                    Id = d.Id,
                    Display = d.Blok + " Blok - No: " + d.DaireNo
                }).ToList();

            // Bu listeyi View'e (HTML sayfasına) "ViewData["DaireId"]" adıyla gönder
            ViewData["DaireId"] = new SelectList(dairelerListesi, "Id", "Display");
            return View();
        }

        // POST: Aidatlar/Create
        // (Formdan gelen Aidat verisini veritabanına kaydet)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tutar,Donem,SonOdemeTarihi,OdendiMi,DaireId")] Aidat aidat)
        {
            // ModelState.IsValid kontrolü, modeldeki kurallara (örn: Tutar boş olamaz)
            // uyulup uyulmadığını kontrol eder. Biz modelde kural belirtmediğimiz için
            // şimdilik DaireId kontrolünü manuel yapalım:
            if (aidat.DaireId != 0 && aidat.Tutar > 0)
            {
                _context.Add(aidat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Eğer kayıt başarısız olursa (örn: Daire seçilmediyse),
            // formu tekrar göster. ANCAK: Açılır liste (dropdown) boşalır.
            // Bu yüzden listeyi TEKRAR doldurup View'e göndermeliyiz.
            var dairelerListesi = _context.Daireler
                .Select(d => new {
                    Id = d.Id,
                    Display = d.Blok + " Blok - No: " + d.DaireNo
                }).ToList();
            ViewData["DaireId"] = new SelectList(dairelerListesi, "Id", "Display", aidat.DaireId);

            return View(aidat);
        }

        // GET: Aidatlar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aidat = await _context.Aidatlar.FindAsync(id);
            if (aidat == null)
            {
                return NotFound();
            }

            // Düzenleme formunda da Daire listesi açılır listesi gerekir
            var dairelerListesi = _context.Daireler
                .Select(d => new {
                    Id = d.Id,
                    Display = d.Blok + " Blok - No: " + d.DaireNo
                }).ToList();
            ViewData["DaireId"] = new SelectList(dairelerListesi, "Id", "Display", aidat.DaireId);
            return View(aidat);
        }

        // POST: Aidatlar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tutar,Donem,SonOdemeTarihi,OdendiMi,DaireId")] Aidat aidat)
        {
            if (id != aidat.Id)
            {
                return NotFound();
            }

            if (aidat.DaireId != 0 && aidat.Tutar > 0)
            {
                try
                {
                    _context.Update(aidat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Aidatlar.Any(e => e.Id == aidat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Hata olursa formu tekrar yükle (açılır liste ile beraber)
            var dairelerListesi = _context.Daireler
                .Select(d => new {
                    Id = d.Id,
                    Display = d.Blok + " Blok - No: " + d.DaireNo
                }).ToList();
            ViewData["DaireId"] = new SelectList(dairelerListesi, "Id", "Display", aidat.DaireId);
            return View(aidat);
        }

        // GET: Aidatlar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aidat = await _context.Aidatlar
                .Include(a => a.Daire) // Silme onayı alırken Daire bilgisini de göster
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aidat == null)
            {
                return NotFound();
            }

            return View(aidat);
        }

        // POST: Aidatlar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aidat = await _context.Aidatlar.FindAsync(id);
            if (aidat != null)
            {
                _context.Aidatlar.Remove(aidat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}