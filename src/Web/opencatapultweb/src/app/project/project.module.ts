import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProjectRoutingModule } from './project-routing.module';
import { ProjectComponent } from './project/project.component';
import { ProjectDetailComponent } from './project-detail/project-detail.component';
import { ProjectInfoComponent } from './project-info/project-info.component';
import { MatSidenavModule, MatToolbarModule, MatListModule, MatIconModule, MatGridListModule, MatExpansionModule, MatTabsModule, MatButtonModule, MatFormFieldModule, MatProgressBarModule, MatInputModule, MatSelectModule, MatDialogModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ProjectInfoFormComponent } from './components/project-info-form/project-info-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ProjectNewComponent } from './project-new/project-new.component';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [ProjectComponent, ProjectDetailComponent, ProjectInfoComponent, ProjectInfoFormComponent, ProjectNewComponent],
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
    MatDialogModule
  ]
})
export class ProjectModule { }
