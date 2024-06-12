import { Component } from '@angular/core';
import {AbstractControl, FormControl, FormGroup, Validators} from "@angular/forms";
import {AccountService} from "../../services/account.service";
import {Router} from "@angular/router";
import {CompareValidator} from "../../validators/custom.validators";
import {RegisterUser} from "../../models/register-user";
import {LoginUser} from "../../models/login-user";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  public loginForm: FormGroup;
  public isFormSubmitted: boolean = false;

  constructor(private accountService: AccountService, private router: Router) {
    this.loginForm = new FormGroup({
        email: new FormControl(null, [Validators.required]),
        password: new FormControl(null, [Validators.required]),
      });
  }

  get login_emailControl(): AbstractControl{
    return this.loginForm.controls["login"];
  }
  get login_passwordControl(): AbstractControl{
    return this.loginForm.controls["password"];
  }
  loginSubmitted(){
    if (this.loginForm.valid){
      this.isFormSubmitted = true;

      this.accountService.postRegister(this.loginForm.value)
        .subscribe({
          next: (response: LoginUser) => {
            console.log(response);

            this.isFormSubmitted = false;

            this.router.navigate(['/cities']);

            this.loginForm.reset();
          },
          error: err => {
            console.log(err);
          },
          complete: () => {}
        });
    }
  }
}
