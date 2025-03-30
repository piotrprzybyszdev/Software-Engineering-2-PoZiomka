import { CommonModule } from '@angular/common';
import { Component, computed, input } from '@angular/core';

export enum CardConfiguration {
  PrimaryLeft, PrimaryRight, NoSecondary
}

@Component({
  selector: 'app-centered-card',
  imports: [CommonModule],
  templateUrl: './centered-card.component.html',
  styleUrl: './centered-card.component.css'
})
export class CenteredCardComponent {
  configuration = input.required<CardConfiguration>();
  backgroundColor = input.required<string>();

  CardConfiguration = CardConfiguration;

  primaryClass = computed(() => {
    switch (this.configuration()) {
      case CardConfiguration.PrimaryLeft:
        return "order-1 col-xl-6";
      case CardConfiguration.PrimaryRight:
        return "order-2 col-xl-6";
      case CardConfiguration.NoSecondary:
        return "";
    }
  });

  secondaryClass = computed(() => {
    switch (this.configuration()) {
      case CardConfiguration.PrimaryLeft:
        return "order-2 d-xl-block";
      case CardConfiguration.PrimaryRight:
        return "order-1 d-xl-block";
      case CardConfiguration.NoSecondary:
        return "d-none";
    }
  });
}
