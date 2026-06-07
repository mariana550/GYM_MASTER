import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-chatbot',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chatbot.html',
  styleUrl: './chatbot.scss'
})
export class Chatbot {
  abierto = false;
  mensaje = '';
  cargando = false;

  mensajes: { rol: 'bot' | 'user'; texto: string }[] = [
    { rol: 'bot', texto: '¡Hola! 👋 Soy el asistente de GymMaster. ¿En qué puedo ayudarte?' }
  ];

  constructor(private http: HttpClient) {}

  toggle() {
  this.abierto = !this.abierto;
  if (!this.abierto) {
    this.mensajes = [
      { rol: 'bot', texto: '¡Hola! 👋 Soy el asistente de GymMaster. ¿En qué puedo ayudarte?' }
    ];
    this.mensaje = '';
  }
}

  enviar() {
    if (!this.mensaje.trim() || this.cargando) return;

    const textoUsuario = this.mensaje.trim();
    this.mensajes.push({ rol: 'user', texto: textoUsuario });
    this.mensaje = '';
    this.cargando = true;

    this.http.post<{ respuesta: string }>(
      `${environment.apiUrl}/Chatbot`,
      { mensaje: textoUsuario }
    ).subscribe({
      next: (res) => {
        this.mensajes.push({ rol: 'bot', texto: res.respuesta });
        this.cargando = false;
      },
      error: () => {
        this.mensajes.push({ rol: 'bot', texto: 'Lo siento, ocurrió un error. Intenta de nuevo.' });
        this.cargando = false;
      }
    });
  }

  onEnter(event: KeyboardEvent) {
    if (event.key === 'Enter') this.enviar();
  }
}