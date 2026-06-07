import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Rutina, CrearRutinaDTO, AsignarRutinaDTO } from '../models/index';

@Injectable({ providedIn: 'root' })
export class RutinaService {

  private apiUrl = `${environment.apiUrl}/rutinas`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Rutina[]> {
    return this.http.get<Rutina[]>(this.apiUrl);
  }

  getById(id: number): Observable<Rutina> {
    return this.http.get<Rutina>(`${this.apiUrl}/${id}`);
  }

  crear(dto: CrearRutinaDTO): Observable<Rutina> {
    return this.http.post<Rutina>(this.apiUrl, dto);
  }

  editar(id: number, dto: Partial<CrearRutinaDTO>): Observable<Rutina> {
    return this.http.put<Rutina>(`${this.apiUrl}/${id}`, dto);
  }

  eliminar(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  asignar(dto: AsignarRutinaDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/asignar`, dto);
  }
  getMiRutina(): Observable<Rutina> {
  return this.http.get<Rutina>(`${this.apiUrl}/mi-rutina`);
}
}