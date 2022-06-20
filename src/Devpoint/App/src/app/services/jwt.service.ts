import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class JwtService {
  constructor(private localStorageService: LocalStorageService) {}

  getToken(): string | null {
    return this.localStorageService.getData('jwt');
  }

  saveToken(token: string) {
    this.localStorageService.setData('jwt', token);
  }

  destroyToken() {
    this.localStorageService.removeData('jwt');
  }
}
