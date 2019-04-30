import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalServiceGenericFormComponent } from './external-service-generic-form.component';
import { FlexModule } from '@angular/flex-layout';
import { MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, MatInputModule,
  MatSelectModule, MatProgressBarModule, MatDividerModule } from '@angular/material';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { GenericService } from '@app/external-service/services/generic.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ExternalServiceGenericFormComponent', () => {
  let component: ExternalServiceGenericFormComponent;
  let fixture: ComponentFixture<ExternalServiceGenericFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExternalServiceGenericFormComponent ],
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
      providers: [
        GenericService
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExternalServiceGenericFormComponent);
    component = fixture.componentInstance;
    component.form = new FormGroup({});
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
