namespace AidatTakipSistemi.Models
{
    public class Daire
    {
        public int Id { get; set; } // Veritabanındaki benzersiz kimlik
        public string Blok { get; set; } = string.Empty;
        public int Kat { get; set; }
        public int DaireNo { get; set; }
        public string SahipAdi { get; set; } = string.Empty;
        public string SakinAdi { get; set; } = string.Empty; 
        // Veya kiracı
        // YENİ EKLENDİ: Bu dairenin sakini hangi kullanıcı? (Foreign Key)
        // ApplicationUser ID'si string (metin) olduğu için bu alanı string yapıyoruz.
        public string? ApplicationUserId { get; set; }
        // YENİ EKLENDİ: Kullanıcı modeline "navigation property"
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
