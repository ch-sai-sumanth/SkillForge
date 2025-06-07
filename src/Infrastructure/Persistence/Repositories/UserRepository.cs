using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using User.Domain.Repositories;

using UserEntity = User.Domain.Entities.User;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserEntity> _mongoCollection;

    public UserRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
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
}