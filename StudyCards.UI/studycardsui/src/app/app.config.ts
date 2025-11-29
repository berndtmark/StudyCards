import { ApplicationConfig, ErrorHandler, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

import { routes } from './app.routes';
import { environment } from '../environments/environment';
import { ApiConfiguration } from './@api/api-configuration';
import { SIGNALR_URL } from './shared/services/hub.service';
import { GlobalErrorHandler } from './core/global-error-handler';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient(),
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
  ],
};
