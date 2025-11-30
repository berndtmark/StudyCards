import { TestBed } from '@angular/core/testing';

import { AuthorizeService } from './authorize.service';

describe('AuthorizeService', () => {
  let service: AuthorizeService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
      ]
    });
    service = TestBed.inject(AuthorizeService);
  });

  it('should be created', async () => {
    expect(service).toBeTruthy();
  });
});
