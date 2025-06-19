using Application.DTOs;
using MediatR;

namespace Application.Commands.UpdateProfile;


public record UpdateMyProfileCommand(UpdateUserProfileDto Dto) : IRequest<UserDto>;

