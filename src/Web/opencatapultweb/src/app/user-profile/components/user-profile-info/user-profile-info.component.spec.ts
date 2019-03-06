import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProfileInfoComponent } from './user-profile-info.component';
import { MatInputModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthService } from '@app/core/auth/auth.service';

describe('UserProfileInfoComponent', () => {
  let component: UserProfileInfoComponent;
  let fixture: ComponentFixture<UserProfileInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserProfileInfoComponent ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        MatInputModule,
        ReactiveFormsModule,
        MatProgressBarModule,
        MatButtonModule,
        FlexLayoutModule,
        CoreModule,
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
    fixture = TestBed.createComponent(UserProfileInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
