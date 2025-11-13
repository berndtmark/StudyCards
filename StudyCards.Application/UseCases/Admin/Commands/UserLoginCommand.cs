using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.Admin.Commands;

public class UserLoginCommand : ICommand<bool>
{
    public string UserEmail { get; set; } = string.Empty;
}

public class UserLoginCommandHandler(IUnitOfWork unitOfWork, ILogger<UserLoginCommandHandler> logger) : ICommandHandler<UserLoginCommand, bool>
{
    public async Task<bool> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.UserEmail == null)
                throw new ApplicationException("User Email Not Found!");

            var user = await unitOfWork.UserRepository.Get(request.UserEmail, cancellationToken);
            
            if (user == null)
            {
                // Create new user
                var newUser = User.CreateUser(request.UserEmail)
                    .UserLogin();

                await unitOfWork.UserRepository.Add(newUser, cancellationToken);
            }
            else
            {
                user = user.UserLogin();
                unitOfWork.UserRepository.Update(user);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        { 
            logger.LogError(ex, "Failed to Create User Profile for {UserEmail}", request.UserEmail);
            throw;
        }
        
    }
}