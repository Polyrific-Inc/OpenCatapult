import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelPropertyComponent } from './data-model-property.component';
import { MatListModule, MatDividerModule } from '@angular/material';

describe('DataModelPropertyComponent', () => {
  let component: DataModelPropertyComponent;
  let fixture: ComponentFixture<DataModelPropertyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelPropertyComponent ],
      imports: [
        MatDividerModule,
        MatListModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataModelPropertyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
