import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberComponent } from './member.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatTableModule, MatIconModule, MatDialogModule, MatInputModule,
  MatSelectModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CoreModule, ProjectService } from '@app/core';
import { MemberInfoDialogComponent } from '../components/member-info-dialog/member-info-dialog.component';
import { MemberNewDialogComponent } from '../components/member-new-dialog/member-new-dialog.component';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { AuthService } from '@app/core/auth/auth.service';

describe('MemberComponent', () => {
  let component: MemberComponent;
  let fixture: ComponentFixture<MemberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MemberComponent, MemberInfoDialogComponent, MemberNewDialogComponent ],
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        FlexLayoutModule,
        MatTableModule,
        MatIconModule,
        SharedModule,
        MatDialogModule,
        ReactiveFormsModule,
        MatInputModule,
        MatSelectModule,
        MatProgressBarModule,
        MatButtonModule,
        CoreModule,
        SharedModule.forRoot()
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            parent: {
              parent: {
                snapshot: { params: of({ id: 1}) }
              }
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
    fixture = TestBed.createComponent(MemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
