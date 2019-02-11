import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProjectRoutingModule } from './project-routing.module';
import { ProjectComponent } from './project/project.component';
import { ProjectDetailComponent } from './project-detail/project-detail.component';
import { ProjectInfoComponent } from './project-info/project-info.component';
import { MatSidenavModule, MatToolbarModule, MatListModule, MatIconModule } from '@angular/material';

@NgModule({
  declarations: [ProjectComponent, ProjectDetailComponent, ProjectInfoComponent],
  imports: [
    CommonModule,
    ProjectRoutingModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule
  ]
})
export class ProjectModule { }
