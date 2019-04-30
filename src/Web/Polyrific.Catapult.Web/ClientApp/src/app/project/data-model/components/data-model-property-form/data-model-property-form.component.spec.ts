import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelPropertyFormComponent } from './data-model-property-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatCheckboxModule, MatSelectModule, MatDividerModule } from '@angular/material';
import { CoreModule } from '@app/core';

describe('DataModelPropertyFormComponent', () => {
  let component: DataModelPropertyFormComponent;
  let fixture: ComponentFixture<DataModelPropertyFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelPropertyFormComponent ],
      imports: [
        BrowserAnimationsModule,
        ReactiveFormsModule,
        MatInputModule,
        MatCheckboxModule,
        MatSelectModule,
        MatDividerModule,
        CoreModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataModelPropertyFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
