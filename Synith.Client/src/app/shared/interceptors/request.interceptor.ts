import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TokenService } from '@synith/modules/user-account/services';

export const requestInterceptor: HttpInterceptorFn = (req, next) => {
  const token = inject(TokenService);
  
  return next(req.clone({
    setHeaders: {
      Authorization: `Bearer ${token.getAccessToken()}`
    }
  }));
};
