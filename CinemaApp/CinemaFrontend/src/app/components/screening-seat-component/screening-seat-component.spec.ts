import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScreeningSeatComponent } from './screening-seat-component';

describe('ScreeningSeatComponent', () => {
  let component: ScreeningSeatComponent;
  let fixture: ComponentFixture<ScreeningSeatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScreeningSeatComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ScreeningSeatComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
