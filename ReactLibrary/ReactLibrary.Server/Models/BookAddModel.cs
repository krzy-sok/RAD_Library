using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReactLibrary.Server.Models
{
    public class BookAddModel
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string? Publisher { get; set; }


        [Display(Name = "Publication Date")]
        [DataType(DataType.Date)]
        public DateTime? PublicationDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
    }
}
