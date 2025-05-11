import { Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';

export type Tile = {
  title: string,
  description: string,
  icon: string,
  link: string
}

@Component({
  selector: 'app-tile-list',
  imports: [],
  templateUrl: './tile-list.component.html',
  styleUrl: './tile-list.component.css'
})
export class TileListComponent {
  tiles = input.required<Tile[]>();

  private router = inject(Router);

  onClick(link: string): void {
    this.router.navigate([link]);
  }
}
