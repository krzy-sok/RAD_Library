using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RAD_biblioteka.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter user name or email")]
        [DisplayName("Username or Email")]
        public string userNameOrEmail { get; set; }


        [Required(ErrorMessage ="Please enter password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
