import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskProviderComponent } from './task-provider.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatTableModule, MatButtonModule, MatIconModule, MatDialogModule, MatSelectModule,
  MatInputModule, MatChipsModule, MatProgressBarModule, MatTooltipModule, MatAutocompleteModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { FlexModule } from '@angular/flex-layout';
import { TaskProviderInfoDialogComponent } from '../components/task-provider-info-dialog/task-provider-info-dialog.component';
import { TaskProviderRegisterDialogComponent } from '../components/task-provider-register-dialog/task-provider-register-dialog.component';
import { CoreModule } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('TaskProviderComponent', () => {
  let component: TaskProviderComponent;
  let fixture: ComponentFixture<TaskProviderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskProviderComponent, TaskProviderInfoDialogComponent, TaskProviderRegisterDialogComponent ],
      imports: [
        HttpClientTestingModule,
        BrowserAnimationsModule,
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
        MatTooltipModule,
        MatAutocompleteModule
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            queryParams: of({ newProvider: true})
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskProviderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
