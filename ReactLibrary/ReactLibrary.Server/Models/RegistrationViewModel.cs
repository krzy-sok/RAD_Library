using System.ComponentModel.DataAnnotations;

namespace ReactLibrary.Server.Models
{
    public class RegistrationViewModel
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? email { get; set; }
        public string? phoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }
        [Compare("password")]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }
    }
}
