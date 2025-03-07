using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RAD_biblioteka.Models;

namespace RAD_biblioteka.Data
{
    public class RAD_bibliotekaContext : DbContext
    {
        public RAD_bibliotekaContext (DbContextOptions<RAD_bibliotekaContext> options)
            : base(options)
        {
        }

        public DbSet<RAD_biblioteka.Models.Book> Book { get; set; } = default!;

        public DbSet<RAD_biblioteka.Models.User>? User { get; set; }

        public DbSet<RAD_biblioteka.Models.Leases>? Leases { get; set; }

        public DbSet<RAD_biblioteka.Models.RegistrationViewModel>? RegistrationViewModel { get; set; }
    }
}
