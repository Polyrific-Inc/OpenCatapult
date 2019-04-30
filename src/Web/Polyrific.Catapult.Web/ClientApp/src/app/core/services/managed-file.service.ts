import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { ConfigService } from '@app/config/config.service';
import { ManagedFileDto } from '../models/managed-file/managed-file-dto';

@Injectable()
export class ManagedFileService {

  constructor(
    private api: ApiService,
    private configService: ConfigService) { }

  getImageUrl(managedFileId: number) {
    const apiUrl = this.configService.getConfig().apiUrl;
    return `${apiUrl}/file/${managedFileId}/content?${(new Date()).getTime()}`;
  }

  createManagedFile(fileToUpload: File) {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    return this.api.post<ManagedFileDto>('file', formData);
  }

  updateManagedFile(managedFileId: number, fileToUpload: File) {
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    return this.api.put(`file/${managedFileId}`, formData);
  }

  deleteManagedFile(managedFileId: number) {
    return this.api.delete(`file/${managedFileId}`);
  }
}
