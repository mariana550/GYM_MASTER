import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {
  nombre = '';
  email = '';
  password = '';
  rol = 'Cliente';
  error = '';
  cargando = false;

  constructor(private authService: AuthService, private router: Router) {}

  register() {
    this.error = '';
    this.cargando = true;
    this.authService.register({
      nombre: this.nombre,
      email: this.email,
      password: this.password,
      rol:  this.rol
    }).subscribe({
      next: () => {
        this.cargando = false;
        this.router.navigate(['/cliente/dashboard']);
      },
      error: (err) => {
        this.cargando = false;
        this.error = err.error?.mensaje ?? 'Error al registrarse';
      }
    });
  }
}
