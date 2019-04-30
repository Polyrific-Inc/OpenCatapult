import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelFormComponent } from './data-model-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatCheckboxModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from '@app/core';

describe('DataModelFormComponent', () => {
  let component: DataModelFormComponent;
  let fixture: ComponentFixture<DataModelFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelFormComponent ],
      imports: [
        BrowserAnimationsModule,
        ReactiveFormsModule,
        MatInputModule,
        MatCheckboxModule,
        CoreModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataModelFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
