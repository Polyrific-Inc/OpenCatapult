import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EngineTokenDialogComponent } from './engine-token-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatTableModule, MatButtonModule, MatDialogModule, MatIconModule, MatSelectModule,
   MatTooltipModule, MatProgressBarModule, MatInputModule, MatDatepickerModule, MatNativeDateModule,
    MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { FlexModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@app/core';

describe('EngineTokenDialogComponent', () => {
  let component: EngineTokenDialogComponent;
  let fixture: ComponentFixture<EngineTokenDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EngineTokenDialogComponent ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        MatTableModule,
        SharedModule.forRoot(),
        MatButtonModule,
        MatDialogModule,
        FlexModule,
        MatIconModule,
        MatSelectModule,
        ReactiveFormsModule,
        MatTooltipModule,
        MatProgressBarModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
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
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EngineTokenDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
