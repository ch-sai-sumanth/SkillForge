namespace User.Domain.Repositories;
using User.Domain.Entities;

public interface IAuthRepository
{
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
    Task InvalidateRefreshTokenAsync(string userId);

}