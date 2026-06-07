import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { UsuarioService } from '../../../services/usuario.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit {
  totalUsuarios = 0;
  totalClientes = 0;
  totalEntrenadores = 0;

  constructor(private usuarioService: UsuarioService) {}

  ngOnInit() {
    this.usuarioService.getAll().subscribe(usuarios => {
      this.totalUsuarios = usuarios.length;
      this.totalClientes = usuarios.filter(u => u.rol === 'Cliente').length;
      this.totalEntrenadores = usuarios.filter(u => u.rol === 'Entrenador').length;
    });
  }
}
