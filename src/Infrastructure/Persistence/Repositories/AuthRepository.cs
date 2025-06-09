using MongoDB.Driver;
using User.Domain.Repositories;
using UserEntity = User.Domain.Entities.User;
namespace Infrastructure.Repositories;

public class AuthRepository :IAuthRepository
{
    private readonly IMongoCollection<UserEntity> _mongoCollection;

    public AuthRepository(IMongoDatabase database)
    {
        _mongoCollection = database.GetCollection<UserEntity>("Users");
    }

    public async Task<UserEntity?> GetByRefreshTokenAsync(string refreshToken)
    {
        var filter = Builders<UserEntity>.Filter.Eq(u => u.RefreshToken, refreshToken);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }
    public async Task InvalidateRefreshTokenAsync(string userId)
    {
        var update = Builders<UserEntity>.Update
            .Set(u => u.RefreshToken, null)
            .Set(u => u.RefreshTokenExpiryTime, null);

        await _mongoCollection.UpdateOneAsync(u => u.Id == userId, update);
    }
    
}