using Riok.Mapperly.Abstractions;
using StudyCards.Api.Models.Response;
using StudyCards.Domain.Entities;

namespace StudyCards.Api.Mapper;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DeckMapper
{
    public partial DeckResponse Map(Deck deck);
    public partial IEnumerable<DeckResponse> Map(IEnumerable<Deck> deck);
    public partial DeckSettingsResponse Map(DeckSettings settings);
    public partial DeckReviewStatusResponse Map(DeckReviewStatus status);
}