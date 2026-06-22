import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovieGenreComponent } from './movie-genre-component';

describe('MovieGenreComponent', () => {
  let component: MovieGenreComponent;
  let fixture: ComponentFixture<MovieGenreComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MovieGenreComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MovieGenreComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
