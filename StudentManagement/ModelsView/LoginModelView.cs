using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace StudentManagement.ModelsView
{
    public class LoginModelView
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Please import user")]
        [Display(Name = "User")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "PassWord")]
        [Required(ErrorMessage = "Please import password")]
        [MaxLength(30, ErrorMessage = "password lenght 30 characters")]
        public string Password { get; set; } = string.Empty;

        public bool KeepMeLogin { get; set; }
    }
}
