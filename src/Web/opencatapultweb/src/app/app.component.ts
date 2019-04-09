import { Component } from '@angular/core';
import { Router, RouterEvent, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  showRouteLoadIndicator: boolean;

  constructor(router: Router) {
    this.showRouteLoadIndicator = false;
        let asyncLoadCount = 0;

        router.events.subscribe(
            ( event: RouterEvent ): void => {
                if ( event instanceof RouteConfigLoadStart ) {
                    asyncLoadCount++;
                } else if ( event instanceof RouteConfigLoadEnd ) {
                    asyncLoadCount--;
                }

                this.showRouteLoadIndicator = !! asyncLoadCount;
            }
        );
  }
}
