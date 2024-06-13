import { Component } from '@angular/core';
import {AbstractControl, FormControl, FormGroup, Validators} from "@angular/forms";
import {AccountService} from "../../services/account.service";
import {Router} from "@angular/router";
import {RegisterUser} from "../../models/register-user";
import {CompareValidator} from "../../validators/custom.validators";
import {authenticationResponse} from "../../interfaces/authenticationResponse";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  public registerForm: FormGroup;
  public isFormSubmitted: boolean = false;

  constructor(private accountService: AccountService, private router: Router) {
    this.registerForm = new FormGroup({
      personName: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required]),
      phoneNumber: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required]),
      confirmPassword: new FormControl(null, [Validators.required]),
    },
      {
        validators: [CompareValidator("password", "confirmPassword")]
      });
  }

  get register_personNameControl(): AbstractControl{
    return this.registerForm.controls["personName"];
  }
  get register_emailControl(): AbstractControl{
    return this.registerForm.controls["email"];
  }
  get register_phoneNumberControl(): AbstractControl{
    return this.registerForm.controls["phoneNumber"];
  }
  get register_passwordControl(): AbstractControl{
    return this.registerForm.controls["password"];
  }
  get register_confirmPasswordControl(): AbstractControl{
    return this.registerForm.controls["confirmPassword"];
  }

  registerSubmitted(){
    if (this.registerForm.valid){
      this.isFormSubmitted = true;

      this.accountService.postRegister(this.registerForm.value)
        .subscribe({
          next: (response:authenticationResponse) => {

            this.isFormSubmitted = false;
            this.accountService.currentUsername = response.email;
            localStorage.setItem("token", response.token);
            this.router.navigate(['/cities']);

            this.registerForm.reset();
          },
          error: err => {
            console.log(err);
          },
          complete: () => {}
        });
    }
  }
}
