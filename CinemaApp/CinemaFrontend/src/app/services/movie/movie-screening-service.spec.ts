import { TestBed } from '@angular/core/testing';

import { MovieScreeningService } from './movie-screening-service';

describe('MovieScreeningService', () => {
  let service: MovieScreeningService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MovieScreeningService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
