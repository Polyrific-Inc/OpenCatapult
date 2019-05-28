import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EnableTwoFactorComponent } from './enable-two-factor.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatInputModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { QRCodeModule } from 'angularx-qrcode';
import { ShowRecoveryTwoFactorComponent } from '../show-recovery-two-factor/show-recovery-two-factor.component';

describe('EnableTwoFactorComponent', () => {
  let component: EnableTwoFactorComponent;
  let fixture: ComponentFixture<EnableTwoFactorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EnableTwoFactorComponent, ShowRecoveryTwoFactorComponent ],
      imports: [
        BrowserAnimationsModule,
        RouterTestingModule,
        HttpClientTestingModule,
        QRCodeModule,
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
    fixture = TestBed.createComponent(EnableTwoFactorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
