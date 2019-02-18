import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeployDbTaskConfigFormComponent } from './deploy-db-task-config-form.component';

describe('DeployDbTaskConfigFormComponent', () => {
  let component: DeployDbTaskConfigFormComponent;
  let fixture: ComponentFixture<DeployDbTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeployDbTaskConfigFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeployDbTaskConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
