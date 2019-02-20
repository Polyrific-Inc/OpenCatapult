import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MergeTaskConfigFormComponent } from './merge-task-config-form.component';
import { AdditionalConfigFormComponent } from '../additional-config-form/additional-config-form.component';
import { AdditionalConfigFieldComponent } from '../additional-config-field/additional-config-field.component';
import { MatExpansionModule, MatInputModule, MatCheckboxModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';

describe('MergeTaskConfigFormComponent', () => {
  let component: MergeTaskConfigFormComponent;
  let fixture: ComponentFixture<MergeTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MergeTaskConfigFormComponent, AdditionalConfigFormComponent, AdditionalConfigFieldComponent ],
      imports: [ MatExpansionModule, ReactiveFormsModule, MatInputModule, MatCheckboxModule ]
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
