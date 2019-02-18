import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MergeTaskConfigFormComponent } from './merge-task-config-form.component';

describe('MergeTaskConfigFormComponent', () => {
  let component: MergeTaskConfigFormComponent;
  let fixture: ComponentFixture<MergeTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MergeTaskConfigFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MergeTaskConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
