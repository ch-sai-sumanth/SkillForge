using Application.DTOs;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using User.Domain.Entities;
using User.Domain.Repositories;

using UserEntity = User.Domain.Entities.User;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserEntity> _mongoCollection;

    public UserRepository(IMongoDatabase database)
    {
        _mongoCollection = database.GetCollection<UserEntity>("Users");
    }

    public async Task<UserEntity> GetByIdAsync(string id) =>
        await _mongoCollection.Find(u => u.Id == id).FirstOrDefaultAsync();

    public async Task<List<UserEntity>> GetAllAsync() =>
        await _mongoCollection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(UserEntity user) =>
        await _mongoCollection.InsertOneAsync(user);

    public async Task UpdateAsync(UserEntity user) =>
        await _mongoCollection.ReplaceOneAsync(u => u.Id == user.Id, user);

    public async Task DeleteAsync(string id) =>
        await _mongoCollection.DeleteOneAsync(u => u.Id == id);
    
    public async Task<UserEntity?> GetByUsernameAsync(string username)
    {
        return await _mongoCollection
            .Find(u => u.Username == username)
            .FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;
        
        return await _mongoCollection
            .Find(u => u.Email.ToLower() == email.ToLower())
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<UserEntity>> GetMentorsBySkillsAsync(List<string> skills)
    {
        var filterBuilder = Builders<UserEntity>.Filter;
        var filter = filterBuilder.Eq(u => u.Role, "Mentor") &
                     filterBuilder.AnyIn(u => u.Skills, skills);

        return await _mongoCollection.Find(filter).ToListAsync();
    }

    public async Task<List<UserEntity>> GetMentorsBySkillAsync(string skill)
    {
        var filter = Builders<UserEntity>.Filter.Regex(u => u.Skills, new MongoDB.Bson.BsonRegularExpression($"^{skill}$", "i"));
        return await _mongoCollection.Find(filter).ToListAsync();
        
    }

}