import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginWithTwofaComponent } from './login-with-twofa.component';

describe('LoginWithTwofaComponent', () => {
  let component: LoginWithTwofaComponent;
  let fixture: ComponentFixture<LoginWithTwofaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoginWithTwofaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginWithTwofaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
