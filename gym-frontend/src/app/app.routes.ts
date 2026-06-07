    import { Routes } from '@angular/router';
    import { authGuard } from './guards/auth-guard';
    import { roleGuard } from './guards/role-guard';
    import { Login } from './components/login/login';
    import { Register } from './components/register/register';
    import { AccesoDenegado } from './components/acceso-denegado/acceso-denegado';
    import { Dashboard as AdminDashboard } from './components/admin/dashboard/dashboard';
    import { Usuarios } from './components/admin/usuarios/usuarios';
    import { Dashboard as EntrenadorDashboard } from './components/entrenador/dashboard/dashboard';
    import { Rutinas } from './components/entrenador/rutinas/rutinas';
    import { Dashboard as ClienteDashboard } from './components/cliente/dashboard/dashboard';
    import { MiRutina } from './components/cliente/mi-rutina/mi-rutina';
    import { MiProgreso } from './components/cliente/mi-progreso/mi-progreso';
    import { Home } from './components/home/home';

    export const routes: Routes = [
    { path: '', component: Home },
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: 'acceso-denegado', component: AccesoDenegado },
    {
        path: 'admin/dashboard',
        component: AdminDashboard,
        canActivate: [authGuard, roleGuard],
        data: { roles: ['Admin'] }
    },
    {
        path: 'admin/usuarios',
        component: Usuarios,
        canActivate: [authGuard, roleGuard],
        data: { roles: ['Admin'] }
    },
    {
        path: 'entrenador/dashboard',
        component: EntrenadorDashboard,
        canActivate: [authGuard, roleGuard],
        data: { roles: ['Entrenador'] }
    },
    {
        path: 'entrenador/rutinas',
        component: Rutinas,
        canActivate: [authGuard, roleGuard],
        data: { roles: ['Entrenador'] }
    },
    {
        path: 'cliente/dashboard',
        component: ClienteDashboard,
        canActivate: [authGuard, roleGuard],
        data: { roles: ['Cliente'] }
    },
    {
        path: 'cliente/mi-rutina',
        component: MiRutina,
        canActivate: [authGuard, roleGuard],
        data: { roles: ['Cliente'] }
    },
    {
        path: 'cliente/mi-progreso',
        component: MiProgreso,
        canActivate: [authGuard, roleGuard],
        data: { roles: ['Cliente'] }
    },
    { path: '**', redirectTo: 'login' }
    ];