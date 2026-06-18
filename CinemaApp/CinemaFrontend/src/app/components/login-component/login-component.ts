import { Component } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { LoginService } from '../../services/login/login';
import { Router } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-login-component',
  imports: [NgIf,NgFor],
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

  constructor(public loginService: LoginService, private router: Router) { }

  ngOnInit(): void { }

  //Login
  login() {
    if (this.loginForma.valid) {
      this.loginService.login(this.loginForma.value).subscribe(

        (res: any) => {
          if (res.token) {
            console.log("Working")
            this.router.navigate(['/Genre']);

          } else {
            console.log(res.message)
            this.loginFailed = true;

          }
        },



      );
    }

  }
}
