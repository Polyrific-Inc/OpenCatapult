import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberNewDialogComponent } from './member-new-dialog.component';
import { MemberRoutingModule } from '../../member-routing.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatTableModule, MatIconModule, MatDialogModule, MatInputModule, MatSelectModule,
  MatProgressBarModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('MemberNewDialogComponent', () => {
  let component: MemberNewDialogComponent;
  let fixture: ComponentFixture<MemberNewDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MemberNewDialogComponent ],
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
    fixture = TestBed.createComponent(MemberNewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
