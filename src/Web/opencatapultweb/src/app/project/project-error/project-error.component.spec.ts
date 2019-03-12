import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectErrorComponent } from './project-error.component';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute } from '@angular/router';

describe('ProjectErrorComponent', () => {
  let component: ProjectErrorComponent;
  let fixture: ComponentFixture<ProjectErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectErrorComponent ],
      imports: [
        RouterTestingModule
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            snapshot: {
              params: { projectId: 1 }
            }
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
