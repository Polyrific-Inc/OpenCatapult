import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginLayoutComponent } from './login-layout.component';
import { RouterTestingModule } from '@angular/router/testing';
import { MatProgressBarModule, MatToolbarModule } from '@angular/material';
import { FooterComponent } from '@app/footer/footer.component';
import { ConfigService } from '@app/config/config.service';

describe('LoginLayoutComponent', () => {
  let component: LoginLayoutComponent;
  let fixture: ComponentFixture<LoginLayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ RouterTestingModule, MatProgressBarModule, MatToolbarModule ],
      declarations: [ LoginLayoutComponent, FooterComponent ],
      providers: [
        {
          provide: ConfigService, useValue: {
            getConfig: () => ({
              environmentName: 'test'
            })
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
