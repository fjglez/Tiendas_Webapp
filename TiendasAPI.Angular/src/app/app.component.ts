import { Component } from '@angular/core';
import { AuthenticationService } from './shared/authentication/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TiendasAPI.Angular';
  isExpanded = false;

  constructor(public authenticationService: AuthenticationService) { }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
