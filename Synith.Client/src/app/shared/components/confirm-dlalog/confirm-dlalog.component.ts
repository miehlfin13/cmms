import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';
import { ConfirmDialogService } from '@synith/shared/services';

@Component({
  selector: 'app-confirm-dlalog',
  standalone: true,
  imports: [
    TranslateModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './confirm-dlalog.component.html'
})
export class ConfirmDialogComponent {
  public readonly dialog = inject(ConfirmDialogService);
}
