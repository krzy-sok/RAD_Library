using System.ComponentModel.DataAnnotations;

namespace RAD_biblioteka.Models
{
    public class Book
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public string? Publisher { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        public int? Price { get; set; }

        public string? Status { get; set; }

    }
}
