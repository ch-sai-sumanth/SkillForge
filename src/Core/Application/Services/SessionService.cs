using Application.DTOs;
using Application.Interfaces;
using User.Domain.Entities;
using User.Domain.Repositories;

namespace Application.Services;

public class SessionService : ISessionService
    
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;

    public SessionService(ISessionRepository repository,IUserRepository userRepository)
    {
        _sessionRepository = repository;
        _userRepository = userRepository;
    }

    public async Task<List<Session>> GetUserSessionsAsync(string userId)
    {
        return await _sessionRepository.GetSessionsByUserIdAsync(userId);
    }

    public async Task<Session> BookSessionAsync(BookSessionDto dto)
    {
        var mentor = await _userRepository.GetByIdAsync(dto.MentorId);
        if (mentor == null)
            throw new InvalidOperationException("Mentor not found.");
        
        var hasSkill = mentor.Skills.Any(skill =>
            string.Equals(skill, dto.Topic, StringComparison.OrdinalIgnoreCase));

        if (!hasSkill)
            throw new InvalidOperationException("Mentor does not have the required skill/topic.");

        var isAvailable = await IsMentorAvailableAsync(dto.MentorId, dto.StartTime, dto.EndTime);
        if (!isAvailable)
            throw new InvalidOperationException("Selected time is outside the mentor's available hours.");

        var isFree = await IsSessionSlotFreeAsync(dto.MentorId, dto.StartTime, dto.EndTime);
        if (!isFree)
            throw new InvalidOperationException("Mentor is already booked during the selected time.");

        var session = new Session
        {
            Id = Guid.NewGuid().ToString(),
            MentorId = dto.MentorId,
            MenteeId = dto.MenteeId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Topic = dto.Topic,
            Status = "Pending"
        };

        await _sessionRepository.CreateAsync(session);
        return session;
    }
    


    private async Task<bool> IsMentorAvailableAsync(string mentorId, DateTime requestedStart, DateTime requestedEnd)
    {
        var availability = await _sessionRepository.GetMentorAvailabilityAsync(mentorId);

        return availability.Any(a =>
            requestedStart >= a.StartTime &&
            requestedEnd <= a.EndTime);
    }

    private async Task<bool> IsSessionSlotFreeAsync(string mentorId, DateTime start, DateTime end, string? excludeSessionId = null)
    {
        var sessions = await _sessionRepository.GetSessionsByUserIdAsync(mentorId);

        return sessions
            .Where(s => s.Id != excludeSessionId)
            .All(s => s.EndTime <= start || s.StartTime >= end);
    }

    public async Task CancelSessionAsync(string sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new InvalidOperationException("Session not found.");

        if (session.Status == "Cancelled")
            throw new InvalidOperationException("Session is already cancelled.");

        session.Status = "Cancelled";
        await _sessionRepository.UpdateAsync(session);

        // Restore availability
        var availabilityList = await _sessionRepository.GetMentorAvailabilityAsync(session.MentorId);

        var newSlot = new MentorAvailability
        {
            MentorId = session.MentorId,
            StartTime = session.StartTime,
            EndTime = session.EndTime
        };

        availabilityList.Add(newSlot);

        // Merge overlapping slots
        var mergedAvailability = MergeAvailabilitySlots(availabilityList);

        await _sessionRepository.UpdateMentorAvailabilityAsync(session.MentorId, mergedAvailability);
    }

    public Task<Session?> GetSessionByIdAsync(string sessionId)
    {
        var session = _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new InvalidOperationException("Session not found.");
        return session;
    }
    
    private List<MentorAvailability> MergeAvailabilitySlots(List<MentorAvailability> slots)
    {
        var sorted = slots
            .OrderBy(s => s.StartTime)
            .ToList();

        var merged = new List<MentorAvailability>();

        foreach (var slot in sorted)
        {
            if (merged.Count == 0)
            {
                merged.Add(slot);
            }
            else
            {
                var last = merged.Last();
                if (last.MentorId == slot.MentorId && slot.StartTime <= last.EndTime)
                {
                    last.EndTime = new DateTime(Math.Max(last.EndTime.Ticks, slot.EndTime.Ticks));
                }
                else
                {
                    merged.Add(slot);
                }
            }
        }

        return merged;
    }
    
    public async Task UpdateSessionAsync(string sessionId, UpdateSessionDto dto)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new InvalidOperationException("Session not found.");
        
        session.StartTime = dto.StartTime;
        session.EndTime = dto.EndTime;
        session.Topic = dto.Topic;
        session.MeetingLink = dto.MeetingLink;
        session.Notes = dto.Notes;

        await _sessionRepository.UpdateAsync(session);
    }

    public async Task AcceptSessionAsync(string sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new InvalidOperationException("Session not found.");

        if (session.Status != "Pending")
            throw new InvalidOperationException("Only pending sessions can be accepted.");

        session.Status = "Scheduled";
        await _sessionRepository.UpdateAsync(session);
    }

    public async Task DeclineSessionAsync(string sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new InvalidOperationException("Session not found.");

        if (session.Status != "Pending")
            throw new InvalidOperationException("Only pending sessions can be declined.");

        session.Status = "Declined";
        await _sessionRepository.UpdateAsync(session);
    }

    public async Task<List<Session>> GetPendingSessionsForMentorAsync(string mentorId)
    {
        var mentor = await _userRepository.GetByIdAsync(mentorId);
        if (mentor == null || mentor.Role != "Mentor")
            throw new UnauthorizedAccessException("Only mentors can view pending sessions.");

        return await _sessionRepository.GetSessionsByMentorAndStatusAsync(mentorId, "Pending");
    }
}