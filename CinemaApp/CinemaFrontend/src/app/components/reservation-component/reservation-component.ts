import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GenericCrudComponent } from '../../generics/generic-component';
import { Reservation } from '../../models/reservation.model';
import { TableColumn } from '../../generics/generic-reusable-table/generic-reusable-table';
import { ReservationService } from '../../services/reservation/reservation-service';
import { Router } from '@angular/router';
import { GenericTableComponent } from '../../generics/generic-reusable-table/generic-reusable-table';
@Component({
  selector: 'app-reservation-component',
  imports: [GenericTableComponent],
  templateUrl: './reservation-component.html',
  styleUrl: './reservation-component.css',
})
export class ReservationComponent extends GenericCrudComponent<Reservation> implements OnInit {
  columns: TableColumn<Reservation>[] = [
    {
      label: 'Id',
      key: 'id'
    },
    {
      label: 'Guest Email',
      key: 'guestEmail'
    },
    {
      label: 'Code Reservation',
      key: 'reservationCode'
    },
    {
      label: 'Total Price',
      key: 'totalPrice'
    },
    {
      label: 'Status',
      key: 'isCancelled'
    }

  ];

  constructor(private reservationService: ReservationService,public router : Router,cdr: ChangeDetectorRef){
    super(reservationService,cdr);
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
