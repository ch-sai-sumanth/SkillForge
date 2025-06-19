using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Commands.sessions;

public class BookSessionHandler : IRequestHandler<BookSessionCommand, SessionDto>
{
    private readonly ISessionService _service;
    public BookSessionHandler(ISessionService service) => _service = service;
    public Task<SessionDto> Handle(BookSessionCommand request, CancellationToken ct) => _service.BookSessionAsync(request.Dto);
}