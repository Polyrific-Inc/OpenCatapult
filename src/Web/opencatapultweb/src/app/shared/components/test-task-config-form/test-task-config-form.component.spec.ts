import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestTaskConfigFormComponent } from './test-task-config-form.component';

describe('TestTaskConfigFormComponent', () => {
  let component: TestTaskConfigFormComponent;
  let fixture: ComponentFixture<TestTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestTaskConfigFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestTaskConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
