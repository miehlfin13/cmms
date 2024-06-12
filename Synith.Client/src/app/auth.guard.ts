import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { TokenService } from '@synith/modules/user-account/services';
import { NotificationService } from '@synith/shared/services';

export const canActivate = (permissionCode: string = ''): CanActivateFn => () => {
  const token = inject(TokenService);
  const router = inject(Router);
  const translate = inject(TranslateService);
  const notification = inject(NotificationService);

  const isLoggedOn = token.isLoggedOn();
  const isExpired = token.isExpired();

  if (!isLoggedOn || isExpired || (permissionCode != '' && !token.hasPermission(permissionCode))) {
    // find way to use translate service
    let message = '';
    switch (translate.currentLang) {
      default:
        message = 'You have no permission to access the page.';
    }

    if (isExpired) {
      switch (translate.currentLang) {
        default:
          message = 'Token has expired. You will be logged out.';
      }
    }

    notification.error(message);

    if (!isLoggedOn || isExpired) {
      token.clearAccessToken();
      router.navigate(['']);
    }

    return false;
  }

  return true;
};

export const invalidPage: CanActivateFn = () => {
  const translate = inject(TranslateService);
  const notification = inject(NotificationService);

  let message = '';
  switch (translate.currentLang) {
    default:
      message = 'The page you are trying to access is invalid.';
  }
  notification.error(message);

  return true;
}
