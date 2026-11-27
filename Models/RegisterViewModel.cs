using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email міндетті.")]
    [EmailAddress(ErrorMessage = "Email форматы дұрыс емес.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Аты міндетті.")]
    [Display(Name = "Аты")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Тегі міндетті.")]
    [Display(Name = "Тегі")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпия сөз міндетті.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Құпия сөз кемінде 8 таңба болуы керек.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Құпия сөзде кемінде бір бас әріп және бір цифр болуы керек.")]
    [Display(Name = "Құпия сөз")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Құпия сөзді қайталаңыз.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Құпия сөздер сәйкес емес.")]
    [Display(Name = "Құпия сөзді растау")]
    public string ConfirmPassword { set; get; } = string.Empty;
}
