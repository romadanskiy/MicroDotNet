import { Injectable } from '@angular/core';
import { Developer } from '../models/developer';
import {
  BehaviorSubject,
  distinctUntilChanged,
  map,
  Observable,
  ReplaySubject,
} from 'rxjs';
import { JwtService } from './jwt.service';
import { HttpClient } from '@angular/common/http';
import { ApiService } from './api.service';
import { LoginCredentials } from '../models/login-credentials';
import { RegisterDto } from '../models/register-dto';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private currentUserSubject = new BehaviorSubject<Developer>({} as Developer);
  public currentUser = this.currentUserSubject
    .asObservable()
    .pipe(distinctUntilChanged());

  private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
  public isAuthenticated = this.isAuthenticatedSubject.asObservable();

  constructor(
    private apiService: ApiService,
    private http: HttpClient,
    private jwtService: JwtService,
  ) {}

  populate() {
    if (this.jwtService.getToken()) {
      this.apiService.get('/users').subscribe(
        (data) => this.setAuth(data, this.jwtService.getToken()!),
        (err) => this.purgeAuth(),
      );
    } else {
      this.purgeAuth();
    }
  }

  setAuth(user: Developer, token: string) {
    this.jwtService.saveToken(token);
    this.currentUserSubject.next(user);
    this.isAuthenticatedSubject.next(true);
  }

  purgeAuth() {
    this.jwtService.destroyToken();
    this.currentUserSubject.next({} as Developer);
    this.isAuthenticatedSubject.next(false);
  }

  attemptAuth(credentials: LoginCredentials): Observable<Developer> {
    return this.apiService.post('/users/login', credentials).pipe(
      map((data) => {
        this.setAuth(data.developer, data.token);
        return data;
      }),
    );
  }

  attemptRegister(credentials: RegisterDto): Observable<Developer> {
    return this.apiService.post('/users/register', credentials).pipe(
      map((data) => {
        this.setAuth(data.developer, data.token);
        return data;
      }),
    );
  }

  getCurrentUser(): Developer {
    return this.currentUserSubject.value;
  }
}
