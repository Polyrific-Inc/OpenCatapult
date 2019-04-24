import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalAccountFormComponent } from './external-account-form.component';

describe('ExternalAccountFormComponent', () => {
  let component: ExternalAccountFormComponent;
  let fixture: ComponentFixture<ExternalAccountFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExternalAccountFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExternalAccountFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
