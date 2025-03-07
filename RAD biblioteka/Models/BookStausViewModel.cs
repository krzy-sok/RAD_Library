using Microsoft.AspNetCore.Mvc.Rendering;

namespace RAD_biblioteka.Models
{
    public class BookStausViewModel
    {
        public List<Book>? Books { get; set; }
        public SelectList? Statuses { get; set; }
        public string? BookStatus { get; set; }
        public string? SearchString { get; set; }      
    }
}
