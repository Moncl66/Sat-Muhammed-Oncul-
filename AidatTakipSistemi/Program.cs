// Program.cs - GÜNCELLENMÝÞ
using Microsoft.EntityFrameworkCore;
using AidatTakipSistemi.Data;
using AidatTakipSistemi.Models;
using Microsoft.AspNetCore.Identity; // BU YENÝ

var builder = WebApplication.CreateBuilder(args);

// ---- Veritabaný ve DbContext ----
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// ---- YENÝ ADIM: IDENTITY SÝSTEMÝNÝ EKLEME (Düzeltilmiþ) ----

// "AddDefaultIdentity" yerine "AddIdentity" kullanarak Rolleri (Roles) de ekliyoruz.
// Bu, DbInitializer'daki RoleManager'ýn çalýþmasýný saðlayacak.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // Rol sistemi için bu gerekli

// ---- YENÝ ADIM: ÖNEMLÝ DÜZELTME (404 Hatasý Çözümü) ----
// Identity sistemine, bizim manuel "Login" sayfamýzýn nerede olduðunu söylüyoruz.
builder.Services.ConfigureApplicationCookie(options =>
{
    // [Authorize] etiketi tetiklendiðinde, kullanýcýyý buraya yönlendir:
    options.LoginPath = "/Account/Login"; // /Identity/Account/Login DEÐÝL

    // "Yönetici" olmayan biri "Daireler"e girmeye çalýþtýðýnda buraya yönlendir:
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// ---- MVC Servisi ----
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ---- YENÝ ADIM: GÜVENLÝK KONTROLÜNÜ AKTÝF ETME ----
// app.UseAuthentication(); // Önce kimlik doðrula (Kimsin?)
app.UseAuthorization(); // Sonra yetkilendir (Neye iznin var?)
// ÖNEMLÝ: app.UseRouting() ile app.MapControllerRoute() arasýnda olmalý.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// ---- YENÝ ADIM: ROLLERÝ VE ADMÝN'Ý OLUÞTURMA (SEED) ----

// Proje baþlarken, DbContext ve Identity servislerini "talep et"
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // DbInitializer sýnýfýmýzdaki o "tohumlama" metodunu çalýþtýr.
        await DbInitializer.InitializeAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        // Hata olursa logla (þimdilik konsola yaz)
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanýný tohumlarken bir hata oluþtu.");
    }
}
app.Run();