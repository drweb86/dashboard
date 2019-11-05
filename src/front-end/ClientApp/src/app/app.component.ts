import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  private readonly logoutName = 'Log Out';

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
    // await this._router.navigate(['/']);
    document.location.href = "/"; // force reload components
  }

  async ngOnInit(): Promise<void> {
    if (await this._authService.isAuthenticated().toPromise()) {
      this.menuItems.find(z => z.title == this.logoutName).visible = true;
    }
  }
}
