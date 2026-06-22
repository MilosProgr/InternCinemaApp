import { TestBed } from '@angular/core/testing';

import { ScreeningSeatService } from './screening-seat-service';

describe('ScreeningSeatService', () => {
  let service: ScreeningSeatService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ScreeningSeatService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
