import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../admin/admin.service';
import { Router } from '@angular/router';
import { CardConfiguration, CenteredCardComponent } from "../../common/centered-card/centered-card.component";

@Component({
  selector: 'app-admin-profile',
  standalone: true,
  imports: [CommonModule, CenteredCardComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class AdminProfileComponent implements OnInit {
  private adminService = inject(AdminService);
  private router = inject(Router);

  CardConfiguration = CardConfiguration;

  admin = signal<{ id: number; email: string } | null>(null);
  errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.fetchProfile();
  }

  fetchProfile(): void {
    this.adminService.fetchLoggedInAdmin().subscribe({
      next: (response) => {
        this.admin.set(response.palyload ?? null);
      },
      error: () => {
        this.errorMessage.set('Błąd pobierania profilu administratora');
      }
    });
  }

  goToDashboard(): void {
    this.router.navigate(['/admin/dashboard']);
  }

  logout(): void {
    //this.adminService.logout(); 
    this.router.navigate(['/admin/login']);
  }
}