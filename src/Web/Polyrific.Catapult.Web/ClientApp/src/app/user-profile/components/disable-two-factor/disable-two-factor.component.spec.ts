import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DisableTwoFactorComponent } from './disable-two-factor.component';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatInputModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';

describe('DisableTwoFactorComponent', () => {
  let component: DisableTwoFactorComponent;
  let fixture: ComponentFixture<DisableTwoFactorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DisableTwoFactorComponent ],
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
    fixture = TestBed.createComponent(DisableTwoFactorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
