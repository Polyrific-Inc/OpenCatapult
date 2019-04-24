import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExternalAccountFormComponent } from './external-account-form.component';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { MatInputModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ExternalAccountFormComponent', () => {
  let component: ExternalAccountFormComponent;
  let fixture: ComponentFixture<ExternalAccountFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExternalAccountFormComponent ],
      imports: [ BrowserAnimationsModule, ReactiveFormsModule, MatInputModule ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExternalAccountFormComponent);
    component = fixture.componentInstance;
    component.form = new FormGroup({});
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
