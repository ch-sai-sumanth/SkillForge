using MediatR;
using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Common;
using Application.Interfaces.Repositories;
using Domain.Repositories;

namespace Application.Commands.CreateGoal;

public class CreateGoalCommandHandlerDDD : IRequestHandler<CreateGoalCommandDDD, GoalDto>
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;

    public CreateGoalCommandHandlerDDD(IGoalRepository goalRepository, IMapper mapper)
    {
        _goalRepository = goalRepository;
        _mapper = mapper;
    }

    public async Task<GoalDto> Handle(CreateGoalCommandDDD request, CancellationToken cancellationToken)
    {
        try
        {
            // Create value objects with validation
            var menteeId = UserId.Create(request.MenteeId);
            var title = GoalTitle.Create(request.Title);
            var description = GoalDescription.Create(request.Description);
            var targetDate = TargetDate.Create(request.TargetDate);

            // Create the domain entity using factory method
            var learningGoal = LearningGoal.Create(menteeId, title, description, targetDate);

            // Save to repository
            await _goalRepository.AddAsync(learningGoal);

            // Map to DTO for response
            return _mapper.Map<GoalDto>(learningGoal);
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException($"Invalid goal data: {ex.Message}", ex);
        }
        catch (DomainException ex)
        {
            throw new InvalidOperationException($"Domain rule violation: {ex.Message}", ex);
        }
    }
}