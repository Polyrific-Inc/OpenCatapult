import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderComponent } from './header.component';
import { MatToolbarModule, MatMenuModule, MatIconModule, MatSidenavModule, MatDividerModule } from '@angular/material';
import { AuthService } from '@app/core/auth/auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@app/core';
import { of } from 'rxjs';
import { FlexLayoutModule } from '@angular/flex-layout';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HeaderComponent ],
      imports: [
        MatToolbarModule,
        HttpClientTestingModule,
        RouterTestingModule,
        SharedModule,
        CoreModule,
        MatMenuModule,
        MatMenuModule,
        MatSidenavModule,
        FlexLayoutModule,
        MatIconModule,
        MatDividerModule
      ],
      providers: [
        {
          provide: AuthService, useValue: {
            currentUser: of({email: 'test@test.com'}),
            checkRoleAuthorization() {
              return false;
            }
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
