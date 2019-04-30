import { Component, OnInit } from '@angular/core';
import { AuthorizePolicy } from '@app/core';

@Component({
  selector: 'app-home-layout',
  templateUrl: './home-layout.component.html',
  styleUrls: ['./home-layout.component.css']
})
export class HomeLayoutComponent implements OnInit {
  authorizePolicyEnum = AuthorizePolicy;

  constructor(
    ) { }

  ngOnInit() {
  }
}
