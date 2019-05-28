import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-show-recovery-two-factor',
  templateUrl: './show-recovery-two-factor.component.html',
  styleUrls: ['./show-recovery-two-factor.component.css']
})
export class ShowRecoveryTwoFactorComponent implements OnInit {
  @Input() recoveryCodes: string[];

  constructor() { }

  ngOnInit() {
  }

}
