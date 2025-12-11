import { TestBed } from '@angular/core/testing';

import { StudyService } from './study.service';

describe('StudyService', () => {
  let service: StudyService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
      ],
    });
    service = TestBed.inject(StudyService);
  });

  it('should be created', async () => {
    expect(service).toBeTruthy();
  });
});
