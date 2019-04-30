import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDeletingComponent } from './project-deleting.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatDialogModule, MatTabsModule, MatProgressSpinnerModule, MatChipsModule } from '@angular/material';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { AuthService } from '@app/core/auth/auth.service';
import { of } from 'rxjs';

describe('ProjectDeletingComponent', () => {
  let component: ProjectDeletingComponent;
  let fixture: ComponentFixture<ProjectDeletingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectDeletingComponent ],
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        MatDialogModule,
        MatTabsModule,
        MatChipsModule,
        CoreModule,
        MatProgressSpinnerModule,
        SharedModule.forRoot()
      ],
      providers: [
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
    fixture = TestBed.createComponent(ProjectDeletingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
