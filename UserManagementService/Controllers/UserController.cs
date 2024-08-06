using Microsoft.AspNetCore.Mvc;
using UserManagementService.Models;
using UserManagementService.Repositories;

namespace UserManagementService.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Login))
            {
                return BadRequest("Логин не может быть пустым.");
            }

            var existingUsers = await _userRepository.GetAllAsync();
            if (existingUsers.Any(u => u.Login == user.Login))
            {
                return BadRequest("Логин уже занят.");
            }

            await _userRepository.AddAsync(user);
            return Ok();
        }
    }
}
