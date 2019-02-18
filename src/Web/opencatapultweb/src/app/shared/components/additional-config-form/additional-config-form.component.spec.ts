import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdditionalConfigFormComponent } from './additional-config-form.component';

describe('AdditionalConfigFormComponent', () => {
  let component: AdditionalConfigFormComponent;
  let fixture: ComponentFixture<AdditionalConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdditionalConfigFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdditionalConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
