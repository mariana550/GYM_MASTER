import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  email = '';
  password = '';
  error = '';
  cargando = false;

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.error = '';
    this.cargando = true;
    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: (res) => {
        this.cargando = false;
        if (res.rol === 'Admin') this.router.navigate(['/admin/dashboard']);
        else if (res.rol === 'Entrenador') this.router.navigate(['/entrenador/dashboard']);
        else this.router.navigate(['/cliente/dashboard']);
      },
      error: () => {
        this.cargando = false;
        this.error = 'Credenciales incorrectas. Intenta de nuevo.';
      }
    });
  }
}
