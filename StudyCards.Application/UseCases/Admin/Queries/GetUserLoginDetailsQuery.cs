using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.Admin.Queries;

public class GetUserLoginDetailsQuery : IQuery<IEnumerable<User>>
{
}

public class GetUserLoginDetailsQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserLoginDetailsQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetUserLoginDetailsQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAll(cancellationToken);
        return users;
    }
}
