import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { ProjectService } from './services/project.service';
import { ApiService } from './services/api.service';
import { ProjectTemplateService } from './services/project-template.service';
import { AuthGuard } from './auth/auth.guard';
import { AuthService } from './auth/auth.service';
import { ExternalServiceService } from './services/external-service.service';
import { TaskProviderService } from './services/task-provider.service';
import { DataModelService } from './services/data-model.service';
import { JobQueueService } from './services/job-queue.service';
import { JobDefinitionService } from './services/job-definition.service';
import { SignalrService } from './services/signalr.service';
import { MemberService } from './services/member.service';
import { AccountService } from './services/account.service';
import { ExternalServiceTypeService } from './services/external-service-type.service';
import { EngineService } from './services/engine.service';
import { YamlService } from './services/yaml.service';
import { ProjectHistoryService } from './services/project-history.service';
import { TextHelperService } from './services/text-helper.service';
import { ManagedFileService } from './services/managed-file.service';
import { HelpContextService } from './services/help-context.service';
import { VersionService } from './services/version.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    ApiService,
    ProjectService,
    ProjectTemplateService,
    ExternalServiceService,
    TaskProviderService,
    AuthGuard,
    AuthService,
    DataModelService,
    JobQueueService,
    JobDefinitionService,
    SignalrService,
    MemberService,
    AccountService,
    ExternalServiceTypeService,
    EngineService,
    YamlService,
    ProjectHistoryService,
    TextHelperService,
    ManagedFileService,
    HelpContextService,
    VersionService
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error('The core module is injected twice. It should only be injected in AppModule');
    }
  }
}
