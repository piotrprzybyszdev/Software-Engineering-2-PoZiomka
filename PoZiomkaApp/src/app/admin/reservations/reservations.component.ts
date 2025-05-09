import { Component, OnInit} from '@angular/core';
import { ReservationStudentModel } from '../../reservation/reservation.model';
import { ReservationService } from '../../reservation/reservation.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reservations',
  imports: [CommonModule],
  templateUrl: './reservations.component.html',
  styleUrl: './reservations.component.css'
})
export class ReservationsComponent implements OnInit {
  reservations: ReservationStudentModel[] = [];
  isLoading = true;

  constructor(
    private reservationService: ReservationService,
    private toastrService: ToastrService 
  ) {}

  ngOnInit(): void {
    this.loadReservationsWithDetails();
  }

  loadReservationsWithDetails() {
    this.isLoading = true;
    this.reservationService.getReservations().subscribe(response => {
      const reservationList = response.payload ?? [];

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
    this.reservationService.updateReservation(reservationId, newStatus).subscribe({
      next: response => {
        if (response.success) {
          this.toastrService.success('Pomyślnie zmieniono status rezerwacji');
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
        this.loadReservationsWithDetails();
      },
      error: err => {
        this.toastrService.error('Wystąpił błąd podczas aktualizacji rezerwacji');
      }
    });
  }
}