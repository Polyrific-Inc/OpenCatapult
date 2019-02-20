import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateTaskConfigFormComponent } from './generate-task-config-form.component';
import { AdditionalConfigFormComponent } from '../additional-config-form/additional-config-form.component';
import { AdditionalConfigFieldComponent } from '../additional-config-field/additional-config-field.component';
import { MatExpansionModule, MatInputModule, MatCheckboxModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';

describe('GenerateTaskConfigFormComponent', () => {
  let component: GenerateTaskConfigFormComponent;
  let fixture: ComponentFixture<GenerateTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateTaskConfigFormComponent, AdditionalConfigFormComponent, AdditionalConfigFieldComponent ],
      imports: [ MatExpansionModule, ReactiveFormsModule, MatInputModule, MatCheckboxModule ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateTaskConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
