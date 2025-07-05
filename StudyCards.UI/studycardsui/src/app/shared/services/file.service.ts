import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  async readJsonFile<T>(event: Event): Promise<T> {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) throw new Error('No file selected');

    const text = await file.text();
    return JSON.parse(text) as T;
  }
}
