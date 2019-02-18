import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdditionalConfigFieldComponent } from './additional-config-field.component';

describe('AdditionalConfigFieldComponent', () => {
  let component: AdditionalConfigFieldComponent;
  let fixture: ComponentFixture<AdditionalConfigFieldComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdditionalConfigFieldComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdditionalConfigFieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
