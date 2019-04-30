import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskProviderInfoDialogComponent } from './task-provider-info-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatTableModule, MatButtonModule, MatIconModule, MatDialogModule,
  MatSelectModule, MatInputModule, MatChipsModule, MatProgressBarModule, MatTooltipModule,
  MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { FlexModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';

describe('TaskProviderInfoDialogComponent', () => {
  let component: TaskProviderInfoDialogComponent;
  let fixture: ComponentFixture<TaskProviderInfoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskProviderInfoDialogComponent ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        MatTableModule,
        MatButtonModule,
        MatIconModule,
        MatDialogModule,
        MatSelectModule,
        ReactiveFormsModule,
        MatInputModule,
        MatChipsModule,
        SharedModule.forRoot(),
        CoreModule,
        FlexModule,
        MatProgressBarModule,
        MatTooltipModule
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
    fixture = TestBed.createComponent(TaskProviderInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
