import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../shared/authentication/authentication.service';
import { IRegisterForm } from '../../shared/authentication/iregisterform';
import { ILoggingUser } from '../../shared/authentication/ILoggingUser';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  templateRegisterUser: IRegisterForm = {
    username: undefined,
    email: undefined,
    password: undefined,
    confirmPassword: undefined
  }
  registerUser: IRegisterForm = { ...this.templateRegisterUser }

  errorMessage?: string;

  constructor(private router: Router, private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {
    // El nombre de usuario no puede tener caracteres especiales
    const nameRegexp: RegExp = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/? ]/;
    if (this.registerUser.username != null && nameRegexp.test(this.registerUser.username)) {
      form.controls['username'].setErrors({ invalidusername: true });
    }
    // La contraseña y la confirmación deben coincidir
    if (this.registerUser.confirmPassword != this.registerUser.password) {
      form.controls['confirmPassword'].setErrors({ mustmatch: true });
    }
    if (form.valid) {
      this.authenticationService.register(this.registerUser).subscribe(
        result => this.onHttpSuccess(form),
        error => this.onHttpError(error,form)
      );;
    }
  }

  onHttpError(errorResponse: any, form: NgForm) {
    console.error('Error: ', errorResponse);
    if (errorResponse.status == "409") {
      form.controls['username'].setErrors({ existingusername: true });
    }
    else {
      this.errorMessage = "Ha ocurrido un error";
    }
  }

  onHttpSuccess(form: NgForm) {
    let logUser: ILoggingUser = {
      username: this.registerUser.username,
      password: this.registerUser.password
    }
    this.authenticationService.login(logUser).subscribe(
      error => this.onHttpError(error,form)
    );
    this.router.navigate(['']);

  }

}
