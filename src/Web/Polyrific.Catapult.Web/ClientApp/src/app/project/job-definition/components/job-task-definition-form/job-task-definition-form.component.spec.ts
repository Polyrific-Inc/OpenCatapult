import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobTaskDefinitionFormComponent } from './job-task-definition-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatSelectModule, MatDividerModule } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { TaskProviderService } from '@app/core';
import { ApiService } from '@app/core/services/api.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('JobTaskDefinitionFormComponent', () => {
  let component: JobTaskDefinitionFormComponent;
  let fixture: ComponentFixture<JobTaskDefinitionFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobTaskDefinitionFormComponent ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        ReactiveFormsModule,
        MatInputModule,
        MatSelectModule,
        MatDividerModule,
        SharedModule.forRoot()
      ],
      providers: [
        TaskProviderService,
        ApiService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobTaskDefinitionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
