import { ApplicationConfig, ErrorHandler, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { SIGNALR_URL } from './shared/services/hub.service';
import { environment } from '../environments/environment';
import { GlobalErrorHandler } from './core/global-error-handler';
import { ApiConfiguration } from './@api/api-configuration';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { timezoneInterceptor } from './core/time-zone-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(
      withInterceptors([timezoneInterceptor])
    ),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    {
      provide: ApiConfiguration,
      useFactory: () => {
        const config = new ApiConfiguration();
        config.rootUrl = environment.apiBase; // Set your custom URL here
        return config;
      }
    },
    { provide: SIGNALR_URL, useValue: environment.chatHubUrl },
    { provide: ErrorHandler, useClass: GlobalErrorHandler }
  ]
};
