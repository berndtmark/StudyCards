import { computed, signal } from "@angular/core";
import { interval, Subscription } from "rxjs";

export function createAutoEvoke(
  callback: () => void,
  delaySeconds: number
) {
  if (delaySeconds <= 0) throw new Error("Delay must be positive");

  const delayMs = delaySeconds * 1000;
  const remainingMs = signal(delayMs), 
  isRunning = signal(false);
  let timer: ReturnType<typeof setTimeout> | null, 
  sub: Subscription | null, 
  startAt: number;

  const clear = () => {
    timer && clearTimeout(timer);
    sub && sub.unsubscribe();
    timer = sub = null;
    isRunning.set(false);
    remainingMs.set(delayMs);
  };

  const start = () => {
    clear();
    startAt = Date.now();
    isRunning.set(true);
    sub = interval(1000).subscribe(() =>
      remainingMs.set(Math.max(0, delayMs - (Date.now() - startAt)))
    );
    timer = setTimeout(() => { clear(); callback(); }, delayMs);
  };

  const flush = () => { clear(); callback(); };
  const reset = () => { start(); };

  return {
    start,
    flush,
    clear,
    reset,
    isRunning: computed(isRunning),
    remainingSeconds: computed(() => Math.ceil(remainingMs() / 1000)),
  };
}