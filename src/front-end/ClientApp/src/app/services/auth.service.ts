import { Injectable, Inject } from "@angular/core";
import { AuthRegisterInputModel } from '../models/auth/auth-register-input-model';
import { Observable, empty, of } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { switchMap } from 'rxjs/operators';
import { AuthLoginInputModel } from '../models/auth/auth-login-input-model';
import { AuthResultModel } from '../models/auth/auth-result-model';

@Injectable()
export class AuthService {
    private readonly _token = "token";

    constructor(private _http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) {

    }

    register(info: AuthRegisterInputModel): Observable<void> {
        return this._http.post<void>(`${this._baseUrl}auth/register`, info);
    }

    login(info: AuthLoginInputModel): Observable<void> {
        return this._http
            .post<AuthResultModel>(`${this._baseUrl}auth/login`, info)
            .pipe(
                switchMap(login => {
                    window.localStorage.setItem(this._token, login.token);
                    return of(undefined);
                }),
            );
    }

    logout(): void {
        window.localStorage.removeItem(this._token);
    }
}