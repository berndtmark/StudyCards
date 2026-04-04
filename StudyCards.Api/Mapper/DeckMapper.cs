using Riok.Mapperly.Abstractions;
using StudyCards.Api.Models.Response;
using StudyCards.Domain.Entities;
using StudyCards.Domain.ValueObjects;

namespace StudyCards.Api.Mapper;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DeckMapper
{
    [MapperIgnoreTarget(nameof(DeckResponse.CardNoToReview))]
    public partial DeckResponse Map(Deck deck);
    public partial IEnumerable<DeckResponse> Map(IEnumerable<Deck> deck);
    public partial DeckSettingsResponse Map(DeckSettings settings);
    public partial DeckReviewStatusResponse Map(DeckReviewStatus status);

    public DeckResponse Map(Deck deck, TimeZoneInfo timeZoneInfo)
    {
        var response = Map(deck);
        response.CardNoToReview = deck.CardNoToReview(timeZoneInfo);

        return response;
    }

    public IEnumerable<DeckResponse> Map(IEnumerable<Deck> deck, TimeZoneInfo timeZoneInfo)
    {
        return deck.Select(deck => Map(deck, timeZoneInfo));
    }
}