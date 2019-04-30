import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountComponent } from './account.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatTableModule, MatButtonModule, MatIconModule, MatSelectModule,
  MatTooltipModule, MatInputModule, MatProgressBarModule, MatDialogModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@app/core';
import { UserInfoDialogComponent } from '../components/user-info-dialog/user-info-dialog.component';
import { UserRegisterDialogComponent } from '../components/user-register-dialog/user-register-dialog.component';
import { UserSetRoleDialogComponent } from '../components/user-set-role-dialog/user-set-role-dialog.component';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('AccountComponent', () => {
  let component: AccountComponent;
  let fixture: ComponentFixture<AccountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AccountComponent,
        UserInfoDialogComponent,
        UserRegisterDialogComponent,
        UserSetRoleDialogComponent
      ],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        MatTableModule,
        MatButtonModule,
        MatIconModule,
        MatSelectModule,
        MatTooltipModule,
        FlexLayoutModule,
        ReactiveFormsModule,
        SharedModule.forRoot(),
        CoreModule,
        MatInputModule,
        MatProgressBarModule,
        MatDialogModule
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            queryParams: of({ newUser: true})
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
