import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProgresoService } from '../../../services/progreso.service';
import { Progreso, CrearProgresoDTO } from '../../../models/index';

@Component({
  selector: 'app-mi-progreso',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './mi-progreso.html',
  styleUrl: './mi-progreso.scss'
})
export class MiProgreso implements OnInit {
  progresos: Progreso[] = [];
  cargando = false;
  mensaje = '';
  error = '';
  mostrarFormulario = false;

  form: CrearProgresoDTO = { peso: 0, altura: 0, porcentajeGrasa: 0, notas: '' };

  constructor(private progresoService: ProgresoService, private cdr: ChangeDetectorRef) {}

  ngOnInit() { this.cargar(); }

  cargar() {
    this.progresoService.getMiProgreso().subscribe({
      next: (data) => {
        this.progresos = [...data];
        this.cargando = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Error cargando progreso';
        this.cargando = false;
      }
    });
  }

  registrar() {
    this.progresoService.registrar(this.form).subscribe({
      next: () => {
        this.mensaje = 'Progreso registrado correctamente';
        this.mostrarFormulario = false;
        this.form = { peso: 0, altura: 0, porcentajeGrasa: 0, notas: '' };
        this.cargar();
        setTimeout(() => this.mensaje = '', 3000);
      },
      error: () => this.error = 'Error registrando progreso'
    });
  }

  eliminar(id: number) {
    if (!confirm('¿Eliminar este registro?')) return;
    this.progresoService.eliminar(id).subscribe({
      next: () => {
        this.mensaje = 'Registro eliminado';
        this.cargar();
        setTimeout(() => this.mensaje = '', 3000);
      },
      error: () => this.error = 'Error eliminando registro'
    });
  }
}
