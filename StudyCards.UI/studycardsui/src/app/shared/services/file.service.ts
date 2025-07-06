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

  downloadJson<T>(data: T, filename = 'data.json') {
    const json = JSON.stringify(data, null, 2); // Pretty-print with 2-space indentation
    const blob = new Blob([json], { type: 'application/json' });
    const url = URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();

    URL.revokeObjectURL(url);
  }
}
