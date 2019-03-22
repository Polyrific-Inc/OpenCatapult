import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDeletingComponent } from './project-deleting.component';

describe('ProjectDeletingComponent', () => {
  let component: ProjectDeletingComponent;
  let fixture: ComponentFixture<ProjectDeletingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectDeletingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectDeletingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
