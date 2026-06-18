import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenericReusableTable } from './generic-reusable-table';

describe('GenericReusableTable', () => {
  let component: GenericReusableTable;
  let fixture: ComponentFixture<GenericReusableTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GenericReusableTable],
    }).compileComponents();

    fixture = TestBed.createComponent(GenericReusableTable);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
