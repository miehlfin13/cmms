import { Injectable, inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private readonly toastr = inject(ToastrService);

  info = (message: string) => this.toastr.info(message, '', { timeOut: 10000, closeButton: true });
  success = (message: string) => this.toastr.success(message, '', { timeOut: 5000, closeButton: true });
  warning = (message: string) => this.toastr.warning(message, '', { timeOut: 10000, closeButton: true });
  error = (message: string, clear = true) => {
    if (clear) {
      this.clear();
    }
    this.toastr.error(message, '', { timeOut: 0, extendedTimeOut: 0, closeButton: true });
  }
  clear = () => this.toastr.clear();
}
