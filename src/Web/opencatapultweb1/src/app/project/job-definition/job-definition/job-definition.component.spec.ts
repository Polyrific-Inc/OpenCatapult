import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobDefinitionComponent } from './job-definition.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatButtonModule, MatExpansionModule, MatListModule, MatIconModule,
  MatCheckboxModule, MatDialogModule, MatSnackBarModule, MatProgressSpinnerModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';
import { FormsModule } from '@angular/forms';
import { SnackbarService } from '@app/shared';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { JobTaskDefinitionComponent } from '../job-task-definition/job-task-definition.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { SharedModule } from '@app/shared/shared.module';

describe('JobDefinitionComponent', () => {
  let component: JobDefinitionComponent;
  let fixture: ComponentFixture<JobDefinitionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobDefinitionComponent, JobTaskDefinitionComponent ],
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        MatButtonModule,
        MatExpansionModule,
        MatListModule,
        FlexLayoutModule,
        MatIconModule,
        CoreModule,
        MatCheckboxModule,
        FormsModule,
        MatDialogModule,
        MatSnackBarModule,
        DragDropModule,
        MatProgressSpinnerModule,
        MatCheckboxModule,
        SharedModule.forRoot()
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            parent: {
              parent: {
                snapshot: { params: of({ id: 1}) }
              }
            }
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobDefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
