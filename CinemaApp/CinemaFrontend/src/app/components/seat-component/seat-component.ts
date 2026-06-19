import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GenericTableComponent, TableColumn } from '../../generics/generic-reusable-table/generic-reusable-table';
import { GenericCrudComponent } from '../../generics/generic-component';
import { Seat } from '../../models/seat.model';
import { SeatService } from '../../services/seat/seat-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-seat-component',
  imports: [GenericTableComponent],
  templateUrl: './seat-component.html',
  styleUrl: './seat-component.css',
})
export class SeatComponent extends GenericCrudComponent<Seat> implements OnInit {

  columns: TableColumn<Seat>[] = [
    {
        label: 'Id',
        key: 'id'
      },
      {
        label: 'Row',
        key: 'row'
      },
      {
        label: 'Number',
        key: 'number'
      }
  ];

  constructor(private seatService: SeatService, private router: Router, cdr: ChangeDetectorRef){
    super(seatService,cdr);
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
