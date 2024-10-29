using RAD_biblioteka.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace RAD_biblioteka.Models
{
    public class seedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RAD_bibliotekaContext(
                serviceProvider.GetRequiredService<
                DbContextOptions<RAD_bibliotekaContext>>()))
            {
                if (context.Book.Any())
                {
                    return;
                }
                context.Book.AddRange(
                    new Book
                    {
                        Title = "The Colour of Magic",
                        Author = "Terry Prachet",
                        Publisher = "Corgi",
                        PublicationDate = DateTime.Parse("1998-01-01"),
                        Price = 8,
                        Status = "Available",

                    },
                    new Book
                    {
                        Title = "The Hitchiker's Guide to the Galaxy",
                        Author = "Diuglas Adams",
                        Publisher = "Megadodo Publications",
                        PublicationDate = DateTime.Parse("1979-01-01"),
                        Price = 42,
                        Status = "Available",
                       
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
