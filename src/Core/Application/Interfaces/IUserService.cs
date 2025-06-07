using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task CreateAsync(UserDto user);
    Task UpdateAsync(UserDto user);
    Task DeleteAsync(string id);
}