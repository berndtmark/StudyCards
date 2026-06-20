import { inject, Service } from '@angular/core';
import { DiagnosticsService } from '../../@api/services/diagnostics.service';
import { ClientLogRequest } from '../../@api/models/client-log-request';

@Service()
export class LoggerService {
  private diagnosticsService = inject(DiagnosticsService);

  error(message: string, error?: unknown, source?: string) {
    console.error(message, error);

    let errorMessage = '';
    let stackTrace: string | undefined = undefined;

    if (error instanceof Error) {
      errorMessage = ` - ${error.message}`;
      stackTrace = error.stack;
    } else if (error) {
      errorMessage = ` - ${JSON.stringify(error)}`;
    }

    const logRequest: ClientLogRequest = {
      message: message + errorMessage,
      stackTrace: stackTrace,
      source: source,
      path: window.location.pathname
    };

    // Fire and forget log request
    this.diagnosticsService.logError({ body: logRequest }).subscribe({
      error: (e) => console.error('Failed to send remote log', e)
    });
  }

  info(message: string, data?: any) {
    console.log(message, data);
  }

  warn(message: string, data?: any) {
    console.warn(message, data);
  }
}
