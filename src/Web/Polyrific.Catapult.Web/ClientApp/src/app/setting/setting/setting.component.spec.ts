import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingComponent } from './setting.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatButtonModule, MatSelectModule, MatCheckboxModule } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { SettingFieldComponent } from '../components/setting-field/setting-field.component';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('SettingComponent', () => {
  let component: SettingComponent;
  let fixture: ComponentFixture<SettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SettingComponent, SettingFieldComponent ],
      imports: [
        HttpClientTestingModule,
        ReactiveFormsModule,
        MatInputModule,
        MatButtonModule,
        MatSelectModule,
        MatCheckboxModule,
        SharedModule.forRoot(),
        CoreModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
