using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAD_biblioteka.Models
{
    public class Leases
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime leaseStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime leaseEnd { get; set; }

        // add book and user fk
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public string Type { get; set;  }

    }
}
