using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using User.Domain.Repositories;
using UserEntity = User.Domain.Entities.User;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository _userRepository,IMapper _mapper)
    {
        this._userRepository = _userRepository;
        this._mapper = _mapper;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task CreateAsync(UserDto userDto)
    {
        var user = _mapper.Map<UserEntity>(userDto);
        
        // Generate new ID for creation (ignore any ID from DTO)
        user.Id = Guid.NewGuid().ToString();
        
        await _userRepository.CreateAsync(user);
    }

    public async Task UpdateAsync(UserDto userDto)
    {
        var user = _mapper.Map<UserEntity>(userDto);
        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(string id)
    {
        await _userRepository.DeleteAsync(id);
    }

}