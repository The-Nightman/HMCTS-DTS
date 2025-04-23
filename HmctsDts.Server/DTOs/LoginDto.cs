using System.ComponentModel.DataAnnotations;

namespace HmctsDts.Server.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public required string Email { get; set; }

    [Required(AllowEmptyStrings = false,
        ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage =
            "Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public required string Password { get; set; }
}