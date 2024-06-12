import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { UserService } from '@synith/openapi';
import { TokenService } from '@synith/modules/user-account/services';
import { NotificationService } from '@synith/shared/services';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    TranslateModule,
    ReactiveFormsModule,
    MatGridListModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  private readonly router = inject(Router);
  private readonly userApi = inject(UserService);
  private readonly token = inject(TokenService);
  private readonly notification = inject(NotificationService);

  // to prevent login form to show
  // when user is already logged in
  // and will be redirected
  isRendered = false;

  form = new FormGroup({
    username: new FormControl(),
    password: new FormControl()
  });

  constructor() {
    if (this.token.getUsername() != '') {
      this.router.navigate(['dashboard']);
    } else {
      this.isRendered = true;
    }
  }

  submit() {
    this.userApi.validateLogin({
      username: this.form.controls.username.value,
      password: this.form.controls.password.value
    }).subscribe({
      next: (res) => {
        this.token.setAccessToken(res.access_token);
        this.notification.clear();
        this.router.navigate(['dashboard']);
      }
    });
  }
}
