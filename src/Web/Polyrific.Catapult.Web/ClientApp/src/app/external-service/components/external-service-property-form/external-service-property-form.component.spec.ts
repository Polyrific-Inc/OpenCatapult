import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalServicePropertyFormComponent } from './external-service-property-form.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FlexModule } from '@angular/flex-layout';
import { MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, MatInputModule,
  MatSelectModule, MatProgressBarModule, MatDividerModule } from '@angular/material';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ExternalServicePropertyFormComponent', () => {
  let component: ExternalServicePropertyFormComponent;
  let fixture: ComponentFixture<ExternalServicePropertyFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExternalServicePropertyFormComponent ],
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
        CoreModule,
        SharedModule.forRoot()
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExternalServicePropertyFormComponent);
    component = fixture.componentInstance;
    component.externalServiceProperty = {
      name: '',
      description: '',
      allowedValues: [],
      isRequired: false,
      isSecret: false,
      sequence: 0,
      additionalLogic: null
    };
    component.form = new FormGroup({});
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
