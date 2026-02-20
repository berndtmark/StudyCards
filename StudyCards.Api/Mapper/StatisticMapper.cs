using Riok.Mapperly.Abstractions;
using StudyCards.Api.Models.Response;
using StudyCards.Domain.Entities;

namespace StudyCards.Api.Mapper;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class StatisticMapper
{
    public partial StudyStatisticResponse Map(StudyStatistic statistic);
    public partial IEnumerable<StudyStatisticResponse> Map(IEnumerable<StudyStatistic> statistic);
}
