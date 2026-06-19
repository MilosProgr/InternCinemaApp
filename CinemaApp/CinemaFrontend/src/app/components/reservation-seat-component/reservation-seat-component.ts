import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GenericCrudComponent } from '../../generics/generic-component';
import { ReservationSeat } from '../../models/reservation_seat.model';
import { ReservationSeatService } from '../../services/reservation/reservation-seat-service';
import { Router } from '@angular/router';
import { TableColumn } from '../../generics/generic-reusable-table/generic-reusable-table';
import { GenericTableComponent } from '../../generics/generic-reusable-table/generic-reusable-table';
@Component({
  selector: 'app-reservation-seat-component',
  imports: [GenericTableComponent],
  templateUrl: './reservation-seat-component.html',
  styleUrl: './reservation-seat-component.css',
})
export class ReservationSeatComponent extends GenericCrudComponent<ReservationSeat> implements OnInit {
  
  columns: TableColumn<ReservationSeat>[] = [
      {
        label: 'Id',
        key: 'id'
      },
      {
        label: 'Seat number',
        key: 'seatNumber'
      }
      
  
    ];
  
  constructor(private reservation_seat_service: ReservationSeatService,public router: Router,cdr: ChangeDetectorRef){
    super(reservation_seat_service,cdr);
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
