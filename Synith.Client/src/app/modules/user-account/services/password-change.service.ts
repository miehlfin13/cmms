import { Injectable, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { FormDialogService, NotificationService } from '@synith/shared/services';
import { UserService } from '@synith/openapi';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordChangeService {

  private readonly dialog = inject(FormDialogService);
  private readonly notification = inject(NotificationService);
  private readonly translate = inject(TranslateService);
  private readonly userApi = inject(UserService);
  private readonly token = inject(TokenService);

  form = new FormGroup({
    currentPassword: new FormControl(),
    newPassword: new FormControl(),
    confirmPassword: new FormControl()
  });

  show() {
    this.dialog.show(this.form, () => {

      if (this.form.controls.currentPassword.value.length === 0 || this.form.controls.newPassword.value.length === 0) {
        return;
      }

      if (this.form.controls.currentPassword.value == this.form.controls.newPassword.value) {
        this.translate.get('VALIDATION.PASSWORDMUSTNOTMATCH')
          .subscribe(text => this.notification.error(text));
        return;
      }

      if (this.form.controls.newPassword.value != this.form.controls.confirmPassword.value) {
        this.translate.get('VALIDATION.NEWPASSWORDNOTMATCH')
          .subscribe(text => this.notification.error(text));
        return;
      }

      this.userApi.updatePassword({
        userId: this.token.getUserId(),
        currentPassword: this.form.controls.currentPassword.value,
        newPassword: this.form.controls.newPassword.value
      }).subscribe({
        next: () => {
          this.dialog.close();
          this.notification.clear();
          this.translate.get('SUCCESS.PASSWORDCHANGE')
            .subscribe(text => this.notification.success(text));
        }
      });
    });
  }
}
