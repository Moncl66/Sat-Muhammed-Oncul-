// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace AidatTakipSistemi.Models
{
    // Standart IdentityUser sınıfına ek olarak 
    // ileride Ad, Soyad gibi alanlar ekleyebilmemiz için
    // kendi özel Kullanıcı sınıfımızı oluşturuyoruz.
    public class ApplicationUser : IdentityUser
    {
        // Örnek:
        // public string? Ad { get; set; }
        // public string? Soyad { get; set; }
        public virtual Daire? Daire { get; set; }
    }
}