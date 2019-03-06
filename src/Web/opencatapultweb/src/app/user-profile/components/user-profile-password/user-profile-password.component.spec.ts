import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProfilePasswordComponent } from './user-profile-password.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatInputModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CoreModule } from '@app/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('UserProfilePasswordComponent', () => {
  let component: UserProfilePasswordComponent;
  let fixture: ComponentFixture<UserProfilePasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserProfilePasswordComponent ],
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
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserProfilePasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
