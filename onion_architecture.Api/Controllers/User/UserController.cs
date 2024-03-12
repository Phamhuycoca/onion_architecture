using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onion_architecture.Application.Features.Dto.UserDto;
using onion_architecture.Application.IService;

namespace onion_architecture.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult GetAll(int page = 1, int size = 10, string? keyword = "")
        {
            return Ok(_userService.Items(page, size, keyword));
        }
        [HttpPost]
        public IActionResult Create(CreateUser dto)
        {
            return Ok(_userService.Create(dto));
        }
        [HttpPatch("{id}")]
        public IActionResult Update(long id,UpdateUser dto)
        {
            return Ok(_userService.Update(id, dto));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            return Ok(_userService.GetById(id));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            return Ok(_userService?.Delete(id));
        }
    }
}
