import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelComponent } from './data-model.component';

describe('DataModelComponent', () => {
  let component: DataModelComponent;
  let fixture: ComponentFixture<DataModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
