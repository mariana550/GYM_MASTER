    export interface AuthResponse {
    token: string;
    nombre: string;
    email: string;
    rol: string;
    usuarioId: number;
    }

    export interface LoginDTO {
    email: string;
    password: string;
    }

    export interface RegisterDTO {
    nombre: string;
    email: string;
    password: string;
    rol: string;
    }

    export interface Usuario {
    id: number;
    nombre: string;
    email: string;
    rol: string;
    fechaRegistro: string;
    activo: boolean;
    }

    export interface EditarUsuarioDTO {
    nombre?: string;
    email?: string;
    rol?: string;
    activo?: boolean;
    }

    export interface EditarPerfilDTO {
    nombre?: string;
    email?: string;
    nuevaPassword?: string;
    }

    export interface Rutina {
    id: number;
    nombre: string;
    descripcion: string;
    objetivo: string;
    duracionSemanas: number;
    fechaCreacion: string;
    nombreEntrenador: string;
    entrenadorId: number;
    }

    export interface CrearRutinaDTO {
    nombre: string;
    descripcion: string;
    objetivo: string;
    duracionSemanas: number;
    }

    export interface AsignarRutinaDTO {
    usuarioId: number;
    rutinaId: number;
    }

    export interface Progreso {
    id: number;
    usuarioId: number;
    nombreUsuario: string;
    fecha: string;
    peso: number;
    altura: number;
    porcentajeGrasa: number;
    imc: number;
    notas: string;
    }

    export interface CrearProgresoDTO {
    peso: number;
    altura: number;
    porcentajeGrasa: number;
    notas: string;
    }