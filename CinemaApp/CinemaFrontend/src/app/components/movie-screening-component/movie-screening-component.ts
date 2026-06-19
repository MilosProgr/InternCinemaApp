import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GenericCrudComponent } from '../../generics/generic-component';
import { MovieScreening } from '../../models/movie_screening.model';
import { TableColumn } from '../../generics/generic-reusable-table/generic-reusable-table';
import { MovieScreeningService } from '../../services/movie/movie-screening-service';
import { Router } from '@angular/router';
import { GenericTableComponent } from '../../generics/generic-reusable-table/generic-reusable-table';
@Component({
  selector: 'app-movie-screening-component',
  imports: [GenericTableComponent],
  templateUrl: './movie-screening-component.html',
  styleUrl: './movie-screening-component.css',
})
export class MovieScreeningComponent extends GenericCrudComponent<MovieScreening> implements OnInit {
  columns: TableColumn<MovieScreening>[] = [
    
        {
          label: 'Id',
          key: 'id'
        },
        {
          label: 'StartTime',
          key: 'startTime'
        },
        {
          label: 'Ticket Price',
          key: 'ticketPrice'
        },
        {
          label: 'Available Seats',
          key: 'availableSeats'
        },
      ];

      constructor(private movie_screeningService: MovieScreeningService,public router: Router,cdr: ChangeDetectorRef)
      {
        super(movie_screeningService,cdr)
      }

      override ngOnInit(): void {
        super.ngOnInit();
      }

      override getAllEntities() {
      this.crudService.getPaged(this.currentPage, this.pageSize)
        .subscribe(result => {

          console.log(result);

          this.entities = [...result.items];
          this.totalPages = result.totalPages;
          this.links = result.links;
          this.cdr.detectChanges();
        });
    }
}
