import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDetailComponent } from './project-detail.component';
import { RouterTestingModule } from '@angular/router/testing';
import { MatTabsModule, MatDialogModule, MatProgressSpinnerModule, MatCardModule } from '@angular/material';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { AuthService } from '@app/core/auth/auth.service';

describe('ProjectDetailComponent', () => {
  let component: ProjectDetailComponent;
  let fixture: ComponentFixture<ProjectDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectDetailComponent ],
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        MatDialogModule,
        MatTabsModule,
        MatCardModule,
        MatProgressSpinnerModule,
        CoreModule,
        SharedModule.forRoot()
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            params: of({ id: 1}),
            data: of({project: { id: 1}}),
            snapshot: {},
            firstChild: {
              snapshot: {
                parameters: {},
                parameterMap: new Map(),
                url: [ { path: 'project' } ]
              }
            }
          }
        },
        {
          provide: AuthService, useValue: {
            currentUser: of({}),
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
    fixture = TestBed.createComponent(ProjectDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
