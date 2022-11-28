import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LogoutComponent } from './logout/logout.component';
import { LogoutResolver } from './logout/logout-resolver';
import { AuthActivatorGuard } from '../shared/authentication/auth-activator.guard';
import { RegisterComponent } from './register/register.component';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent
  ],
  providers: [LogoutResolver],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      { path: 'login', component: LoginComponent },
      {
        path: 'logout', component: LogoutComponent,
        resolve: { LogoutResolver },
        canActivate: [AuthActivatorGuard]
      },
      {
        path: 'register', component: RegisterComponent
      },
    ])
  ]
})
export class AuthenticationModule {

}
