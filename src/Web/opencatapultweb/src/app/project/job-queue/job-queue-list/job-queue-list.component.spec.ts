import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueListComponent } from './job-queue-list.component';
import { MatTabsModule, MatIconModule, MatBadgeModule, MatTableModule, MatButtonModule,
  MatTooltipModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatChipsModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { JobQueueStatusComponent } from '../components/job-queue-status/job-queue-status.component';
import { RouterTestingModule } from '@angular/router/testing';

describe('JobQueueListComponent', () => {
  let component: JobQueueListComponent;
  let fixture: ComponentFixture<JobQueueListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueListComponent, JobQueueStatusComponent ],
      imports: [
        RouterTestingModule,
        MatTabsModule,
        MatIconModule,
        MatBadgeModule,
        MatTableModule,
        MatButtonModule,
        MatTooltipModule,
        MatProgressSpinnerModule,
        FlexLayoutModule,
        MatPaginatorModule,
        MatSortModule,
        MatChipsModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobQueueListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
