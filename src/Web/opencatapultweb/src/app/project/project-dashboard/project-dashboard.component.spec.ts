import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDashboardComponent } from './project-dashboard.component';
import { CoreModule } from '@app/core';
import { MatCardModule, MatDividerModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SharedModule } from '@app/shared/shared.module';
import { AuthService } from '@app/core/auth/auth.service';
import { of } from 'rxjs';

describe('ProjectDashboardComponent', () => {
  let component: ProjectDashboardComponent;
  let fixture: ComponentFixture<ProjectDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectDashboardComponent ],
      imports: [
        CoreModule,
        HttpClientTestingModule,
        MatCardModule,
        FlexLayoutModule,
        MatDividerModule,
        RouterTestingModule,
        SharedModule.forRoot()
      ],
      providers: [
        {
          provide: AuthService, useValue: {
            currentUserValue: {
              role: 'Administrator'
            },
            checkRoleAuthorization: function(test, test2) {

            },
            currentUser: of({
              role: 'Administrator'
            })
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
