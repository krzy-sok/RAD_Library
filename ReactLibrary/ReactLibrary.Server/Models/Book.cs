using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using ReactLibrary.Server.Data;

namespace ReactLibrary.Server.Models
{
    public class Book
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public string? Publisher { get; set; }


        [Display(Name = "Publication Date")]
        [DataType(DataType.Date)]
        public DateTime? PublicationDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }

        public string Status { get; set; }

        public bool Hidden { get; set; }


    }
}


//public static class BookEndpoints
//{
//	public static void MapBookEndpoints (this IEndpointRouteBuilder routes)
//    {
//        var group = routes.MapGroup("/api/Book").WithTags(nameof(Book));

//        group.MapGet("/", async (ReactLibraryContext db) =>
//        {
//            return await db.Book.ToListAsync();
//        })
//        .WithName("GetAllBooks")
//        .WithOpenApi();

//        group.MapGet("/{id}", async Task<Results<Ok<Book>, NotFound>> (int id, ReactLibraryContext db) =>
//        {
//            return await db.Book.AsNoTracking()
//                .FirstOrDefaultAsync(model => model.Id == id)
//                is Book model
//                    ? TypedResults.Ok(model)
//                    : TypedResults.NotFound();
//        })
//        .WithName("GetBookById")
//        .WithOpenApi();

//        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Book book, ReactLibraryContext db) =>
//        {
//            var affected = await db.Book
//                .Where(model => model.Id == id)
//                .ExecuteUpdateAsync(setters => setters
//                  .SetProperty(m => m.Id, book.Id)
//                  .SetProperty(m => m.Title, book.Title)
//                  .SetProperty(m => m.Author, book.Author)
//                  .SetProperty(m => m.Publisher, book.Publisher)
//                  .SetProperty(m => m.PublicationDate, book.PublicationDate)
//                  .SetProperty(m => m.Price, book.Price)
//                  .SetProperty(m => m.Status, book.Status)
//                  .SetProperty(m => m.Hidden, book.Hidden)
//                  );
//            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
//        })
//        .WithName("UpdateBook")
//        .WithOpenApi();

//        group.MapPost("/", async (Book book, ReactLibraryContext db) =>
//        {
//            db.Book.Add(book);
//            await db.SaveChangesAsync();
//            return TypedResults.Created($"/api/Book/{book.Id}",book);
//        })
//        .WithName("CreateBook")
//        .WithOpenApi();

//        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ReactLibraryContext db) =>
//        {
//            var affected = await db.Book
//                .Where(model => model.Id == id)
//                .ExecuteDeleteAsync();
//            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
//        })
//        .WithName("DeleteBook")
//        .WithOpenApi();
//    }
//}}
