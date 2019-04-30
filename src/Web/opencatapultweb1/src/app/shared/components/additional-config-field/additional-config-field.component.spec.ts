import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdditionalConfigFieldComponent } from './additional-config-field.component';
import { ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';
import { MatInputModule, MatCheckboxModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('AdditionalConfigFieldComponent', () => {
  let component: AdditionalConfigFieldComponent;
  let fixture: ComponentFixture<AdditionalConfigFieldComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdditionalConfigFieldComponent ],
      imports: [
        BrowserAnimationsModule,
        ReactiveFormsModule,
        MatInputModule,
        MatCheckboxModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdditionalConfigFieldComponent);

    const fb = new FormBuilder();
    component = fixture.componentInstance;
    component.additionalConfig = {
      type: 'string',
      name: 'name',
      isSecret: false,
      isInputMasked: false,
      isRequired: true,
      label: 'name',
      allowedValues: [],
      hint: null
    };
    component.form = fb.group(component.additionalConfig);

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
