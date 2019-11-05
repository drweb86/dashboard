import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { AuthLoginInputModel } from 'src/app/models/auth/auth-login-input-model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  inProgress = false;

  constructor(private _authService: AuthService,
    private _router: Router) { }

  ngOnInit() {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  }

  hasError = (controlName: string, errorName: string) => {
    return this.loginForm.controls[controlName].hasError(errorName);
  }

  login = async () => {
    const formValue = this.loginForm.value;

    this.inProgress = true;
    try {
      const login: AuthLoginInputModel = {
        password: formValue.password,
        username: formValue.username,
      };

      await this._authService.login(login).toPromise();
      await this._router.navigate(['/my-dashboard']);

    } catch (error) {
      this.inProgress = false;
      alert(error.error.message);
    }
  }
}
