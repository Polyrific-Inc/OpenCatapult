import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginWithRecoveryCodeComponent } from './login-with-recovery-code.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressBarModule, MatInputModule, MatCardModule, MatIconModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from '@app/core/auth/auth.service';

describe('LoginWithRecoveryCodeComponent', () => {
  let component: LoginWithRecoveryCodeComponent;
  let fixture: ComponentFixture<LoginWithRecoveryCodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoginWithRecoveryCodeComponent ],
      imports: [
        BrowserAnimationsModule,
        MatProgressBarModule,
        ReactiveFormsModule,
        MatInputModule,
        MatCardModule,
        RouterTestingModule,
        HttpClientTestingModule,
        MatIconModule
      ],
      providers: [ AuthService ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginWithRecoveryCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
