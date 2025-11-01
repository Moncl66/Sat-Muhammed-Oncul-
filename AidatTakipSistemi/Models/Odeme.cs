namespace AidatTakipSistemi.Models
{
    public class Odeme
    {
        public int Id { get; set; }
        public decimal OdenenTutar { get; set; }
        public DateTime OdemeTarihi { get; set; }

        // İlişki: Bu ödeme hangi aidat borcu için yapıldı?
        public int AidatId { get; set; } // Foreign Key
        public virtual Aidat? Aidat { get; set; }
    }
}
