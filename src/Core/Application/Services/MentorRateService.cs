using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using User.Domain.Entities;

namespace Application.Services;

public class MentorRateService : IMentorRateService
{
    private readonly IMentorRateRepository _rateRepository;
    private readonly IMapper _mapper;

    public MentorRateService(IMentorRateRepository rateRepository, IMapper mapper)
    {
        _rateRepository = rateRepository;
        _mapper = mapper;
    }
    
    public async Task SetMentorRateAsync(MentorRateDto dto)
    {
        var rate = _mapper.Map<MentorRate>(dto);
        rate.UpdatedAt = DateTime.UtcNow;
        await _rateRepository.UpsertRateAsync(rate);
    }

    public async Task<MentorRateDto?> GetMentorRateAsync(string mentorId)
    {
        var rate = await _rateRepository.GetRateByMentorIdAsync(mentorId);
        return rate == null ? null : _mapper.Map<MentorRateDto>(rate);
    }

    public async Task<MentorRate?> GetRateForMentorAsync(string mentorId)
    {
        return await _rateRepository.GetRateByMentorIdAsync(mentorId);
    }
    
    public async Task<bool> DeleteMentorRateAsync(string mentorId)
    {
        var rate = await _rateRepository.GetRateByMentorIdAsync(mentorId);
        if (rate == null) return false;

        await _rateRepository.DeleteRateAsync(mentorId);
        return true;
    }
}