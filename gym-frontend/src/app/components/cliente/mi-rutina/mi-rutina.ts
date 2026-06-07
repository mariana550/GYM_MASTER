import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RutinaService } from '../../../services/rutina.service';
import { Rutina } from '../../../models/index';

@Component({
  selector: 'app-mi-rutina',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './mi-rutina.html',
  styleUrl: './mi-rutina.scss'
})
export class MiRutina implements OnInit {
  rutina: Rutina | null = null;
  cargando = true;
  error = '';

  constructor(
    private rutinaService: RutinaService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() { this.cargar(); }

  cargar() {
    this.rutinaService.getMiRutina().subscribe({
      next: (data) => {
        this.rutina = data;
        this.cargando = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.rutina = null;
        this.cargando = false;
        this.cdr.detectChanges();
      }
    });
  }
}