import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobTaskDefinitionComponent } from './job-task-definition.component';
import { MatButtonModule, MatListModule, MatIconModule, MatDialogModule, MatSnackBarModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SnackbarService } from '@app/shared';
import { DragDropModule } from '@angular/cdk/drag-drop';

describe('JobTaskDefinitionComponent', () => {
  let component: JobTaskDefinitionComponent;
  let fixture: ComponentFixture<JobTaskDefinitionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobTaskDefinitionComponent ],
      imports: [
        MatButtonModule,
        MatListModule,
        FlexLayoutModule,
        MatIconModule,
        CoreModule,
        MatDialogModule,
        HttpClientTestingModule,
        DragDropModule,
        MatSnackBarModule
      ],
      providers: [
        SnackbarService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobTaskDefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
