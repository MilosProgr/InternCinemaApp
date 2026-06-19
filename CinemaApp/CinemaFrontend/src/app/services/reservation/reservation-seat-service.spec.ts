import { TestBed } from '@angular/core/testing';

import { ReservationSeatService } from './reservation-seat-service';

describe('ReservationSeatService', () => {
  let service: ReservationSeatService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReservationSeatService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
