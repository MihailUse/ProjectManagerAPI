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
        public IAsyncEnumerable<UserModel> Get()
        {
            IAsyncEnumerable<User> users = _userService.GetUsers();
            return _mapper.Map<IAsyncEnumerable<UserModel>>(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<UserModel> GetBuId(Guid id)
        {
            User user = await _userService.GetUserById(id);
            return _mapper.Map<UserModel>(user);
        }

        [HttpGet("{login:alpha}")]
        public async Task<UserModel> GetByLogin(string login)
        {
            User user = await _userService.GetUserByEmail(login);
            return _mapper.Map<UserModel>(user);
        }

        [HttpPost]
        public Task<Guid> Post([FromBody] CreateUserModel newUser)
        {
            User user = _mapper.Map<User>(newUser);
            return _userService.CreateUser(user);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
