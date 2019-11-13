import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { AuthRegisterInputModel } from '../../models/auth/auth-register-input-model';
import { Subscription, timer } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { AuthLoginInputModel } from '../../models/auth/auth-login-input-model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy {

  registerForm: FormGroup;
  subscriptions: Subscription[] = [];
  inProgress = false;

  constructor(private _authService: AuthService,
    private _router: Router) { }

  ngOnInit() {
    this.registerForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required, this.matchPasswordValidator]),
      repeatPassword: new FormControl('', [Validators.required, this.matchPasswordValidator]),
    });

    this.subscriptions.push(
      this.registerForm.controls.password.valueChanges
        .subscribe(() => this.passwordValidatorsRefresh()),
      this.registerForm.controls.repeatPassword.valueChanges
        .subscribe(() => this.passwordValidatorsRefresh())
    )
  }

  ngOnDestroy() {
    this.subscriptions.forEach(z => z.unsubscribe());
  }

  private passwordValidatorsRefresh(): void {
    timer(0)
      .subscribe(() => {
        this.registerForm.controls.password.updateValueAndValidity({ emitEvent: false });
        this.registerForm.controls.repeatPassword.updateValueAndValidity({ emitEvent: false });
      });
  }

  private matchPasswordValidator = (control: AbstractControl) => {
    if (this.registerForm === undefined) {
      return undefined;
    }

    const formValue = this.registerForm.value;

    if (formValue.password != formValue.repeatPassword) {
      return { match: true };
    }
    return undefined;
  }

  hasError = (controlName: string, errorName: string) => {
    return this.registerForm.controls[controlName].hasError(errorName);
  }

  createUser = async () => {
    const formValue = this.registerForm.value;

    const registerInfo: AuthRegisterInputModel = {
      username: formValue.username,
      password: formValue.password,
    };

    this.inProgress = true;
    try {
      await this._authService.register(registerInfo).toPromise();

      const login: AuthLoginInputModel = {
        password: registerInfo.password,
        username: registerInfo.username,
      };

      await this._authService.login(login).toPromise();
      await this._router.navigate(['/my-dashboard']);

    } catch (error) {
      alert(error.error.message);
    } finally {
      this.inProgress = false;
    }
  }
}
