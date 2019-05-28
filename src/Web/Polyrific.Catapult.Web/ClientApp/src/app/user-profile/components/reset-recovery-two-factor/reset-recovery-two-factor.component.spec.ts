import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetRecoveryTwoFactorComponent } from './reset-recovery-two-factor.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatInputModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { ShowRecoveryTwoFactorComponent } from '../show-recovery-two-factor/show-recovery-two-factor.component';

describe('ResetRecoveryTwoFactorComponent', () => {
  let component: ResetRecoveryTwoFactorComponent;
  let fixture: ComponentFixture<ResetRecoveryTwoFactorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResetRecoveryTwoFactorComponent, ShowRecoveryTwoFactorComponent ],
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
    fixture = TestBed.createComponent(ResetRecoveryTwoFactorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
