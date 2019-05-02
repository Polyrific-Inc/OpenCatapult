import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login-layout',
  templateUrl: './login-layout.component.html',
  styleUrls: ['./login-layout.component.css']
})
export class LoginLayoutComponent implements OnInit {
  public version: string = 'v' + require('../../../../package.json').version;

  constructor() { }

  ngOnInit() {
  }

}
