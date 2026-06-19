import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationSeatComponent } from './reservation-seat-component';

describe('ReservationSeatComponent', () => {
  let component: ReservationSeatComponent;
  let fixture: ComponentFixture<ReservationSeatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReservationSeatComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ReservationSeatComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
