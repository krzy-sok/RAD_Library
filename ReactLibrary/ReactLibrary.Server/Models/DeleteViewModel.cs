using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReactLibrary.Server.Models
{
    public class DeleteViewModel
    {
        [Required(ErrorMessage = "Please enter email")]
        [DisplayName("Email")]
        public string userEmail { get; set; }


        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
