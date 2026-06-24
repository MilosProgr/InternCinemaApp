import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from '../../services/login/login';
import { Router } from '@angular/router';
import { NgIf, NgFor } from '@angular/common';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';


@Component({
  selector: 'app-login-component',
  standalone: true,
  imports: [
    NgIf,
    NgFor,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent {

  title = 'Login page';

  loginForma: FormGroup = new FormGroup({
    username: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required),
  });

  loginFailed = false;

  constructor(
    public loginService: LoginService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  login() {
    if (this.loginForma.valid) {
      this.loginService.login(this.loginForma.value).subscribe(
        (res: any) => {
          if (res.token) {
            console.log("Working");
            this.router.navigate(['/homepage']);
          } else {
            console.log(res.message);
            this.loginFailed = true;
          }
        }
      );
    }
  }
}