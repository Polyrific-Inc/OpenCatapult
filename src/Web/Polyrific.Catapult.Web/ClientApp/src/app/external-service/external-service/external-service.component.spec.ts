import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalServiceComponent } from './external-service.component';
import { FlexModule } from '@angular/flex-layout';
import { ExternalServiceRoutingModule } from '../external-service-routing.module';
import { MatTableModule, MatIconModule, MatButtonModule, MatDialogModule,
  MatInputModule, MatSelectModule, MatProgressBarModule, MatDividerModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { GenericService } from '../services/generic.service';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@app/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('ExternalServiceComponent', () => {
  let component: ExternalServiceComponent;
  let fixture: ComponentFixture<ExternalServiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExternalServiceComponent ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        RouterTestingModule,
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
    fixture = TestBed.createComponent(ExternalServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
