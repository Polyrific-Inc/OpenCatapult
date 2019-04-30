import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PushTaskConfigFormComponent } from './push-task-config-form.component';
import { AdditionalConfigFormComponent } from '../additional-config-form/additional-config-form.component';
import { AdditionalConfigFieldComponent } from '../additional-config-field/additional-config-field.component';
import { MatExpansionModule, MatInputModule, MatCheckboxModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { UtilityService } from '@app/shared/services/utility.service';

describe('PushTaskConfigFormComponent', () => {
  let component: PushTaskConfigFormComponent;
  let fixture: ComponentFixture<PushTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PushTaskConfigFormComponent, AdditionalConfigFormComponent, AdditionalConfigFieldComponent ],
      imports: [ MatExpansionModule, ReactiveFormsModule, MatInputModule, MatCheckboxModule ],
      providers: [ UtilityService ]
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
