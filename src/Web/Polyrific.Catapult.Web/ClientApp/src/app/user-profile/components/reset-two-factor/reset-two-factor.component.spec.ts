import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetTwoFactorComponent } from './reset-two-factor.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatInputModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';

describe('ResetTwoFactorComponent', () => {
  let component: ResetTwoFactorComponent;
  let fixture: ComponentFixture<ResetTwoFactorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResetTwoFactorComponent ],
      imports: [
        BrowserAnimationsModule,
        RouterTestingModule,
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
    fixture = TestBed.createComponent(ResetTwoFactorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
