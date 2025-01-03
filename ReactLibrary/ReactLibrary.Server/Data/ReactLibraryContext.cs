using Microsoft.EntityFrameworkCore;


namespace ReactLibrary.Server.Data
{
    public class ReactLibraryContext : DbContext
    {
        public ReactLibraryContext(DbContextOptions<ReactLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<ReactLibrary.Server.Models.Book> Book { get; set; } = default!;

        public DbSet<ReactLibrary.Server.Models.User>? User { get; set; }

        public DbSet<ReactLibrary.Server.Models.Leases>? Leases { get; set; }

        public DbSet<ReactLibrary.Server.Models.RegistrationViewModel>? RegistrationViewModel { get; set; }
    }
}
