import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  private config: Config;

  constructor(private http: HttpClient) { }

  async loadConfig() {

    // console.debug("Loading configs...");
    const data = await this.http.get<Config>('./config.json')
      .toPromise();
    this.config = data;
    // console.debug(`Configs has been loaded for environment "${this.config.environmentName}".`);

  }

  getConfig(): Config {
    return this.config;
  }
}

export interface Config {
  apiUrl: string;
  environmentName: string;
}
