import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EngineComponent } from './engine.component';
import { EngineTokenDialogComponent } from '../components/engine-token-dialog/engine-token-dialog.component';
import { EngineRegisterDialogComponent } from '../components/engine-register-dialog/engine-register-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatTableModule, MatButtonModule, MatDialogModule, MatIconModule, MatSelectModule,
  MatTooltipModule, MatProgressBarModule, MatInputModule, MatDatepickerModule, MatNativeDateModule } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { FlexModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('EngineComponent', () => {
  let component: EngineComponent;
  let fixture: ComponentFixture<EngineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EngineComponent, EngineTokenDialogComponent, EngineRegisterDialogComponent ],
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
          provide: ActivatedRoute, useValue: {
            queryParams: of({ newEngine: false})
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EngineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
