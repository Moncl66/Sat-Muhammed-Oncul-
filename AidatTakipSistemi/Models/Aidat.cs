namespace AidatTakipSistemi.Models
{
    public class Aidat
    {
        public int Id { get; set; }
        public decimal Tutar { get; set; }
        public DateTime Donem { get; set; } // Örn: 1 Ekim 2024 (Ekim ayı aidatı)
        public DateTime SonOdemeTarihi { get; set; }
        public bool OdendiMi { get; set; } = false; // İlk oluşturulduğunda 'ödenmedi' olsun

        // İlişki: Bu aidat hangi daireye ait?
        public int DaireId { get; set; } // Bu bir "Foreign Key" (Yabancı Anahtar)
        public virtual Daire? Daire { get; set; } // DaireId'nin Daire tablosuna bağlı olduğunu söyler
    }
}
