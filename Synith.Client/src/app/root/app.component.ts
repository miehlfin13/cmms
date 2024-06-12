import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidenavComponent, ToolbarComponent } from '@synith/shared/components';
import { TokenService } from '../modules/user-account/services';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ToolbarComponent, SidenavComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {

  public readonly token = inject(TokenService);

  title = 'Synith.Client';
}
