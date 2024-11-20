using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAD_biblioteka.Models
{
    public class Book
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public string? Publisher { get; set; }


        [Display(Name = "Publication Date")]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Column(TypeName ="decimal(18, 2)")]
        public decimal? Price { get; set; }

        public string Status { get; set; }

        public bool Hidden { get; set; }

    }
}
