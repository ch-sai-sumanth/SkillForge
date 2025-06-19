using MediatR;

namespace Application.Commands.DeleteAccount;

public record DeleteMyAccountCommand() : IRequest<bool>;
