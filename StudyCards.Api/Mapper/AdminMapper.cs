using Riok.Mapperly.Abstractions;
using StudyCards.Api.Models.Response;
using StudyCards.Domain.Entities;

namespace StudyCards.Api.Mapper;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AdminMapper
{
    [MapProperty(nameof(User.CreatedDate), nameof(UserDetails.UserCreated))]
    public partial UserDetails Map(User user);
    public partial IEnumerable<UserDetails> Map(IEnumerable<User> user);
}