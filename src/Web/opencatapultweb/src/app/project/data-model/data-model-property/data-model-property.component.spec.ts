import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelPropertyComponent } from './data-model-property.component';
import { MatButtonModule, MatListModule, MatIconModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';

describe('DataModelPropertyComponent', () => {
  let component: DataModelPropertyComponent;
  let fixture: ComponentFixture<DataModelPropertyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelPropertyComponent ],
      imports: [
        MatButtonModule,
        MatListModule,
        FlexLayoutModule,
        MatIconModule,
        CoreModule
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
