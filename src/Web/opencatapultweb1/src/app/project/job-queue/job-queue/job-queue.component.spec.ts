import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueComponent } from './job-queue.component';
import { JobQueueListComponent } from '../job-queue-list/job-queue-list.component';
import { MatTabsModule, MatIconModule, MatBadgeModule, MatTableModule, MatButtonModule,
  MatTooltipModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatChipsModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CoreModule } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JobQueueStatusComponent } from '../components/job-queue-status/job-queue-status.component';
import { SharedModule } from '@app/shared/shared.module';

describe('JobQueueComponent', () => {
  let component: JobQueueComponent;
  let fixture: ComponentFixture<JobQueueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueComponent, JobQueueListComponent, JobQueueStatusComponent ],
      imports: [
        BrowserAnimationsModule,
        RouterTestingModule,
        HttpClientTestingModule,
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
        CoreModule,
        MatChipsModule,
        SharedModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobQueueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
