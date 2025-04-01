import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../admin/admin.service';
import { CardConfiguration } from "../../common/centered-card/centered-card.component";
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class AdminProfileComponent implements OnInit {
  private adminService = inject(AdminService);
  private toastrService = inject(ToastrService);

  CardConfiguration = CardConfiguration;

  admin = signal<{ id: number; email: string } | null>(null);
  errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.fetchProfile();
  }

  fetchProfile(): void {
    this.adminService.fetchLoggedInAdmin().subscribe({
      next: (response) => {
        if (response.success) {
          this.admin.set(response.palyload ?? null);
        } else {
          this.toastrService.error(response.error!.detail, response.error!.title);
        }
      }
    });
  }
}