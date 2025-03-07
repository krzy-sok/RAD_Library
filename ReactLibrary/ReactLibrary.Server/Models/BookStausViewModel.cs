using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReactLibrary.Server.Models
{
    public class BookStausViewModel
    {
        public List<Book>? Books { get; set; }
        public SelectList? Statuses { get; set; }
        public string? BookStatus { get; set; }
        public string? SearchString { get; set; }      
    }
}
