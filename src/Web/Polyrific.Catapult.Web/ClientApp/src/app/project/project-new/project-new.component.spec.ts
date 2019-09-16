import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectNewComponent } from './project-new.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatProgressBarModule, MatInputModule, MatSnackBarModule, MatOptionModule,
  MatSelectModule, MatIconModule, MatTooltipModule, MatStepperModule, MatTabsModule,
  MatExpansionModule, MatDividerModule, MatListModule } from '@angular/material';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { ProjectInfoFormComponent } from '../components/project-info-form/project-info-form.component';
import { JobListFormComponent } from '../components/job-list-form/job-list-form.component';
import { DataModelComponent } from '../components/data-model/data-model.component';
import { DataModelPropertyComponent } from '../components/data-model-property/data-model-property.component';

describe('ProjectNewComponent', () => {
  let component: ProjectNewComponent;
  let fixture: ComponentFixture<ProjectNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        ProjectNewComponent,
        ProjectInfoFormComponent,
        JobListFormComponent,
        DataModelComponent,
        DataModelPropertyComponent
      ],
      imports: [
        BrowserAnimationsModule,
        ReactiveFormsModule,
        MatProgressBarModule,
        MatInputModule,
        RouterTestingModule,
        HttpClientTestingModule,
        MatOptionModule,
        MatSelectModule,
        MatSnackBarModule,
        SharedModule.forRoot(),
        CoreModule,
        MatIconModule,
        MatTooltipModule,
        MatStepperModule,
        MatTabsModule,
        MatExpansionModule,
        MatDividerModule,
        MatListModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
