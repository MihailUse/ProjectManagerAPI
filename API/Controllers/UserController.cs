using API.Models.User;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    // TODO: add action 
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
            return _userService.GetUsers().ProjectTo<UserModel>(_mapper.ConfigurationProvider);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<UserModel> GetUserById(Guid id)
        {
            User user = await _userService.GetUserById(id);
            return _mapper.Map<UserModel>(user);
        }

        [HttpGet("{login:alpha}")]
        public async Task<UserModel> GetUserByLogin(string login)
        {
            User user = await _userService.GetUserByLogin(login);
            return _mapper.Map<UserModel>(user);
        }

        [HttpPost]
        public Task<Guid> CreateUser([FromBody] CreateUserModel newUser)
        {
            User user = _mapper.Map<User>(newUser);
            return _userService.CreateUser(user);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserModel userOptions)
        {
            User user = _mapper.Map<User>(userOptions);
            await _userService.UpdateUser(id, user);
            return Ok("ok");
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUser(id);
            return Ok("ok");
        }
    }
}
