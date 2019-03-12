import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProjectRoutingModule } from './project-routing.module';
import { ProjectComponent } from './project/project.component';
import { ProjectDetailComponent } from './project-detail/project-detail.component';
import { ProjectInfoComponent } from './project-info/project-info.component';
import { MatSidenavModule, MatToolbarModule, MatListModule, MatIconModule, MatGridListModule,
  MatExpansionModule, MatTabsModule, MatButtonModule, MatFormFieldModule, MatProgressBarModule,
  MatInputModule, MatSelectModule, MatDialogModule, MatCheckboxModule, MatCardModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ProjectInfoFormComponent } from './components/project-info-form/project-info-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ProjectNewComponent } from './project-new/project-new.component';
import { SharedModule } from '@app/shared/shared.module';
import { ProjectCloneComponent } from './project-clone/project-clone.component';
import { ProjectArchiveDetailComponent } from './project-archive-detail/project-archive-detail.component';
import { ProjectDashboardComponent } from './project-dashboard/project-dashboard.component';
import { ProjectErrorComponent } from './project-error/project-error.component';
import { ProjectResolverService } from './services/project-resolver.service';

@NgModule({
  declarations: [ProjectComponent,
    ProjectDetailComponent,
    ProjectInfoComponent,
    ProjectInfoFormComponent,
    ProjectNewComponent,
    ProjectCloneComponent,
    ProjectArchiveDetailComponent,
    ProjectDashboardComponent,
    ProjectErrorComponent
  ],
  imports: [
    CommonModule,
    ProjectRoutingModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule,
    MatGridListModule,
    MatExpansionModule,
    MatButtonModule,
    MatTabsModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatProgressBarModule,
    MatInputModule,
    MatSelectModule,
    FlexLayoutModule,
    SharedModule,
    MatDialogModule,
    MatCheckboxModule,
    MatCardModule
  ],
  providers: [
    ProjectResolverService
  ]
})
export class ProjectModule { }
