import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { EMPTY, catchError } from 'rxjs';
import { inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NotificationService } from '@synith/shared/services';
import { ResponseMessage } from '@synith/shared/dto';
import { Router } from '@angular/router';

export const responseInterceptor: HttpInterceptorFn = (req, next) => {
  const notification = inject(NotificationService);
  const translate = inject(TranslateService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((response: HttpErrorResponse) => {
      if (response.status == 401) {
        router.navigate(['']);
        notification.error(translate.instant('ERROR.TOKENEXPIRED'));
        return EMPTY;
      }

      try {
        const message = JSON.parse(response.error) as ResponseMessage;
        const code = `RESPONSE.${message.Code.toUpperCase()}`;
        notification.error(
          translate.instant(code, message.ParametersJson == '' ? undefined : JSON.parse(message.ParametersJson))
        );
      } catch {
        notification.error(translate.instant('ERROR.UNEXPECTEDERROR'));
      }
      return EMPTY;
    })
  );
};
