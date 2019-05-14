import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalServiceFormComponent } from './external-service-form.component';
import { FlexModule } from '@angular/flex-layout';
import { MatTableModule, MatIconModule, MatButtonModule, MatDialogModule,
  MatInputModule, MatSelectModule, MatProgressBarModule, MatDividerModule, MatCheckboxModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { GenericService } from '@app/external-service/services/generic.service';
import { SharedModule } from '@app/shared/shared.module';
import { ExternalServiceGenericFormComponent } from '../external-service-generic-form/external-service-generic-form.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ExternalServicePropertyFormComponent } from '../external-service-property-form/external-service-property-form.component';
import { CoreModule } from '@app/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ExternalServiceFormComponent', () => {
  let component: ExternalServiceFormComponent;
  let fixture: ComponentFixture<ExternalServiceFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExternalServiceFormComponent, ExternalServiceGenericFormComponent, ExternalServicePropertyFormComponent ],
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
        GenericService
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExternalServiceFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
