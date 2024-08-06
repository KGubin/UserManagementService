using Microsoft.EntityFrameworkCore;
using UserManagementService.Models;
using UserManagementService.Repositories;
using Xunit;

namespace UserManagementService.Tests
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _repository;
        private readonly ApplicationDbContext _context;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            var user = new User { Id = Guid.NewGuid(), Login = "testuser", FirstName = "Test", LastName = "User" };
            await _repository.AddAsync(user);

            var users = await _repository.GetAllAsync();
            Assert.Single(users);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Login = "testuser", FirstName = "Test", LastName = "User" };
            await _repository.AddAsync(user);

            var retrievedUser = await _repository.GetByIdAsync(userId);
            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Login, retrievedUser.Login);
        }
    }
}
