import { Component, inject } from '@angular/core';
import { MatIcon} from "@angular/material/icon"
import { MatButton } from '@angular/material/button';
import { MatBadge} from '@angular/material/badge';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { BusyService } from '../../core/services/busy.service';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatSpinner } from '@angular/material/progress-spinner';
import { CartService } from '../../core/services/cart.service';
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatIcon,
    MatButton,
    MatBadge,
    RouterLink,
    RouterLinkActive,
    MatProgressBar
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  busyServices = inject(BusyService);
  cartService = inject(CartService);
}
