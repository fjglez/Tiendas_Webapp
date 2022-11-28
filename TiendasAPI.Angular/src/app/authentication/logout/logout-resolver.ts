import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { AuthenticationService } from "../../shared/authentication/authentication.service";

@Injectable()
export class LogoutResolver implements Resolve<void> {

  constructor(private authenticationService: AuthenticationService) { }

    resolve() {
      console.log("Cerrando sesión");
      this.authenticationService.logout();
    }
}
