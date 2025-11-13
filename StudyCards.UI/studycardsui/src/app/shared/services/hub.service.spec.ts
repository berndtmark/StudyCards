import { TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { HubService, SIGNALR_URL } from './hub.service';

describe('HubService', () => {
  let service: HubService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        HubService,
        { provide: SIGNALR_URL, useValue: 'http://localhost/chatHub' },
        provideZonelessChangeDetection()
      ]
    });
    service = TestBed.inject(HubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
