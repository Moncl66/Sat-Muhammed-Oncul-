// Controllers/AccountController.cs
using AidatTakipSistemi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AidatTakipSistemi.Controllers
{
    public class AccountController : Controller
    {
        // Identity sistemini yöneten iki ana hizmet (service)
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Bu hizmetleri "Dependency Injection" ile kontrolcüye alıyoruz
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // --- KAYIT OL (REGISTER) ---

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Register.cshtml sayfasını göster
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Formdan gelen verilerle yeni bir ApplicationUser oluştur
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                // Identity kütüphanesini kullanarak kullanıcıyı veritabanına kaydet (şifreyi hash'leyerek)
                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    // Kayıt başarılıysa, kullanıcıyı hemen "Login" (Giriş) yap
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // Controllers/AccountController.cs içindeki Register (POST) metodu:

                    if (result.Succeeded)
                    {
                        // YENİ EKLENDİ: Yeni kullanıcıyı "Daire Sakini" rolüne ata.
                        // (DbInitializer'da "Yönetici"yi atadığımız gibi)
                        await _userManager.AddToRoleAsync(user, "Daire Sakini");

                        // Kayıt başarılıysa, kullanıcıyı hemen "Login" (Giriş) yap
                        await _signInManager.SignInAsync(user, isPersistent: false); // Bu satır zaten vardı

                        // Ana sayfaya yönlendir
                        return RedirectToAction("Index", "Home"); // Bu satır zaten vardı
                    }

                    // Ana sayfaya yönlendir
                    return RedirectToAction("Index", "Home");
                }

                // Eğer kayıt başarısız olduysa (örn: e-posta zaten alınmışsa veya şifre zayıfsa)
                // Hataları forma geri gönder
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // Model geçerli değilse veya kayıt başarısızsa, formu tekrar göster
            return View(model);
        }

        // --- GİRİŞ YAP (LOGIN) ---

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Login.cshtml sayfasını göster
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Identity kütüphanesini kullanarak giriş yapmayı dene
                var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, false, lockoutOnFailure: false);


                if (result.Succeeded)
                {
                    // Giriş başarılıysa, ana sayfaya yönlendir
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Giriş başarısızsa (kullanıcı adı veya şifre yanlışsa)
                    ModelState.AddModelError(string.Empty, "Giriş denemesi başarısız. Lütfen bilgilerinizi kontrol edin.");
                    return View(model);
                }
            }
            // Model geçerli değilse, formu tekrar göster
            return View(model);
        }

        // --- ÇIKIŞ YAP (LOGOUT) ---

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Identity kütüphanesini kullanarak oturumu kapat
            await _signInManager.SignOutAsync();

            // Ana sayfaya yönlendir
            return RedirectToAction("Index", "Home");

        }
        // --- ERİŞİM ENGELLENDİ (ACCESS DENIED) ---
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View(); // AccessDenied.cshtml sayfasını göster
        }
    }
}