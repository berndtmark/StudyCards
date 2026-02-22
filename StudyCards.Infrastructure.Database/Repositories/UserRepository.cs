using Microsoft.EntityFrameworkCore;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Infrastructure.Database.Context;

namespace StudyCards.Infrastructure.Database.Repositories;

public class UserRepository(DataBaseContext dbContext, ICurrentUser currentUser) : BaseRepository<User>(dbContext, currentUser), IUserRepository
{
    public async Task<User> Add(User user, CancellationToken cancellationToken)
    {
        EmailAddress = user.UserEmail;
        var entity = await AddEntity(user, cancellationToken);
        return entity;
    }

    public async Task<User?> Get(string userEmail, CancellationToken cancellationToken)
    {
        return await DbContext.User
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.UserEmail == userEmail, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
    {
        return await DbContext.User
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public User Update(User user)
    {
        EmailAddress = user.UserEmail;
        var entity = UpdateEntity(user);
        return entity;
    }
}
