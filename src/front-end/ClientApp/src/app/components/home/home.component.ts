import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  isAuthenticated?: boolean = undefined;

  constructor(
    private _router: Router,
    private _authService: AuthService) {
  }

  async ngOnInit(): Promise<void> {
    this.isAuthenticated = await this._authService.isAuthenticated().toPromise();
  }

  goToDashboard(): void {
    this._router.navigate(['my-dashboard']);
  }
}
