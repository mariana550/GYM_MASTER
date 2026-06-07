import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UsuarioService } from '../../../services/usuario.service';
import { Usuario } from '../../../models/index';

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './usuarios.html',
  styleUrl: './usuarios.scss'
})
export class Usuarios implements OnInit {
  usuarios: Usuario[] = [];
  usuarioEditando: Usuario | null = null;
  cargando = false;
  mensaje = '';
  error = '';

  constructor(
    private usuarioService: UsuarioService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.cargar();
  }

  cargar() {
    this.usuarioService.getAll().subscribe({
      next: (data) => {
        this.usuarios = [...data];
        this.cargando = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Error cargando usuarios';
        this.cargando = false;
        this.cdr.detectChanges();
      }
    });
  }

  editar(usuario: Usuario) {
    this.usuarioEditando = { ...usuario };
  }

  guardar() {
    if (!this.usuarioEditando) return;
    this.usuarioService.editar(this.usuarioEditando.id, {
      nombre: this.usuarioEditando.nombre,
      email: this.usuarioEditando.email,
      rol: this.usuarioEditando.rol,
      activo: this.usuarioEditando.activo
    }).subscribe({
      next: () => {
        this.mensaje = 'Usuario actualizado correctamente';
        this.usuarioEditando = null;
        this.cargar();
        setTimeout(() => this.mensaje = '', 3000);
      },
      error: () => this.error = 'Error actualizando usuario'
    });
  }

  eliminar(id: number) {
    if (!confirm('¿Seguro que deseas desactivar este usuario?')) return;
    this.usuarioService.eliminar(id).subscribe({
      next: () => {
        this.mensaje = 'Usuario desactivado';
        this.cargar();
        setTimeout(() => this.mensaje = '', 3000);
      },
      error: () => this.error = 'Error eliminando usuario'
    });
  }

  cancelar() {
    this.usuarioEditando = null;
  }
}