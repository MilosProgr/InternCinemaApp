import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GenericCrudComponent } from '../../../generics/generic-component';
import { MovieScreening } from '../../../models/movie_screening.model';
import { ScreeningSeat } from '../../../models/screeningSeat';
import { MovieScreeningService } from '../../../services/movie/movie-screening-service';
import { ReservationService } from '../../../services/reservation/reservation-service';
import {
  Reservation,
  CreateReservationRequest
} from '../../../models/reservation.model';
import { LoginService } from '../../../services/login/login';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reservation-component',
  standalone: true,
  imports: [CommonModule,FormsModule,DatePipe],
  templateUrl: './reservation-component.html',
  styleUrl: './reservation-component.css',
})
export class ReservationComponent
  extends GenericCrudComponent<Reservation>
  implements OnInit
{
  screening: MovieScreening | null = null;
  seats: ScreeningSeat[] = [];
  selectedSeats: ScreeningSeat[] = [];
  guestEmail: string = '';
  isAuthenticated: boolean = false;
  showConfirmDialog: boolean = false;

  readonly MAX_TICKETS = 5;
  readonly DISCOUNT = 0.05;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private screeningService: MovieScreeningService,
    private reservationService: ReservationService,
    private loginService: LoginService,
    public override cdr: ChangeDetectorRef
  ) {
    super(reservationService, cdr);
  }

  override ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    this.isAuthenticated = this.loginService.loggedIn;

    if (!id) {
      return;
    }

    this.screeningService.getOne(+id).subscribe({
      next: (screening) => {
        this.screening = screening;
      },
      error: (err) => {
        console.error(err);
      }
    });

    this.screeningService.getSeats(+id).subscribe({
      next: (seats: ScreeningSeat[]) => {
        this.seats = seats;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  get rows(): string[] {
    return ['A', 'B', 'C', 'D', 'E', 'F', 'G'];
  }

  getSeatsForRow(row: string): ScreeningSeat[] {
    return this.seats
      .filter(seat => seat.row === row)
      .sort((a, b) => a.number - b.number);
  }

  getSeatClass(seat: ScreeningSeat): string {
    if (seat.isOccupied) {
      return 'occupied';
    }

    if (this.selectedSeats.some(s => s.id === seat.id)) {
      return 'selected';
    }

    return 'available';
  }

  toggleSeat(seat: ScreeningSeat): void {
    if (seat.isOccupied) {
      return;
    }

    const index = this.selectedSeats.findIndex(
      s => s.id === seat.id
    );

    if (index >= 0) {
      this.selectedSeats.splice(index, 1);
    } else if (this.selectedSeats.length < this.MAX_TICKETS) {
      this.selectedSeats.push(seat);
    }
  }

  get basePrice(): number {
    return (
      (this.screening?.ticketPrice ?? 0) *
      this.selectedSeats.length
    );
  }

  get totalPrice(): number {
    return this.isAuthenticated
      ? this.basePrice * (1 - this.DISCOUNT)
      : this.basePrice;
  }

  confirm(): void {
    if (this.selectedSeats.length === 0) {
      return;
    }

    this.showConfirmDialog = true;
  }

  makeReservation(): void {
    if (!this.screening) {
      return;
    }

    const payload: CreateReservationRequest = {
      movieScreeningId: this.screening.id,
      seatIds: this.selectedSeats.map(s => s.id),
      guestEmail: this.isAuthenticated
        ? null
        : this.guestEmail,
      totalPrice: this.totalPrice
    };

    this.reservationService.createReservation(payload).subscribe({
      next: (reservation) => {
        console.log('Reservation created:', reservation);
        this.router.navigate(['/reservation/success']);
      },
      error: (err) => {
        console.error(err);
        this.router.navigate(['/reservation/failure']);
      }
    });
  }

  cancelConfirmation(): void {
    this.showConfirmDialog = false;
  }
}