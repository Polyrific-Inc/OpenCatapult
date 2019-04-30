import { Injectable, Type } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { HelpContextDto } from '..';
import { HelpContextSection } from '../enums/help-context-section';

@Injectable()
export class HelpContextService {
  constructor(private api: ApiService) {
  }

  getHelpContextsBySection(section: string): Observable<HelpContextDto[]> {
    return this.api.get<HelpContextDto[]>(`help-context/section/${section}`);
  }

  getSectionByActiveRoute(url: string): string {
    const urlSegments = url.toLowerCase().split('/');

    if (urlSegments.includes('project')) {
      if (urlSegments.includes('data-model')) {
        return HelpContextSection.ProjectModel;
      } else if (urlSegments.includes('job-definition')) {
        return HelpContextSection.JobDefinition;
      } else if (urlSegments.includes('job-queue')) {
        return HelpContextSection.JobQueue;
      } else if (urlSegments.includes('member')) {
        return HelpContextSection.ProjectMember;
      } else {
        return HelpContextSection.Project;
      }
    } else if (urlSegments.includes('service')) {
      return HelpContextSection.ExternalService;
    } else if (urlSegments.includes('engine')) {
      return HelpContextSection.Engine;
    } else if (urlSegments.includes('provider')) {
      return HelpContextSection.TaskProvider;
    } else if (urlSegments.includes('user')) {
      return HelpContextSection.User;
    } else if (urlSegments.includes('user-profile')) {
      return HelpContextSection.UserProfile;
    }

    return null;
  }
}
