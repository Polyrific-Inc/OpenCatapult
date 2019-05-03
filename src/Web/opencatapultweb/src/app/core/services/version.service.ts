import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { VersionDto } from '../models/version/version-dto';

@Injectable()
export class VersionService {

  constructor(private api: ApiService) { }

  getVersions(): Observable<VersionDto> {
    return this.api.get<VersionDto>('version');
  }
}
