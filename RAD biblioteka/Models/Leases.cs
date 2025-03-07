using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAD_biblioteka.Models
{
    public class Leases
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime? leaseStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime? leaseEnd { get; set; }

        // add book and user fk
        public Book book { get; set; }

        public User user { get; set; }

        public string Type { get; set;  }

        public bool Active { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
