import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class Navbar {
  constructor(public authService: AuthService, private router: Router) {}

  logout() {
    this.authService.logout();
  }

  get rol() {
    return this.authService.getRol();
  }

  get nombre() {
    return this.authService.getUsuario()?.nombre ?? '';
  }

  get loggedIn() {
    return this.authService.isLoggedIn();
  }
}
