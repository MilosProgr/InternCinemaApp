import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GenericCrudComponent } from '../../generics/generic-component';
import { Genre } from '../../models/genre.model';
import { GenreService } from '../../services/genre/genre';
import { Router } from '@angular/router';
import { GenericTableComponent, TableColumn } from '../../generics/generic-reusable-table/generic-reusable-table';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-genre-component',
  standalone: true,
  imports: [
    GenericTableComponent,
    CommonModule
  ],
  templateUrl: './genre-component.html',
  styleUrl: './genre-component.css',
})
export class GenreComponent 
extends GenericCrudComponent<Genre>
implements OnInit {


  columns: TableColumn<Genre>[] = [

    {
      label: 'Name',
      key: 'name'
    }

  ];


  constructor(
    private genreService: GenreService,
    public router: Router,
    cdr: ChangeDetectorRef
  ) {

    super(genreService, cdr);

  }



  override ngOnInit(): void {

    super.ngOnInit();
    console.log('GenreComponent ngOnInit, entities:', this.entities);  // ← dodaj ovo


  }

  override getAllEntities() {
    this.crudService.getAll().subscribe(entities => {
        console.log('Podaci stigli:', entities);  // ← dodaj ovo
        this.entities = [...entities];  // novi niz, nova referenca
        this.cdr.detectChanges(); 
        console.log('entities posle assignovanja:', this.entities);  // ← i ovo
    });
}



}