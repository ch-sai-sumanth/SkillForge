using Application.DTOs;
using MediatR;

namespace Application.Queries.User;

public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>> { }
