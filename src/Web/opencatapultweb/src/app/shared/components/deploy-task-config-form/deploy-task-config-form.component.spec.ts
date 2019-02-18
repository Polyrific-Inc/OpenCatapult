import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeployTaskConfigFormComponent } from './deploy-task-config-form.component';

describe('DeployTaskConfigFormComponent', () => {
  let component: DeployTaskConfigFormComponent;
  let fixture: ComponentFixture<DeployTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeployTaskConfigFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeployTaskConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
