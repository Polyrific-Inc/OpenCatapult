import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectArchiveDetailComponent } from './project-archive-detail.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatProgressBarModule, MatInputModule, MatSnackBarModule } from '@angular/material';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { ProjectInfoFormComponent } from '../components/project-info-form/project-info-form.component';

describe('ProjectArchiveDetailComponent', () => {
  let component: ProjectArchiveDetailComponent;
  let fixture: ComponentFixture<ProjectArchiveDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectArchiveDetailComponent, ProjectInfoFormComponent ],
      imports: [
        BrowserAnimationsModule,
        ReactiveFormsModule,
        MatProgressBarModule,
        MatInputModule,
        RouterTestingModule,
        HttpClientTestingModule,
        MatSnackBarModule,
        CoreModule,
        SharedModule.forRoot()
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            params: of({ id: 1})
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectArchiveDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
