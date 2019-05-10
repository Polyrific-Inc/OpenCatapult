import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeployDbTaskConfigFormComponent } from './deploy-db-task-config-form.component';
import { AdditionalConfigFormComponent } from '../additional-config-form/additional-config-form.component';
import { AdditionalConfigFieldComponent } from '../additional-config-field/additional-config-field.component';
import { MatExpansionModule, MatInputModule, MatCheckboxModule, MatSelectModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';

describe('DeployDbTaskConfigFormComponent', () => {
  let component: DeployDbTaskConfigFormComponent;
  let fixture: ComponentFixture<DeployDbTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeployDbTaskConfigFormComponent, AdditionalConfigFormComponent, AdditionalConfigFieldComponent ],
      imports: [ MatExpansionModule, ReactiveFormsModule, MatInputModule, MatCheckboxModule, MatSelectModule ]
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
