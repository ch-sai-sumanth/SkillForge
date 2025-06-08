using System.Security.Cryptography;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using User.Domain.Repositories;
using UserEntity = User.Domain.Entities.User;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public AuthService(IUserRepository _userRepository,IMapper _mapper)
    {
        this._userRepository = _userRepository;
        this._mapper = _mapper;
    }
    public async Task CreateUserAsync(RegisterRequestDto registerDto)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var newUser = new UserEntity()
        {
            Id = Guid.NewGuid().ToString(),
            Username = registerDto.Username,
            Email = registerDto.Email,
            Password = passwordHash,
            Role = registerDto.Role,
        };
        
        await _userRepository.CreateAsync(newUser);
    }
    
    public async Task<UserDto?> ValidateUserAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null || string.IsNullOrWhiteSpace(user.Password))
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }
    
    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}