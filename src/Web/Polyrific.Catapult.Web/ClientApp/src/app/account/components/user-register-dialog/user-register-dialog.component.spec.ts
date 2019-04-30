import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserRegisterDialogComponent } from './user-register-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatInputModule, MatButtonModule, MatDialogModule, MatProgressBarModule, MatDialogRef } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@app/core';

describe('UserRegisterDialogComponent', () => {
  let component: UserRegisterDialogComponent;
  let fixture: ComponentFixture<UserRegisterDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserRegisterDialogComponent ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        MatButtonModule,
        MatInputModule,
        ReactiveFormsModule,
        SharedModule.forRoot(),
        MatProgressBarModule,
        MatDialogModule,
        CoreModule
      ],
      providers: [
        {
          provide: MatDialogRef, useValue: {
            close: function (result) {

            }
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserRegisterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
