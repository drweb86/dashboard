import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, OnDestroy {
  private readonly logoutName = 'Log Out';
  private _subscriptions: Subscription[] = [];

  menuItems: { title: string, url?: string, action?: () => {}, visible: boolean }[] = [
    {
      title: 'Home',
      url: '/',
      visible: true,
    },
    {
      title: 'Dashboard',
      url: '/my-dashboard',
      visible: true,
    },
    {
      title: this.logoutName,
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

  async ngOnInit(): Promise<void> {
    this._subscriptions.push(this._authService.isAuthorised$.subscribe(isAuthorised => this.menuItems.find(z => z.title == this.logoutName).visible = isAuthorised));
    await this._authService.isAuthenticated().toPromise();
  }

  ngOnDestroy(): void {
    this._subscriptions.forEach(z => z.unsubscribe());
  }
}
