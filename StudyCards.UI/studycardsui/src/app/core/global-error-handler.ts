import { ErrorHandler, inject } from "@angular/core";
import { DialogService } from "app/shared/services/dialog.service";

export class GlobalErrorHandler implements ErrorHandler {
  private readonly dialog = inject(DialogService);

  handleError(error: any) {
    const errorMessage = error?.message ?? 'unknown';

    this.dialog.info('Error Occured', `An unhandled exception occured: ${errorMessage}`);
    console.error(GlobalErrorHandler.name, { error });
  }
}