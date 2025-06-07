using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserEntity = User.Domain.Entities.User;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService,IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    // GET: api/user
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    // GET: api/user/{id}
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

// POST: api/user
    [HttpPost]
    public async Task<IActionResult> Create(UserDto userDto)
    {
        userDto.Id = string.Empty;
    
        await _userService.CreateAsync(userDto);
        return Created();
    }

    // PUT: api/user/{id}
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, UserDto userDto)
    {
        if (id != userDto.Id)
            return BadRequest("ID mismatch");

        await _userService.UpdateAsync(userDto);
        return NoContent();
    }

    // DELETE: api/user/{id}
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}