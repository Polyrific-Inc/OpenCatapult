import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { ApplicationSettingValueDto } from '../models/setting/application-setting-value-dto';
import { ApplicationSettingDto } from '../models/setting/application-setting-dto';
import { UpdateApplicationSettingDto } from '../models/setting/update-application-setting-dto';

@Injectable()
export class ApplicationSettingService {

  constructor(private api: ApiService) { }

  getApplicationSettings() {
    return this.api.get<ApplicationSettingDto[]>('applicationsetting');
  }

  getApplicationSettingValue() {
    return this.api.get<ApplicationSettingValueDto>('applicationsetting/value');
  }

  updateApplicationSetting(dto: UpdateApplicationSettingDto) {
    return this.api.put('applicationsetting', dto);
  }
}
