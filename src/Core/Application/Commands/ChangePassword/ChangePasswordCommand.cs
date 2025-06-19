using Application.DTOs;
using MediatR;

namespace Application.Commands.ChangePassword;

public record ChangePasswordCommand(ChangePasswordDto Dto) : IRequest<bool>;
