import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Progreso, CrearProgresoDTO } from '../models/index';

@Injectable({ providedIn: 'root' })
export class ProgresoService {

  private apiUrl = `${environment.apiUrl}/progreso`;

  constructor(private http: HttpClient) {}

  getMiProgreso(): Observable<Progreso[]> {
    return this.http.get<Progreso[]>(`${this.apiUrl}/mio`);
  }

  getAll(): Observable<Progreso[]> {
    return this.http.get<Progreso[]>(this.apiUrl);
  }

  getByUsuario(id: number): Observable<Progreso[]> {
    return this.http.get<Progreso[]>(`${this.apiUrl}/usuario/${id}`);
  }

  registrar(dto: CrearProgresoDTO): Observable<Progreso> {
    return this.http.post<Progreso>(this.apiUrl, dto);
  }

  eliminar(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
