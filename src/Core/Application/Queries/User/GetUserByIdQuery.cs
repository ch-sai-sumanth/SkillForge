using Application.DTOs;
using MediatR;

namespace Application.Queries.User;

public class GetUserByIdQuery : IRequest<UserDto?>
{
    public string Id { get; set; }

    public GetUserByIdQuery(string id) => Id = id;
}