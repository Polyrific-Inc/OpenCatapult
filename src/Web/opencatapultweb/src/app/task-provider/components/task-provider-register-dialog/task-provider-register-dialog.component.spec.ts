import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskProviderRegisterDialogComponent } from './task-provider-register-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatTableModule, MatButtonModule, MatIconModule, MatDialogModule, MatSelectModule,
  MatInputModule, MatChipsModule, MatProgressBarModule, MatTooltipModule, MatDialogRef } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { FlexModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';

describe('TaskProviderRegisterDialogComponent', () => {
  let component: TaskProviderRegisterDialogComponent;
  let fixture: ComponentFixture<TaskProviderRegisterDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskProviderRegisterDialogComponent ],
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
        }
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskProviderRegisterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
