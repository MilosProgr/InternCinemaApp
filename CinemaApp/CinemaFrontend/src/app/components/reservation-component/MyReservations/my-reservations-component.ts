import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Reservation } from '../../../models/reservation.model';
import { ReservationService } from '../../../services/reservation/reservation-service';
import { RatingService } from '../../../services/rating/rating-service';

@Component({
  selector: 'app-my-reservations-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-reservations-component.html',
  styleUrl: './my-reservations-component.css',
})
export class MyReservationsComponent implements OnInit {

  currentReservations: Reservation[] = [];
  pastReservations: Reservation[] = [];
  activeTab: 'current' | 'past' = 'current';

  constructor(
    private reservationService: ReservationService,
    private ratingService: RatingService
  ) {}

  ngOnInit(): void {
    this.loadReservations();
  }

  loadReservations(): void {
    this.reservationService.getMyReservations().subscribe(all => {
      const now = new Date();
      this.currentReservations = all.filter(r =>
        !r.isCancelled && new Date(r.movieScreening.startTime) >= now
      );
      this.pastReservations = all.filter(r =>
        r.isCancelled || new Date(r.movieScreening.startTime) < now
      );
    });
  }

  cancel(reservation: Reservation): void {
    this.reservationService.cancel(reservation.id).subscribe(() => {
      reservation.isCancelled = true;
      // Premesti iz current u past
      this.currentReservations = this.currentReservations
        .filter(r => r.id !== reservation.id);
      this.pastReservations = [reservation, ...this.pastReservations];
    });
  }

  rate(reservation: Reservation, stars: number): void {
    this.ratingService.rate(reservation.movieScreening.movieId, stars)
      .subscribe({
        next: () => console.log('Ocena sačuvana'),
        error: (err) => console.error('Greška:', err)
      });
}

  isPast(r: Reservation): boolean {
    return new Date(r.movieScreening.startTime) < new Date();
  }

  setTab(tab: 'current' | 'past'): void {
    this.activeTab = tab;
  }
}