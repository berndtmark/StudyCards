import { TestBed } from '@angular/core/testing';

import { HubService, SIGNALR_URL } from './hub.service';

describe('HubService', () => {
  let service: HubService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        HubService,
        { provide: SIGNALR_URL, useValue: 'http://localhost/chatHub' }
      ]
    });
    service = TestBed.inject(HubService);
  });

  it('should be created', async () => {
    expect(service).toBeTruthy();
  });
});
