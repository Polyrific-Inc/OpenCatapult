import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@aspnet/signalr';
import { AuthService } from '../auth/auth.service';
import { JobLogDto } from '../models/job-queue/job-log-dto';
import { Subject } from 'rxjs';

@Injectable()
export class SignalrService {
  private connection: HubConnection;

  constructor(
    private authenticationService: AuthService) { }

  connect(path: string, message$: Subject<any>) {
    const currentUser = this.authenticationService.currentUserValue;
    if (!this.connection) {
      this.connection = new HubConnectionBuilder()
        .withUrl(`${environment.apiUrl}/${path}`, {
          accessTokenFactory: () => currentUser.token,
          transport: HttpTransportType.LongPolling,
        })
        .build();

      this.connection.on('ReceiveMessage', (taskName: string, message: string) => {
        message$.next({
          taskName: taskName,
          message: message
        });
      });

      this.connection.on('JobCompleted', () => {
        this.disconnect();
        message$.complete();
      });

      this.connection.start().catch(err => {
        console.error(err);
      });
    }
  }

  disconnect() {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  }
}
