using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> Get(string userEmail, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken);
    Task<User> Add(User user, CancellationToken cancellationToken);
    User Update(User user);
}
