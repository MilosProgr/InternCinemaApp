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
    this.crudService.getPaged(this.currentPage, this.pageSize).subscribe(result => {
        this.entities = [...result.items];
        this.totalPages = result.totalPages;
        this.links = result.links;
        this.cdr.detectChanges();
    });
}

  movie(){
    console.log('Kliknuto dugme za filmove');
    this.router.navigate(['/Movie']);
  }
  movieScreening() {
      this.router.navigate(['/MovieScreening']);
  }
  reservation() {
    this.router.navigate(['/Reservation']);
  }
  reservationSeat() {
    this.router.navigate(['/ReservationSeat']);
  }
  seat() {
    this.router.navigate(['/Seat']);
  }
  rating() {
    this.router.navigate(['/Rating']);
  }

}