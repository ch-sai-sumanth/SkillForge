using Application.Interfaces.Repositories;
using MongoDB.Driver;
using User.Domain.Entities;

namespace Infrastructure.Repositories;

public class MentorRateRepository : IMentorRateRepository
{
    private readonly IMongoCollection<MentorRate> _collection;

    public MentorRateRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<MentorRate>("MentorRates");
    }

    // READ (By Mentor ID)
    public async Task<MentorRate?> GetByMentorIdAsync(string mentorId) =>
        await _collection.Find(r => r.MentorId == mentorId).FirstOrDefaultAsync();

    // CREATE or UPDATE (Upsert)
    public async Task SetOrUpdateRateAsync(MentorRate rate)
    {
        var existing = await GetByMentorIdAsync(rate.MentorId);
        if (existing == null)
            await _collection.InsertOneAsync(rate);
        else
            await _collection.ReplaceOneAsync(r => r.MentorId == rate.MentorId, rate);
    }

    // READ (Alternative naming)
    public async Task<MentorRate?> GetRateByMentorIdAsync(string mentorId) =>
        await GetByMentorIdAsync(mentorId);

    // UPSERT (Direct MongoDB Upsert)
    public async Task UpsertRateAsync(MentorRate rate)
    {
        var filter = Builders<MentorRate>.Filter.Eq(r => r.MentorId, rate.MentorId);
        await _collection.ReplaceOneAsync(filter, rate, new ReplaceOptions { IsUpsert = true });
    }
    

    // DELETE (Optional)
    public async Task<bool> DeleteRateAsync(string mentorId)
    {
        var result = await _collection.DeleteOneAsync(r => r.MentorId == mentorId);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

}