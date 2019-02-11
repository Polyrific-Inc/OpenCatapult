import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobDefinitionComponent } from './job-definition.component';

describe('JobDefinitionComponent', () => {
  let component: JobDefinitionComponent;
  let fixture: ComponentFixture<JobDefinitionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobDefinitionComponent ]
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
