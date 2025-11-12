using System.ComponentModel.DataAnnotations;

namespace WorkNest.Models.User.Dto
{
    public class LoginDTO
    {
        [Required]
        [MinLength(2)]
        public string EmailOrUsername { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
    }
}

