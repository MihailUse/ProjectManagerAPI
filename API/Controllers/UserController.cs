using API.Models.User;
using API.Services;
using AutoMapper;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<UserModel> GetUsers()
        {
            IEnumerable<User> users = _userService.GetUsers();
            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<UserModel> GetUserById(Guid id)
        {
            User user = await _userService.GetUserByIdAsync(id);
            return _mapper.Map<UserModel>(user);
        }

        [HttpGet("{login:alpha}")]
        public async Task<UserModel> GetUserByLogin(string login)
        {
            User user = await _userService.GetUserByLoginAsync(login);
            return _mapper.Map<UserModel>(user);
        }

        [HttpPost]
        public Task<Guid> CreateUser([FromBody] CreateUserModel newUser)
        {
            User user = _mapper.Map<User>(newUser);
            return _userService.CreateUserAsync(user);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserModel newUser)
        {
            User user = _mapper.Map<User>(newUser);
            await _userService.UpdateUserAsync(id, user);
            return Ok("ok");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok("ok");
        }
    }
}
