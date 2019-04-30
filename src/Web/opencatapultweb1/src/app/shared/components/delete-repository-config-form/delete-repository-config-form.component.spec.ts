import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteRepositoryConfigFormComponent } from './delete-repository-config-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatCheckboxModule } from '@angular/material';

describe('DeleteRepositoryConfigFormComponent', () => {
  let component: DeleteRepositoryConfigFormComponent;
  let fixture: ComponentFixture<DeleteRepositoryConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeleteRepositoryConfigFormComponent ],
      imports: [ ReactiveFormsModule, MatInputModule, MatCheckboxModule ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteRepositoryConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
