using System.ComponentModel.DataAnnotations;

namespace MyNotes.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="First Name")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Password")]

        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
