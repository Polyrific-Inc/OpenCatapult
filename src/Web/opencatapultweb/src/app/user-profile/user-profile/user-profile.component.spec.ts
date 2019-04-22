import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProfileComponent } from './user-profile.component';
import { MatTabsModule, MatInputModule, MatProgressBarModule, MatButtonModule, MatIconModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { UserProfileInfoComponent } from '../components/user-profile-info/user-profile-info.component';
import { UserProfilePasswordComponent } from '../components/user-profile-password/user-profile-password.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from '@app/core/auth/auth.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('UserProfileComponent', () => {
  let component: UserProfileComponent;
  let fixture: ComponentFixture<UserProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserProfileComponent, UserProfileInfoComponent, UserProfilePasswordComponent ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        MatTabsModule,
        MatInputModule,
        ReactiveFormsModule,
        MatProgressBarModule,
        MatButtonModule,
        FlexLayoutModule,
        CoreModule,
        MatIconModule,
        SharedModule.forRoot()
      ],
      providers: [
        {
          provide: AuthService, useValue: {
            currentUserValue: {}
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
