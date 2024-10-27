using System.ComponentModel.DataAnnotations;

namespace RAD_biblioteka.Models
{
    public class Leases
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime leaseStart { get; set; }
        public DataType? leaseEnd { get; set; }

        // add book and user fk
    }
}
