import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { ShopsModule } from './shops/shops.module';
import { WelcomeComponent } from './welcome/welcome.component';
import { ContactPageComponent } from './contact-page/contact-page.component';
import { AuthenticationModule } from './authentication/authentication.module';

@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent,
    ContactPageComponent
  ],
  imports: [
    BrowserModule,
    ShopsModule,
    AuthenticationModule,
    HttpClientModule,
    RouterModule.forRoot([

      { path: 'contact', component: ContactPageComponent },
      { path: '', component: WelcomeComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
