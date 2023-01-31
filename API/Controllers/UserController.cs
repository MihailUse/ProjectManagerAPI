﻿using API.Models.User;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserController(IMapper mapper, UserService userService)
        {
            _mapper = mapper;
            _userService = userService;
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
        public async Task UpdateUser(Guid id, [FromForm] UpdateUserModel userOptions)
        {
            User user = _mapper.Map<User>(userOptions);
            await _userService.UpdateUser(id, user);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task DeleteUser(Guid id)
        {
            await _userService.DeleteUser(id);
        }
    }
}
