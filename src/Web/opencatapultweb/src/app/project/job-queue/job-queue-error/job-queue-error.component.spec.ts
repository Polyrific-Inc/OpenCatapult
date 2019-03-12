import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueErrorComponent } from './job-queue-error.component';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute } from '@angular/router';
import { MatIconModule, MatButtonModule } from '@angular/material';

describe('JobQueueErrorComponent', () => {
  let component: JobQueueErrorComponent;
  let fixture: ComponentFixture<JobQueueErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueErrorComponent ],
      imports: [
        RouterTestingModule,
        MatIconModule,
        MatButtonModule
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            snapshot: {
              params: { id: 1 }
            },
            parent: {
              parent: {
                snapshot: { params: { projectId: 1 }}
              }
            }
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobQueueErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
