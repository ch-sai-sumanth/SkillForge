using MongoDB.Driver;
using User.Domain.Repositories;
using UserEntity = User.Domain.Entities.User;
namespace Infrastructure.Repositories;

public class AuthRepository :IAuthRepository
{
    private readonly IMongoCollection<UserEntity> _usersCollection;

    public AuthRepository(IMongoDatabase database)
    {
        _usersCollection = database.GetCollection<UserEntity>("Users");
    }

    public async Task<UserEntity?> GetByRefreshTokenAsync(string refreshToken)
    {
        var filter = Builders<UserEntity>.Filter.Eq(u => u.RefreshToken, refreshToken);
        return await _usersCollection.Find(filter).FirstOrDefaultAsync();
    }
}