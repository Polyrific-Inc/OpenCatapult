import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@aspnet/signalr';
import { AuthService } from '../auth/auth.service';
import { Subject } from 'rxjs';
import { Config, ConfigService } from '../../config/config.service';

@Injectable()
export class SignalrService {
  private connection: HubConnection;
  private config: Config;

  constructor(
    private configService: ConfigService,
    private authenticationService: AuthService) {
      this.config = this.configService.getConfig();
    }

  connect(path: string, message$: Subject<any>) {
    if (!this.config) {
      this.config = this.configService.getConfig();
    }

    const currentUser = this.authenticationService.currentUserValue;
    if (!this.connection) {
      this.connection = new HubConnectionBuilder()
        .withUrl(`${this.config.apiUrl}/${path}`, {
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
