import { ErrorHandler, inject } from "@angular/core";
import { DialogService } from "../shared/services/dialog.service";
import { LoggerService } from "../shared/services/logger.service";

export class GlobalErrorHandler implements ErrorHandler {
  private readonly dialog = inject(DialogService);
  private readonly logger = inject(LoggerService);

  handleError(error: any) {
    this.dialog.info('Error Occured', `An unhandled exception has occured`);
    this.logger.error('Unhandled Exception', error, GlobalErrorHandler.name);
  }
}