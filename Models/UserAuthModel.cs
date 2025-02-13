using System.ComponentModel.DataAnnotations;
namespace TaskManagerAPI.Models
{
    public class UserAuthModel
    {
        [Required(ErrorMessage = "Username обязателен.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password обязателен.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
