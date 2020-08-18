using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace  SppApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "E-mail adresa je obavezna")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Display(Name = "Davatelj")]
        [Required(ErrorMessage = "Davatelj je obavezan")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "Kod je obavezan")]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Zapamti ovaj preglednik?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "E-mail adresa je obavezna")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail adresa je obavezna")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "E-mail adresa nije u ispravnom formatu")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [Display(Name = "Zapamti me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "E-mail adresa je obavezna")]
        [EmailAddress(ErrorMessage = "E-mail adresa nije u ispravnom formatu")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [StringLength(100, ErrorMessage = "{0} mora biti duga najmanje {2} znakova.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrda lozinke")]
        [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "E-mail adresa je obavezna")]
        [EmailAddress(ErrorMessage = "E-mail adresa nije u ispravnom formatu")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [StringLength(100, ErrorMessage = "{0} mora biti duga najmanje {2} znakova.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrda lozinke")]
        [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-mail adresa je obavezna")]
        [EmailAddress(ErrorMessage = "E-mail adresa nije u ispravnom formatu")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
