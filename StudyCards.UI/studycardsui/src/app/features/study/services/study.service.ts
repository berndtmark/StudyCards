import { inject, Injectable } from '@angular/core';
import { StudyService as StudyServiceApi } from '../../../@api/services';
import { StudyMethodology } from '../../../shared/models/study-methodology';

@Injectable({
  providedIn: 'root'
})
export class StudyService {
  private studyServiceApi = inject(StudyServiceApi);

  getStudyCards(deckId: string, methodology: StudyMethodology) {
    return this.studyServiceApi.getstudycard$Json({ methodology, deckId });
  }
}
