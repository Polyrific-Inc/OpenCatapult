import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalServiceComponent } from './external-service.component';

describe('ExternalServiceComponent', () => {
  let component: ExternalServiceComponent;
  let fixture: ComponentFixture<ExternalServiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExternalServiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExternalServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
