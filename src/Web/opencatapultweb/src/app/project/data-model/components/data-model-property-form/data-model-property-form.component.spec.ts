import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelPropertyFormComponent } from './data-model-property-form.component';

describe('DataModelPropertyFormComponent', () => {
  let component: DataModelPropertyFormComponent;
  let fixture: ComponentFixture<DataModelPropertyFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelPropertyFormComponent ]
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
