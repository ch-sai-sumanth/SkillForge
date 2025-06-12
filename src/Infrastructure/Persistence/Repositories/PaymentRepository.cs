using Application.Interfaces.Repositories;
using MongoDB.Driver;
using User.Domain.Entities;

namespace Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> _paymentCollection;

    public PaymentRepository(IMongoDatabase database)
    {
        _paymentCollection = database.GetCollection<Payment>("Payments");
    }

    // CREATE
    public async Task CreateAsync(Payment payment) =>
        await _paymentCollection.InsertOneAsync(payment);
    

    // READ (By MenteeId)
    public async Task<List<Payment>> GetPaymentsByMenteeAsync(string menteeId) =>
        await _paymentCollection.Find(p => p.MenteeId == menteeId).ToListAsync();

    // READ (By ID)
    public async Task<Payment?> GetByIdAsync(string id) =>
        await _paymentCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

    public Task<List<Payment>> GetAllAsync()
    {
        return _paymentCollection.Find(_ => true).ToListAsync();
    }

    // UPDATE
    public async Task<bool> UpdateAsync(Payment payment)
    {
        var result = await _paymentCollection.ReplaceOneAsync(p => p.Id == payment.Id, payment);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _paymentCollection.DeleteOneAsync(p => p.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
    

    // READ (By UserId) - for fallback compatibility
    public async Task<List<Payment>> GetPaymentsByUserIdAsync(string userId) =>
        await _paymentCollection.Find(p => p.MenteeId == userId).ToListAsync();
}