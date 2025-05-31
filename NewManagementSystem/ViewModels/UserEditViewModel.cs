using System.ComponentModel.DataAnnotations;

namespace NewsManagementSystem.Controllers.ViewModels;

public class UserEditViewModel
{
    public short AccountId { get; set; } // Null for create, set for edit

    [Required]
    [Display(Name = "Name")]
    public string? AccountName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? AccountEmail { get; set; }

    [Required]
    [Display(Name = "Role")]
    public int? AccountRole { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? AccountPassword { get; set; }

    public string? GetRoleName()
    {
        return AccountRole switch
        {
            1 => "Staff",
            2 => "Lecturer",
            _ => null
        };
    }
}