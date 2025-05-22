using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Core.Entities;



    public class BorrowingRepositoryTests
    {
        private ApplicationDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new ApplicationDbContext(options, null, null);
        }

        private Borrowing CreateSampleBorrowing(long id = 1, long borrowerId = 2, string borrowDate = "2024-01-01", string dueDate = "2024-01-10")
        {
            return new Borrowing
            {
                Id = id,
                borrowerid = borrowerId,
                branchid = 1,
                borrowdate = borrowDate,
                duedate = dueDate,
                returnstatus = false
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBorrowings()
        {
            // Arrange
            var dbName = nameof(GetAllAsync_ReturnsAllBorrowings);
            using var context = GetDbContext(dbName);
            context.borrowings.Add(CreateSampleBorrowing(1));
            context.borrowings.Add(CreateSampleBorrowing(2));
            await context.SaveChangesAsync();

            var repo = new BorrowingRepository(context);

            // Act
            var result = await repo.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByCompositeKeyAsync_ReturnsCorrectBorrowing()
        {
            // Arrange
            var dbName = nameof(GetByCompositeKeyAsync_ReturnsCorrectBorrowing);
            using var context = GetDbContext(dbName);
            var borrowing = CreateSampleBorrowing(1, 2, "2024-01-01", "2024-01-10");
            context.borrowings.Add(borrowing);
            await context.SaveChangesAsync();

            var repo = new BorrowingRepository(context);

            // Act
            var result = await repo.GetByCompositeKeyAsync(1, 2, "2024-01-01", "2024-01-10");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.borrowerid);
        }

        [Fact]
        public async Task AddAsync_AddsBorrowingToDatabase()
        {
            // Arrange
            var dbName = nameof(AddAsync_AddsBorrowingToDatabase);
            using var context = GetDbContext(dbName);
            var repo = new BorrowingRepository(context);
            var borrowing = CreateSampleBorrowing();

            // Act
            await repo.AddAsync(borrowing);

            // Assert
            Assert.Single(context.borrowings);
            Assert.Equal(borrowing.Id, context.borrowings.First().Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesBorrowingInDatabase()
        {
            // Arrange
            var dbName = nameof(UpdateAsync_UpdatesBorrowingInDatabase);
            using var context = GetDbContext(dbName);
            var borrowing = CreateSampleBorrowing();
            context.borrowings.Add(borrowing);
            await context.SaveChangesAsync();

            var repo = new BorrowingRepository(context);

            // Act
            borrowing.returnstatus = true;
            await repo.UpdateAsync(borrowing);

            // Assert
            var updated = context.borrowings.First();
            Assert.True(updated.returnstatus);
        }

        [Fact]
        public async Task DeleteAsync_RemovesBorrowingFromDatabase()
        {
            // Arrange
            var dbName = nameof(DeleteAsync_RemovesBorrowingFromDatabase);
            using var context = GetDbContext(dbName);
            var borrowing = CreateSampleBorrowing();
            context.borrowings.Add(borrowing);
            await context.SaveChangesAsync();

            var repo = new BorrowingRepository(context);

            // Act
            await repo.DeleteAsync(borrowing);

            // Assert
            Assert.Empty(context.borrowings);
        }
    }
