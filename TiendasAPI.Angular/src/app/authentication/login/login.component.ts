import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../shared/authentication/authentication.service';
import { ILoggingUser } from '../../shared/authentication/ILoggingUser';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  templateLoggingUser: ILoggingUser = {
    username: undefined,
    password: undefined
  }
  loggingUser: ILoggingUser = { ...this.templateLoggingUser }

  errorMessage?: string;

  constructor(private router: Router, private authenticationService: AuthenticationService ) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.authenticationService.login(this.loggingUser).subscribe(
        result => this.onHttpSuccess(),
        error => this.onHttpError(error)
      );;
    }
  }

  onHttpError(errorResponse: any) {
    console.error('Error: ', errorResponse);
    if (errorResponse.status == "401") {
      this.errorMessage = "El usuario y la contraseña no coinciden"
    }
    else {
      this.errorMessage = "Ha ocurrido un error";
    }
  }

  onHttpSuccess() {
    this.router.navigate(['']);

  }
}
