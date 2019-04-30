import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSetRoleDialogComponent } from './user-set-role-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatTableModule, MatIconModule, MatDialogModule, MatInputModule, MatSelectModule,
  MatProgressBarModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@app/core';

describe('UserSetRoleDialogComponent', () => {
  let component: UserSetRoleDialogComponent;
  let fixture: ComponentFixture<UserSetRoleDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserSetRoleDialogComponent ],
      imports: [
        BrowserAnimationsModule,
        FlexLayoutModule,
        HttpClientTestingModule,
        MatTableModule,
        MatIconModule,
        MatDialogModule,
        ReactiveFormsModule,
        MatInputModule,
        MatSelectModule,
        MatProgressBarModule,
        SharedModule.forRoot(),
        CoreModule
      ],
      providers: [
        {
          provide: MatDialogRef, useValue: {
            close: function (result) {

            }
          }
        },
        {
          provide: MAT_DIALOG_DATA, useValue: {

          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserSetRoleDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
