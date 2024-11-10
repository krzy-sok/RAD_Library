using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RAD_biblioteka.Models
{
    //[Index(nameof(email), IsUnique = true)]
    //[Index(nameof(userName), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string userName { get; set; }

        public string? firstName { get; set; }

        public string? lastName { get; set; }

        public string? email { get; set; }
        public string? phoneNumber { get; set; }

        public string password { get; set; }

        public bool admin { get; set; }
    }
}
