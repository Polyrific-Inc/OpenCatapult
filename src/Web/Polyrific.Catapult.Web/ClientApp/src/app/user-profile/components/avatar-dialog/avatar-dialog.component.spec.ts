import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvatarDialogComponent } from './avatar-dialog.component';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { MatDialogModule, MatProgressBarModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('AvatarDialogComponent', () => {
  let component: AvatarDialogComponent;
  let fixture: ComponentFixture<AvatarDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvatarDialogComponent ],
      imports: [
        HttpClientTestingModule,
        CoreModule,
        SharedModule.forRoot(),
        MatDialogModule,
        MatProgressBarModule
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
            file: new File([], 'test')
          }
        }
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvatarDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
