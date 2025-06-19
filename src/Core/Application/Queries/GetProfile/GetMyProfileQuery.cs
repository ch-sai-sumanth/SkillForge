using Application.DTOs;
using MediatR;

namespace Application.Queries.GetProfile;

public record GetMyProfileQuery() : IRequest<UserDto>;

