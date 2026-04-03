import { HttpInterceptorFn } from "@angular/common/http";

export const timezoneInterceptor: HttpInterceptorFn = (req, next) => {
  const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;
  const modifiedReq = req.clone({
    headers: req.headers.set('X-Timezone', userTimeZone)
  });
  return next(modifiedReq);
};