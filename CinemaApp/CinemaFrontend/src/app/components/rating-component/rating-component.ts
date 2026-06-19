import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GenericCrudComponent } from '../../generics/generic-component';
import { Rating } from '../../models/rating.model';
import { TableColumn } from '../../generics/generic-reusable-table/generic-reusable-table';
import { RatingService } from '../../services/rating/rating-service';
import { Router } from '@angular/router';
import { GenericTableComponent } from '../../generics/generic-reusable-table/generic-reusable-table';
@Component({
  selector: 'app-rating-component',
  imports: [GenericTableComponent],
  templateUrl: './rating-component.html',
  styleUrl: './rating-component.css',
})
export class RatingComponent extends GenericCrudComponent<Rating> implements OnInit {
  columns: TableColumn<Rating>[] = [
     {
          label: 'Id',
          key: 'id'
        },
        {
          label: 'Stars',
          key: 'stars'
        }
  ];

  constructor(private ratingService: RatingService,public router: Router,cdr: ChangeDetectorRef){
    super(ratingService,cdr)
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
