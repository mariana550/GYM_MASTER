import { inject } from '@angular/core/primitives/di';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ActivatedRouteSnapshot } from '@angular/router';

export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const rolesPermitidos: string[] = route.data['roles'] ?? [];
  const rolUsuario = authService.getRol();
  if (rolesPermitidos.includes(rolUsuario)) return true;
  router.navigate(['/acceso-denegado']);
  return false;
};