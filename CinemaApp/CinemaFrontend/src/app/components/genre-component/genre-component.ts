import { Component } from '@angular/core';
import { GenericCrudComponent } from '../../generics/generic-component';
import { Genre } from '../../models/genre.model';
import { GenreService } from '../../services/genre/genre';
import { Router } from '@angular/router';
import { GenericReusableTable } from '../../generics/generic-reusable-table/generic-reusable-table';

@Component({
  selector: 'app-genre-component',
  imports: [ GenericReusableTable],
  templateUrl: './genre-component.html',
  styleUrl: './genre-component.css',
})
export class GenreComponent extends GenericCrudComponent<Genre> {
  constructor(private genreService: GenreService, public router: Router) {
    super(genreService);
  }
}
