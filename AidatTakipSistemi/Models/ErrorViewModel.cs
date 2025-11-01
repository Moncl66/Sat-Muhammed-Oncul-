// Models/ErrorViewModel.cs
// İSİM ALANI BU OLMALI:
namespace AidatTakipSistemi.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}