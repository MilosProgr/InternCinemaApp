import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GenericTableComponent, TableColumn } from '../../generics/generic-reusable-table/generic-reusable-table';
import { CommonModule } from '@angular/common';
import { GenericCrudComponent } from '../../generics/generic-component';
import { Movie } from '../../models/movie.model';
import { MovieService } from '../../services/movie/movie-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-movie-component',
  imports: [GenericTableComponent,CommonModule],
  templateUrl: './movie-component.html',
  styleUrl: './movie-component.css',
})
export class MovieComponent extends GenericCrudComponent<Movie> implements OnInit {
  columns: TableColumn<Movie>[] = [
  
      {
        label: 'Id',
        key: 'id'
      },
      {
        label: 'Name',
        key: 'name'
      },
      {
        label: 'OriginalName',
        key: 'originalName'
      },
      {
        label: 'Duration',
        key: 'duration'
      },
      {
        label: 'PosterUrl',
        key: 'posterUrl'
      },
     
      

    ];

    constructor(private movieService: MovieService,
      public router: Router,cdr: ChangeDetectorRef
    ){
      super(movieService,cdr)
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
    
    // movie() {
    //   this.router.navigate(['/Movie']);
    // }
    // movie() {
    //   this.router.navigate(['/Movie']);
    // }
}
