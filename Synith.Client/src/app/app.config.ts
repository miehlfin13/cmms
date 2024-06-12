import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { HttpClient, provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';

import { ApiModule, Configuration, ConfigurationParameters } from '@synith/openapi';
import { routes } from '@synith/app.routes';
import { requestInterceptor, responseInterceptor } from '@synith/shared/interceptors';
import { environment } from '@synith/../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    importProvidersFrom(ApiModule.forRoot(() => {
      const configParam: ConfigurationParameters = {
        basePath: environment.apiBaseUrl
      }
      return new Configuration(configParam);
    })),
    provideHttpClient(
      withFetch(),
      withInterceptors([
        requestInterceptor,
        responseInterceptor
      ])),
    provideAnimations(),
    provideToastr(),
    importProvidersFrom(TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }))
  ]
};

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
