import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelComponent } from './data-model.component';
import { MatExpansionModule, MatListModule } from '@angular/material';
import { DataModelPropertyComponent } from '../data-model-property/data-model-property.component';

describe('DataModelComponent', () => {
  let component: DataModelComponent;
  let fixture: ComponentFixture<DataModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelComponent, DataModelPropertyComponent ],
      imports: [
        MatExpansionModule,
        MatListModule
      ]
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
