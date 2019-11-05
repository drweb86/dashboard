import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpEvent
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(
        private _router: Router,
        private _authService: AuthService) { }

    intercept(
        req: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        if (req.headers.get('No-Auth') === 'True') {
            return next.handle(req.clone());
        }

        const token = this._authService.getToken();
        if (token != null) {
            const clonedreq = req.clone({
                headers: req.headers.set(
                    'Authorization',
                    'Bearer ' + token
                )
            });
            return next.handle(clonedreq).pipe(
                tap(
                    succ => { },
                    err => {
                        if (err.status === 401) {
                            this._authService.logout();
                            this._router.navigateByUrl('/');
                        } else if ((err.status = 403)) {
                            this._authService.logout();
                            this._router.navigateByUrl('/');
                        }
                    }
                )
            );
        } else {
            this._router.navigateByUrl('/');
        }
    }
}