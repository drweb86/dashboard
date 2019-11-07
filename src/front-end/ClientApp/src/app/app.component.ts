import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, OnDestroy {
  private readonly _logoutName = 'Log Out';
  private readonly _dashboardTitle = 'Dashboard';
  private _subscriptions: Subscription[] = [];

  menuItems: { title: string, url?: string, action?: () => {}, visible: boolean }[] = [
    {
      title: 'Home',
      url: '/',
      visible: true,
    },
    {
      title: this._dashboardTitle,
      url: '/my-dashboard',
      visible: true,
    },
    {
      title: this._logoutName,
      action: () => this.logout(),
      visible: false,
    }
  ];

  constructor(
    private _authService: AuthService,
    private _router: Router) {

  }

  private async logout(): Promise<void> {
    this._authService.logout();
    await this._router.navigateByUrl('/');
  }

  ngOnInit(): void {
    this._subscriptions.push(this._authService.isAuthorised$.subscribe(isAuthorised => {
      this.menuItems.find(z => z.title == this._logoutName).visible = isAuthorised;
      this.menuItems.find(z => z.title == this._dashboardTitle).visible = isAuthorised;
    }));

    // not waiting.
    this._authService.isAuthenticated().toPromise();
  }

  ngOnDestroy(): void {
    this._subscriptions.forEach(z => z.unsubscribe());
  }
}
