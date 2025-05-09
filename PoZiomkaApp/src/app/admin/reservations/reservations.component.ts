import { Component, OnInit, NgModule} from '@angular/core';
import { ReservationModel, ReservationStudentModel } from '../../reservation/reservation.model';
import { ReservationService } from '../../reservation/reservation.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reservations',
  imports: [CommonModule],
  templateUrl: './reservations.component.html',
  styleUrl: './reservations.component.css'
})
export class ReservationsComponent implements OnInit {
  reservations: ReservationStudentModel[] = [];
  isLoading = true;

  constructor(private reservationService: ReservationService) {}

  ngOnInit(): void {
    this.loadReservationsWithDetails();
  }

  loadReservationsWithDetails() {
    this.isLoading = true;
    this.reservationService.getReservations().subscribe(response => {
      const reservationList = response.payload ?? [];

      // pobieramy dane szczegółowe dla każdej rezerwacji
      const detailRequests = reservationList.map(r =>
        this.reservationService.getReservation(r.id)
      );

      Promise.all(detailRequests.map(obs => obs.toPromise()))
        .then(responses => {
          this.reservations = responses.map(res => res?.payload!).filter(Boolean);
          this.isLoading = false;
        })
        .catch(() => this.isLoading = false);
    });
  }

  toggleAccept(reservationId: number, currentStatus: boolean) {
    const newStatus = !currentStatus;
    this.reservationService.updateReservation(reservationId, newStatus).subscribe(() => {
      const reservation = this.reservations.find(r => r.id === reservationId);
      if (reservation) reservation.isAcceptedByAdmin = newStatus;
    });
  }
}