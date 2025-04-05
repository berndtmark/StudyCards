import { TestBed } from '@angular/core/testing';

import { StudyService } from './study.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('StudyService', () => {
  let service: StudyService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ],
    });
    service = TestBed.inject(StudyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
