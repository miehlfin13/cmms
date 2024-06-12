import { Injectable, inject } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@synith/shared/components';
import { NotificationService } from './notification.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class ConfirmDialogService {

  private readonly dialog = inject(MatDialog);
  private readonly translate = inject(TranslateService);
  private readonly notification = inject(NotificationService);

  message = '';
  confirmAction: () => void;

  constructor() {
    this.confirmAction = () => {
      this.translate.get('ERROR.NOTIMPLEMENTED')
        .subscribe(text => this.notification.error(text))
    };
  }

  show(message: string, confirm: () => void) {
    this.message = message;
    this.confirmAction = confirm;

    const config = new MatDialogConfig();
    config.disableClose = true;
    this.dialog.open(ConfirmDialogComponent, config);
  }

  confirm() {
    this.close();
    this.confirmAction();
  }

  close() {
    this.dialog.closeAll();
  }
}
