import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit, OnDestroy {
  isAuthenticated?: boolean = undefined;
  private _subscriptions: Subscription[] = [];

  constructor(
    private _router: Router,
    private _authService: AuthService) {
  }

  ngOnInit(): void {
    this._subscriptions.push(this._authService.isAuthorised$.subscribe(isAuthorised => this.isAuthenticated = isAuthorised));

    // not waiting
    this._authService.isAuthenticated().toPromise();
  }

  ngOnDestroy(): void {
    this._subscriptions.forEach(z => z.unsubscribe());
  }

  goToDashboard(): void {
    this._router.navigate(['my-dashboard']);
  }
}
