using System.ComponentModel.DataAnnotations;

namespace CookiesAuthentication.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name ="What is your name?")]
        public string Name { get; set; }
    }
}
