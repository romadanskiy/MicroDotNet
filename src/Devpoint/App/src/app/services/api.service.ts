import { Injectable } from '@angular/core';
import { JwtService } from './jwt.service';
import { catchError, Observable, throwError } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  readonly ApiUrl = 'http://localhost:8000/devpoint';

  constructor(private http: HttpClient) {}

  private formatErrors(error: any) {
    return throwError(error.error);
  }

  get(path: string, params: HttpParams = new HttpParams()): Observable<any> {
    return this.http
      .get(`${this.ApiUrl}${path}`, { params })
      .pipe(catchError(this.formatErrors));
  }

  put(path: string, body: Object = {}): Observable<any> {
    return this.http
      .put(`${this.ApiUrl}${path}`, JSON.stringify(body))
      .pipe(catchError(this.formatErrors));
  }

  post(path: string, body: Object = {}): Observable<any> {
    return this.http
      .post(`${this.ApiUrl}${path}`, JSON.stringify(body))
      .pipe(catchError(this.formatErrors));
  }

  postRaw(path: string, body: any): Observable<any> {
    return this.http
      .post(`${this.ApiUrl}${path}`, body)
      .pipe(catchError(this.formatErrors));
  }

  postForm(path: string, body: Record<string, string>): Observable<any> {
    const formData = new FormData();
    for (const [key, value] of Object.entries(body)) formData.set(key, value);
    return this.http
      .post(`${this.ApiUrl}${path}`, formData)
      .pipe(catchError(this.formatErrors));
  }

  delete(path: string): Observable<any> {
    return this.http
      .delete(`${this.ApiUrl}${path}`)
      .pipe(catchError(this.formatErrors));
  }
}
