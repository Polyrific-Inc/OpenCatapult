import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCloneComponent } from './project-clone.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatProgressBarModule, MatCheckboxModule, MatInputModule, MatSnackBarModule } from '@angular/material';
import { ProjectInfoFormComponent } from '../components/project-info-form/project-info-form.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('ProjectCloneComponent', () => {
  let component: ProjectCloneComponent;
  let fixture: ComponentFixture<ProjectCloneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectCloneComponent, ProjectInfoFormComponent ],
      imports: [
        BrowserAnimationsModule,
        ReactiveFormsModule,
        MatProgressBarModule,
        MatCheckboxModule,
        MatInputModule,
        RouterTestingModule,
        HttpClientTestingModule,
        CoreModule,
        SharedModule.forRoot()
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            params: of({ id: 1}),
            data: of({project: { id: 1}}),
            snapshot: {}
          }
        },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCloneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
