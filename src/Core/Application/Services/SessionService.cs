using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain;
using User.Domain.Entities;
using User.Domain.Repositories;

namespace Application.Services;

public class SessionService : ISessionService
    
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISessionHistoryRepository _sessionHistoryRepository;
    private readonly IMapper _mapper;

    public SessionService(ISessionRepository repository,IUserRepository userRepository,ISessionHistoryRepository sessionHistoryRepository,IMapper mapper)
    {
        _sessionRepository = repository;
        _userRepository = userRepository;
        _sessionHistoryRepository = sessionHistoryRepository;
        _mapper = mapper;
    }

    public async Task<List<SessionDto>> GetUserSessionsAsync(string userId)
    {
        List<Session> sessions = await _sessionRepository.GetSessionsByUserIdAsync(userId);
        return _mapper.Map<List<SessionDto>>(sessions);
    }

    public async Task<SessionDto> BookSessionAsync(BookSessionDto dto)
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
            Status = SessionStatus.Pending
        };

        await _sessionRepository.CreateAsync(session);
        return _mapper.Map<SessionDto>(session);
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
            .Where(s => s.Id != excludeSessionId && s.Status != SessionStatus.Cancelled)
            .All(s => s.EndTime <= start || s.StartTime >= end);
    }

    public async Task<bool> CancelSessionAsync(string sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new InvalidOperationException("Session not found.");

        if (session.Status == SessionStatus.Cancelled)
            throw new InvalidOperationException("Session is already cancelled.");

        session.Status = SessionStatus.Cancelled;
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
        return true;
    }

    public async Task<SessionDto?> GetSessionByIdAsync(string sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new InvalidOperationException("Session not found.");
        return _mapper.Map<SessionDto>(session);
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
            throw new NotFoundException($"Session with ID '{sessionId}' not found.");

        var isTimeChanged = session.StartTime != dto.StartTime || session.EndTime != dto.EndTime;
        var oldStatus = session.Status;

        session.Topic = dto.Topic;
        session.MeetingLink = dto.MeetingLink;
        session.Notes = dto.Notes;
        session.StartTime = dto.StartTime;
        session.EndTime = dto.EndTime;
        session.Status = dto.Status?.Trim().ToLowerInvariant() switch
        {
            "pending" => SessionStatus.Pending,
            "accepted" => SessionStatus.Accepted,
            "declined" => SessionStatus.Declined,
            "completed" => SessionStatus.Completed,
            "cancelled" => SessionStatus.Cancelled,
            _ => throw new InvalidOperationException("Invalid session status.")
        };

        if (isTimeChanged && session.Status != SessionStatus.Cancelled)
        {
            session.Status = SessionStatus.Rescheduled;

            var history = new SessionHistory
            {
                SessionId = session.Id,
                OldStatus = oldStatus,
                NewStatus = SessionStatus.Rescheduled,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = dto.UpdatedBy ?? "System", // You can add this to UpdateSessionDto
                ChangeReason = "Rescheduled by user"
            };

            await _sessionHistoryRepository.AddAsync(history);
        }

        await _sessionRepository.UpdateAsync(session);
    }

    public async Task AcceptSessionAsync(string sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new InvalidOperationException("Session not found.");

        if (session.Status != SessionStatus.Pending)
            throw new InvalidOperationException("Only pending sessions can be accepted.");

        session.Status = SessionStatus.Accepted;
        await _sessionRepository.UpdateAsync(session);
    }

    public async Task DeclineSessionAsync(string sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new InvalidOperationException("Session not found.");

        if (session.Status != SessionStatus.Pending)
            throw new InvalidOperationException("Only pending sessions can be declined.");

        session.Status = SessionStatus.Declined;
        await _sessionRepository.UpdateAsync(session);
    }

    public async Task<List<SessionDto>> GetPendingSessionsForMentorAsync(string mentorId)
    {
        var mentor = await _userRepository.GetByIdAsync(mentorId);
        if (mentor == null || mentor.Role != "Mentor")
            throw new UnauthorizedAccessException("Only mentors can view pending sessions.");

        List<Session> sessions= await _sessionRepository.GetSessionsByMentorAndStatusAsync(mentorId, "Pending");
        if (sessions == null || sessions.Count == 0)
            throw new NotFoundException($"No pending sessions found for mentor with ID '{mentorId}'.");
        return _mapper.Map<List<SessionDto>>(sessions);
    }
    public async Task<bool> CompleteSessionAsync(string sessionId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new NotFoundException($"Session with ID '{sessionId}' not found.");

        if (session.Status != SessionStatus.Accepted)
            throw new InvalidOperationException($"Only accepted sessions can be marked as completed. Current status: {session.Status}");

        if (session.EndTime > DateTime.UtcNow)
            throw new InvalidOperationException("Cannot mark session as completed before it has ended.");

        session.Status = SessionStatus.Completed;

        await _sessionRepository.UpdateAsync(session);
        return true;
    }

    public async Task<List<SessionHistoryDto>> GetSessionHistoryAsync(string sessionId)
    {
        List<SessionHistory> sessionHistories= await _sessionHistoryRepository.GetBySessionIdAsync(sessionId);
        if (sessionHistories == null || sessionHistories.Count == 0)
            throw new NotFoundException($"No history found for session with ID '{sessionId}'.");
        return _mapper.Map<List<SessionHistoryDto>>(sessionHistories);
    }
}