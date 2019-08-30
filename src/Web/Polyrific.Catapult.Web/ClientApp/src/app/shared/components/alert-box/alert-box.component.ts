import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AlertBoxService } from '@app/shared/services/alert-box.service';

@Component({
  selector: 'alert-box',
  templateUrl: './alert-box.component.html',
  styleUrls: ['./alert-box.component.css']
})
export class AlertBoxComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  message: any;

  constructor(private alertBoxService: AlertBoxService) { }

  ngOnInit() {
    this.subscription = this.alertBoxService.getMessage()
      .subscribe(message => {
          if (message) {
            message.cssClass = 'alert alert-' + message.type;
          }

          this.message = message;
        }
      );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
