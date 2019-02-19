import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCloneComponent } from './project-clone.component';

describe('ProjectCloneComponent', () => {
  let component: ProjectCloneComponent;
  let fixture: ComponentFixture<ProjectCloneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectCloneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCloneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
