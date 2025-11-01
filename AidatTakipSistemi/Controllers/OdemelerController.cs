// Controllers/OdemelerController.cs - UYARILAR GİDERİLDİ
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AidatTakipSistemi.Data;
using AidatTakipSistemi.Models;
using Microsoft.AspNetCore.Authorization;

namespace AidatTakipSistemi.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class OdemelerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OdemelerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Odemeler
        public async Task<IActionResult> Index()
        {
            // DÜZELTME: a!.Daire (Derleyiciye "a"nın null olmayacağına güven" diyoruz)
            var odemeler = _context.Odemeler
                .Include(o => o.Aidat)
                    .ThenInclude(a => a!.Daire);
            return View(await odemeler.ToListAsync());
        }

        // GET: Odemeler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // DÜZELTME: a!.Daire
            var odeme = await _context.Odemeler
                .Include(o => o.Aidat)
                .ThenInclude(a => a!.Daire)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (odeme == null)
            {
                return NotFound();
            }

            return View(odeme);
        }

        // GET: Odemeler/Create
        public IActionResult Create()
        {
            var odenmemisAidatlarListesi = _context.Aidatlar
                .Include(a => a.Daire)
                .Where(a => a.OdendiMi == false)
                .ToList();

            var acilirListeIcinVeri = odenmemisAidatlarListesi.Select(a => new {
                Id = a.Id,
                Display = $"{a.Daire?.Blok} Blok No: {a.Daire?.DaireNo} - {a.Donem:MMMM yyyy} ({a.Tutar} TL)"
            }).ToList();

            ViewData["AidatId"] = new SelectList(acilirListeIcinVeri, "Id", "Display");
            return View();
        }

        // POST: Odemeler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OdenenTutar,OdemeTarihi,AidatId")] Odeme odeme)
        {
            if (odeme.AidatId != 0 && odeme.OdenenTutar > 0)
            {
                var ilgiliAidat = await _context.Aidatlar.FindAsync(odeme.AidatId);

                if (ilgiliAidat != null && ilgiliAidat.OdendiMi == false)
                {
                    ilgiliAidat.OdendiMi = true;
                    _context.Update(ilgiliAidat);
                }

                _context.Add(odeme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var odenmemisAidatlarListesi = _context.Aidatlar
                .Include(a => a.Daire)
                .Where(a => a.OdendiMi == false)
                .ToList();
            var acilirListeIcinVeri = odenmemisAidatlarListesi.Select(a => new {
                Id = a.Id,
                Display = $"{a.Daire?.Blok} Blok No: {a.Daire?.DaireNo} - {a.Donem:MMMM yyyy} ({a.Tutar} TL)"
            }).ToList();
            ViewData["AidatId"] = new SelectList(acilirListeIcinVeri, "Id", "Display", odeme.AidatId);
            return View(odeme);
        }

        // GET: Odemeler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odeme = await _context.Odemeler.FindAsync(id);
            if (odeme == null)
            {
                return NotFound();
            }

            var tumAidatlarListesi = _context.Aidatlar
                .Include(a => a.Daire)
                .ToList();
            var acilirListeIcinVeri = tumAidatlarListesi.Select(a => new {
                Id = a.Id,
                Display = $"{a.Daire?.Blok} Blok No: {a.Daire?.DaireNo} - {a.Donem:MMMM yyyy} ({a.Tutar} TL)"
            }).ToList();
            ViewData["AidatId"] = new SelectList(acilirListeIcinVeri, "Id", "Display", odeme.AidatId);
            return View(odeme);
        }

        // POST: Odemeler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OdenenTutar,OdemeTarihi,AidatId")] Odeme odeme)
        {
            if (id != odeme.Id)
            {
                return NotFound();
            }

            if (odeme.AidatId != 0 && odeme.OdenenTutar > 0)
            {
                try
                {
                    _context.Update(odeme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }

            var tumAidatlarListesi = _context.Aidatlar
                .Include(a => a.Daire)
                .ToList();
            var acilirListeIcinVeri = tumAidatlarListesi.Select(a => new {
                Id = a.Id,
                Display = $"{a.Daire?.Blok} Blok No: {a.Daire?.DaireNo} - {a.Donem:MMMM yyyy} ({a.Tutar} TL)"
            }).ToList();
            ViewData["AidatId"] = new SelectList(acilirListeIcinVeri, "Id", "Display", odeme.AidatId);
            return View(odeme);
        }

        // GET: Odemeler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // DÜZELTME: a!.Daire
            var odeme = await _context.Odemeler
                .Include(o => o.Aidat)
                .ThenInclude(a => a!.Daire)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (odeme == null)
            {
                return NotFound();
            }

            return View(odeme);
        }

        // POST: Odemeler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var odeme = await _context.Odemeler.FindAsync(id);
            if (odeme != null)
            {
                var ilgiliAidat = await _context.Aidatlar.FindAsync(odeme.AidatId);

                if (ilgiliAidat != null)
                {
                    ilgiliAidat.OdendiMi = false;
                    _context.Update(ilgiliAidat);
                }

                _context.Odemeler.Remove(odeme);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}