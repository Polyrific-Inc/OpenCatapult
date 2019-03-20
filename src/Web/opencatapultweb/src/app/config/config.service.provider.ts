import { APP_INITIALIZER } from '@angular/core';
import { ConfigService } from './config.service';

export const ConfigServiceFactory = (configService: ConfigService) => {
  return () => {
    return configService.loadConfig();
  };
};

export const ConfigServiceProvider = {
  provide: APP_INITIALIZER,
  useFactory: ConfigServiceFactory,
  multi: true,
  deps: [ConfigService]
};
