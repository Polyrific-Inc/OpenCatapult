import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeLayoutComponent } from './home-layout.component';
import { HeaderComponent } from '@app/header/header.component';
import { RouterTestingModule } from '@angular/router/testing';
import { MatToolbarModule } from '@angular/material';
import { AuthService } from '@app/core/auth/auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('HomeLayoutComponent', () => {
  let component: HomeLayoutComponent;
  let fixture: ComponentFixture<HomeLayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ RouterTestingModule, HttpClientTestingModule, MatToolbarModule ],
      declarations: [ HomeLayoutComponent, HeaderComponent ],
      providers: [ AuthService ]
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
