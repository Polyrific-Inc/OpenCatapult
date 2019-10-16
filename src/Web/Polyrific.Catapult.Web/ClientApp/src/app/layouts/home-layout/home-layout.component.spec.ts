import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeLayoutComponent } from './home-layout.component';
import { HeaderComponent } from '@app/header/header.component';
import { RouterTestingModule } from '@angular/router/testing';
import { MatToolbarModule, MatMenuModule, MatIconModule,
  MatSidenavModule, MatDividerModule, MatProgressBarModule } from '@angular/material';
import { AuthService } from '@app/core/auth/auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@app/core';
import { of } from 'rxjs';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FooterComponent } from '@app/footer/footer.component';
import { ConfigService } from '@app/config/config.service';

describe('HomeLayoutComponent', () => {
  let component: HomeLayoutComponent;
  let fixture: ComponentFixture<HomeLayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        RouterTestingModule,
        HttpClientTestingModule,
        MatToolbarModule,
        SharedModule,
        MatMenuModule,
        MatIconModule,
        MatSidenavModule,
        FlexLayoutModule,
        CoreModule,
        MatDividerModule,
        MatProgressBarModule
      ],
      declarations: [ HomeLayoutComponent, HeaderComponent, FooterComponent ],
      providers: [
        {
          provide: AuthService, useValue: {
            currentUser: of({email: 'test@test.com'}),
            checkRoleAuthorization() {
              return false;
            }
          }
        },
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
    fixture = TestBed.createComponent(HomeLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
