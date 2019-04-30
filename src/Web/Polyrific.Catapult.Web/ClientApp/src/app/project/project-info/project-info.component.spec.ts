import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectInfoComponent } from './project-info.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatProgressBarModule, MatInputModule, MatSnackBarModule } from '@angular/material';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CoreModule } from '@app/core';
import { ProjectInfoFormComponent } from '../components/project-info-form/project-info-form.component';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from '@app/shared/shared.module';
import { AuthService } from '@app/core/auth/auth.service';

describe('ProjectInfoComponent', () => {
  let component: ProjectInfoComponent;
  let fixture: ComponentFixture<ProjectInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectInfoComponent, ProjectInfoFormComponent ],
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
            parent: {
              data: of({project: { id: 1}})
            }
          }
        },
        {
          provide: AuthService, useValue: {
            currentUserValue: {
              role: 'Administrator'
            },
            checkRoleAuthorization: function(test, test2) {

            }
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
