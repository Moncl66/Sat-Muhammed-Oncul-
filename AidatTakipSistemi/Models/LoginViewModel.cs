// Models/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace AidatTakipSistemi.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}