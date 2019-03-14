import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeLayoutComponent } from './home-layout.component';
import { HeaderComponent } from '@app/header/header.component';
import { RouterTestingModule } from '@angular/router/testing';
import { MatToolbarModule, MatMenuModule, MatIconModule } from '@angular/material';
import { AuthService } from '@app/core/auth/auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@app/core';
import { of } from 'rxjs';

describe('HomeLayoutComponent', () => {
  let component: HomeLayoutComponent;
  let fixture: ComponentFixture<HomeLayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        MatToolbarModule,
        SharedModule,
        MatMenuModule,
        MatIconModule,
        CoreModule
      ],
      declarations: [ HomeLayoutComponent, HeaderComponent ],
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
    fixture = TestBed.createComponent(HomeLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
