import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProjectRoutingModule } from './project-routing.module';
import { ProjectComponent } from './project/project.component';
import { ProjectDetailComponent } from './project-detail/project-detail.component';
import { ProjectInfoComponent } from './project-info/project-info.component';
import { MatSidenavModule, MatToolbarModule, MatListModule, MatIconModule, MatGridListModule, MatExpansionModule, MatTabsModule, MatButtonModule, MatFormFieldModule, MatProgressBarModule, MatInputModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ProjectInfoFormComponent } from './components/project-info-form/project-info-form.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [ProjectComponent, ProjectDetailComponent, ProjectInfoComponent, ProjectInfoFormComponent],
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
    FlexLayoutModule
  ]
})
export class ProjectModule { }
