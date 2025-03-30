import { CommonModule } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  imports: [CommonModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {
  logoLink = input.required<string>();
  logoStyle = input.required<string>();

  router = inject(Router);

  onLogoClick(): void {
    this.router.navigate([this.logoLink()]);
  }
}
