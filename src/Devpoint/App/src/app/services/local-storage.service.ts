import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LocalStorageService {
  setJsonData(key: string, data?: any | null) {
    if (data === null || data === undefined) return;
    const jsonData = JSON.stringify(data);
    localStorage.setItem(key, jsonData);
  }

  getJsonData(key: string) {
    const data = localStorage.getItem(key);
    if (data === null || data === undefined) return null;
    return JSON.parse(data);
  }

  setData(key: string, data?: string | null) {
    if (data === null || data === undefined) return;

    localStorage.setItem(key, data);
  }

  getData(key: string) {
    return localStorage.getItem(key);
  }

  removeData(key: string) {
    localStorage.removeItem(key);
  }
}
