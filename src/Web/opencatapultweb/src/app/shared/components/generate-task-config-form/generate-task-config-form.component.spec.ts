import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateTaskConfigFormComponent } from './generate-task-config-form.component';

describe('GenerateTaskConfigFormComponent', () => {
  let component: GenerateTaskConfigFormComponent;
  let fixture: ComponentFixture<GenerateTaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateTaskConfigFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateTaskConfigFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
