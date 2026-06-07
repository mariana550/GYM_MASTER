import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RutinaService } from '../../../services/rutina.service';
import { UsuarioService } from '../../../services/usuario.service';
import { Rutina, CrearRutinaDTO, Usuario } from '../../../models/index';

@Component({
  selector: 'app-rutinas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './rutinas.html',
  styleUrl: './rutinas.scss'
})
export class Rutinas implements OnInit {
  rutinas: Rutina[] = [];
  clientes: Usuario[] = [];
  cargando = false;
  mensaje = '';
  error = '';

  mostrarFormulario = false;
  mostrarAsignar = false;
  editandoId: number | null = null;

  form: CrearRutinaDTO = { nombre: '', descripcion: '', objetivo: 'Fuerza', duracionSemanas: 4 };
  asignarForm = { usuarioId: 0, rutinaId: 0 };

  constructor(
    private rutinaService: RutinaService,
    private usuarioService: UsuarioService
  ) {}

  ngOnInit() {
    this.cargar();
    this.usuarioService.getClientes().subscribe(c => this.clientes = c);
  }

  cargar() {
    this.cargando = true;
    this.rutinaService.getAll().subscribe({
      next: (data) => { this.rutinas = data; this.cargando = false; },
      error: () => { this.error = 'Error cargando rutinas'; this.cargando = false; }
    });
  }

  guardar() {
    if (this.editandoId) {
      this.rutinaService.editar(this.editandoId, this.form).subscribe({
        next: () => {
          this.mensaje = 'Rutina actualizada';
          this.resetForm();
          this.cargar();
          setTimeout(() => this.mensaje = '', 3000);
        },
        error: () => this.error = 'Error actualizando rutina'
      });
    } else {
      this.rutinaService.crear(this.form).subscribe({
        next: () => {
          this.mensaje = 'Rutina creada';
          this.resetForm();
          this.cargar();
          setTimeout(() => this.mensaje = '', 3000);
        },
        error: () => this.error = 'Error creando rutina'
      });
    }
  }

  editar(r: Rutina) {
    this.editandoId = r.id;
    this.form = { nombre: r.nombre, descripcion: r.descripcion, objetivo: r.objetivo, duracionSemanas: r.duracionSemanas };
    this.mostrarFormulario = true;
  }

  eliminar(id: number) {
    if (!confirm('¿Eliminar esta rutina?')) return;
    this.rutinaService.eliminar(id).subscribe({
      next: () => {
        this.mensaje = 'Rutina eliminada';
        this.cargar();
        setTimeout(() => this.mensaje = '', 3000);
      },
      error: () => this.error = 'Error eliminando rutina'
    });
  }

  asignar() {
    this.rutinaService.asignar(this.asignarForm).subscribe({
      next: () => {
        this.mensaje = 'Rutina asignada correctamente';
        this.mostrarAsignar = false;
        setTimeout(() => this.mensaje = '', 3000);
      },
      error: () => this.error = 'Error asignando rutina'
    });
  }

  resetForm() {
    this.form = { nombre: '', descripcion: '', objetivo: 'Fuerza', duracionSemanas: 4 };
    this.editandoId = null;
    this.mostrarFormulario = false;
  }
}