import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmEmailComponent } from './confirm-email.component';
import { CoreModule } from '@app/core';
import { RouterTestingModule } from '@angular/router/testing';
import { MatCardModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ConfirmEmailComponent', () => {
  let component: ConfirmEmailComponent;
  let fixture: ComponentFixture<ConfirmEmailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmEmailComponent ],
      imports: [
        CoreModule,
        RouterTestingModule,
        MatCardModule,
        MatProgressBarModule,
        MatButtonModule,
        HttpClientTestingModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
