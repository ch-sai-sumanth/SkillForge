namespace User.Domain.Repositories;
using User.Domain.Entities;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string id);
    Task<List<User>> GetAllAsync();
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(string id);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<List<User>> GetMentorsBySkillsAsync(List<string> skills);


    Task<List<User>> GetMentorsBySkillAsync(string skill);
}