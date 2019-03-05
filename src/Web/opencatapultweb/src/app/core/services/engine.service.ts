import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { EngineDto } from '../models/engine/engine-dto';
import { RegisterEngineDto } from '../models/engine/register-engine-dto';
import { EngineTokenRequestDto } from '../models/engine/engine-token-request-dto';
import { Observable } from 'rxjs';

@Injectable()
export class EngineService {

  constructor(private api: ApiService) { }

  getEngineById(id: number) {
    return this.api.get<EngineDto>(`engine/${id}`);
  }

  getEngines(status: string) {
    return this.api.get<EngineDto[]>(`engine?status=${status}`);
  }

  activate(engineId: number) {
    return this.api.postString(`engine/${engineId}/activate`);
  }

  register(dto: RegisterEngineDto) {
    return this.api.post<EngineDto>('engine/register', dto);
  }

  remove(engineId: number) {
    return this.api.delete(`engine/${engineId}`);
  }

  suspend(engineId: number) {
    return this.api.postString(`engine/${engineId}/suspend`);
  }

  requestToken(engineId: number, dto: EngineTokenRequestDto): Observable<string> {
    return this.api.postString(`token/engine/${engineId}`, dto);
  }
}
