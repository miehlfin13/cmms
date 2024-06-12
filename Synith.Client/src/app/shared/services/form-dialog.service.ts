import { Injectable, inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { NotificationService } from '@synith/shared/services';
import { FormDialogComponent } from '@synith/shared/components';

@Injectable({
  providedIn: 'root'
})
export class FormDialogService {

  private readonly dialog = inject(MatDialog);
  private readonly translate = inject(TranslateService);
  private readonly notification = inject(NotificationService);

  form: FormGroup|null = null;
  saveAction: () => void;

  constructor() {
    this.saveAction = () => {
      this.translate.get('ERROR.NOTIMPLEMENTED')
        .subscribe(text => this.notification.error(text))
    };
  }

  show(form: FormGroup, save: () => void) {
    this.form = form;
    this.saveAction = save;

    const config = new MatDialogConfig();
    config.disableClose = true;
    this.dialog.open(FormDialogComponent, config);
  }

  save() {
    this.saveAction();
  }

  close() {
    this.form?.reset();
    this.dialog.closeAll();
  }
}
