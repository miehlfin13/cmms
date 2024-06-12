import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from '@synith/app.config';
import { AppComponent } from '@synith/root/app.component';

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
