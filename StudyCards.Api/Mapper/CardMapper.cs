using Riok.Mapperly.Abstractions;
using StudyCards.Api.Models.Response;
using StudyCards.Application.Common;
using StudyCards.Domain.Entities;

namespace StudyCards.Api.Mapper;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CardMapper
{
    [MapProperty(nameof(Card.CardReviewStatus.ReviewCount), nameof(CardResponse.ReviewCount))]
    [MapProperty(nameof(Card.CardReviewStatus.NextReviewDate), nameof(CardResponse.NextReviewDate))]
    [MapProperty(nameof(Card.CardReviewStatus.CurrentPhase), nameof(CardResponse.ReviewPhase))]
    public partial CardResponse Map(Card card);
    public partial IEnumerable<CardResponse> Map(IEnumerable<Card> cards);

    public PagedResult<CardResponse> Map(PagedResult<Card> source)
    {
        var mappedItems = source.Items
            .Select(Map)
            .ToList();

        return new PagedResult<CardResponse>(
            mappedItems,
            source.TotalCount,
            source.PageNumber,
            source.PageSize
        );
    }
}