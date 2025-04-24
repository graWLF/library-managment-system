using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }

        public DbSet<Borrowing> borrowings { get; set; }
        public DbSet<Branch> branches { get; set; }
        public DbSet<Librarian> librarians { get; set; }
        public DbSet<Supervisor> supervisors { get; set; }
        public DbSet<Registration> registrations { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            // Explicitly map entities to tables
            builder.Entity<Borrowing>().ToTable("borrowing");
            builder.Entity<Borrower>().ToTable("borrower");
            builder.Entity<Branch>().ToTable("branch");
            builder.Entity<Book>().ToTable("book_info");
            builder.Entity<Author>().ToTable("author_table");
            builder.Entity<Publisher>().ToTable("publisher_table");
            builder.Entity<Librarian>().ToTable("librarian");
            builder.Entity<Supervisor>().ToTable("supervisor");
            builder.Entity<Registration>().ToTable("registration");

            // Configure primary keys and identity columns if needed
            builder.Entity<Borrowing>().Property(b => b.Id).HasColumnName("itemno").ValueGeneratedOnAdd();
            builder.Entity<Borrower>().Property(b => b.Id).HasColumnName("borrowerid").ValueGeneratedOnAdd();
            builder.Entity<Branch>().Property(b => b.Id).HasColumnName("branchid").ValueGeneratedOnAdd();
            builder.Entity<Book>().Property(b => b.ID).HasColumnName("isbn").ValueGeneratedOnAdd();
            builder.Entity<Author>().Property(a => a.Id).HasColumnName("authorid").ValueGeneratedOnAdd();
            builder.Entity<Publisher>().Property(p => p.Id).HasColumnName("publisherid").ValueGeneratedOnAdd();
            builder.Entity<Librarian>().Property(l => l.Id).HasColumnName("librarianid").ValueGeneratedOnAdd();
            builder.Entity<Supervisor>().Property(s => s.Id).HasColumnName("supervisorid").ValueGeneratedOnAdd();
            builder.Entity<Registration>().Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();


            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>  
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }
            base.OnModelCreating(builder);
        }
    }
}
