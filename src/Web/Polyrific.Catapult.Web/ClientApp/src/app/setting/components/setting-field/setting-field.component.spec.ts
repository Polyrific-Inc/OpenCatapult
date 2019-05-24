import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingFieldComponent } from './setting-field.component';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { MatInputModule, MatButtonModule, MatSelectModule, MatCheckboxModule } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('SettingFieldComponent', () => {
  let component: SettingFieldComponent;
  let fixture: ComponentFixture<SettingFieldComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SettingFieldComponent ],
      imports: [
        BrowserAnimationsModule,
        ReactiveFormsModule,
        MatInputModule,
        MatButtonModule,
        MatSelectModule,
        MatCheckboxModule,
        SharedModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SettingFieldComponent);
    component = fixture.componentInstance;

    const fb = new FormBuilder();
    component.applicationSetting = {
      key: 'test',
      label: 'test',
      dataType: 'string',
      allowedValues: [],
      value: 'test'
    };
    component.form = fb.group({
      'test': 'test'
    });
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
