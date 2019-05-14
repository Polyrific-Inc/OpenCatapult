import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EngineRegisterDialogComponent } from './engine-register-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatTableModule, MatButtonModule, MatDialogModule, MatIconModule, MatSelectModule,
   MatTooltipModule, MatProgressBarModule, MatInputModule, MatDatepickerModule, MatNativeDateModule,
   MatDialogRef } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { FlexModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@app/core';

describe('EngineRegisterDialogComponent', () => {
  let component: EngineRegisterDialogComponent;
  let fixture: ComponentFixture<EngineRegisterDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EngineRegisterDialogComponent ],
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
        }
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EngineRegisterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
