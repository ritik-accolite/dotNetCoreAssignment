using System.ComponentModel.DataAnnotations;

namespace MyNotes.ViewModel
{
    public class ChangepassViewModel
    {
        [Required]
        [Display(Name = "Old Password")]
        public string? OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        [Display(Name = "Confirm New Password")]
        public string? ConfirmNewPassword { get; set; }
    }
}
