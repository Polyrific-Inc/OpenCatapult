import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueListComponent } from './job-queue-list.component';
import { MatTabsModule, MatIconModule, MatBadgeModule, MatTableModule, MatButtonModule,
  MatTooltipModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';

describe('JobQueueListComponent', () => {
  let component: JobQueueListComponent;
  let fixture: ComponentFixture<JobQueueListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueListComponent ],
      imports: [
        MatTabsModule,
        MatIconModule,
        MatBadgeModule,
        MatTableModule,
        MatButtonModule,
        MatTooltipModule,
        MatProgressSpinnerModule,
        FlexLayoutModule,
        MatPaginatorModule,
        MatSortModule
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
