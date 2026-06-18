import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LoginService } from '../services/login/login';

export const guardGuard: CanActivateFn = (route, state) => {
  const loginService = inject(LoginService);
  const router = inject(Router);

  if (
    loginService.loggedIn &&
    loginService.validateRoles(route.data['allowedRoles'])
  ) {
    return true;
  }

  return router.createUrlTree(['/login']);
};