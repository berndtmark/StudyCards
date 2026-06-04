import { inject, Service } from '@angular/core';
import { StatisticService as StatisticServiceApi } from '../../../@api/services';
import { Observable } from 'rxjs';
import { StudyStatisticResponse } from '../../../@api/models';

@Service()
export class StatisticService {
  private statisticServiceApi = inject(StatisticServiceApi)

  getStudyStatistics(from: Date, to: Date): Observable<StudyStatisticResponse[]> {
    return this.statisticServiceApi.getStudyStatistics$Json({ from: from.toISOString(), to: to.toISOString() });
  }
}
