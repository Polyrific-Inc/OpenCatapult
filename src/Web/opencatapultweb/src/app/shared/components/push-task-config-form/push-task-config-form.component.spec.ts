import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PushTaskConfigFormComponent } from './push-task-config-form.component';

describe('PushTaskConfigFormComponent', () => {
  let component: PushTaskConfigFormComponent;
  let fixture: ComponentFixture<PushTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PushTaskConfigFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PushTaskConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
