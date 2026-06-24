import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationFailure } from './reservation-failure';

describe('ReservationFailure', () => {
  let component: ReservationFailure;
  let fixture: ComponentFixture<ReservationFailure>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReservationFailure],
    }).compileComponents();

    fixture = TestBed.createComponent(ReservationFailure);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
