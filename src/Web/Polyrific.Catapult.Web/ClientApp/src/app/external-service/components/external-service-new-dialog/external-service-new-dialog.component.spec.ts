import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalServiceNewDialogComponent } from './external-service-new-dialog.component';
import { FlexModule } from '@angular/flex-layout';
import { MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, MatInputModule,
  MatSelectModule, MatProgressBarModule, MatDividerModule, MatDialogRef, MAT_DIALOG_DATA, MatCheckboxModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { GenericService } from '@app/external-service/services/generic.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ExternalServiceFormComponent } from '../external-service-form/external-service-form.component';
import { ExternalServiceGenericFormComponent } from '../external-service-generic-form/external-service-generic-form.component';
import { ExternalServicePropertyFormComponent } from '../external-service-property-form/external-service-property-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ExternalServiceNewDialogComponent', () => {
  let component: ExternalServiceNewDialogComponent;
  let fixture: ComponentFixture<ExternalServiceNewDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
         ExternalServiceNewDialogComponent,
         ExternalServiceFormComponent,
         ExternalServiceGenericFormComponent,
         ExternalServicePropertyFormComponent
        ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        FlexModule,
        MatTableModule,
        MatIconModule,
        MatButtonModule,
        MatDialogModule,
        ReactiveFormsModule,
        MatInputModule,
        MatSelectModule,
        MatProgressBarModule,
        MatDividerModule,
        MatCheckboxModule,
        CoreModule,
        SharedModule.forRoot()
      ],
      providers: [
        GenericService,
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
    fixture = TestBed.createComponent(ExternalServiceNewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
