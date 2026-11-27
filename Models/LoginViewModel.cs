using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email міндетті.")]
    [EmailAddress(ErrorMessage = "Email форматы дұрыс емес.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпия сөз міндетті.")]
    [DataType(DataType.Password)]
    [Display(Name = "Құпия сөз")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Мені есте сақта")]
    public bool RememberMe { get; set; }
}
