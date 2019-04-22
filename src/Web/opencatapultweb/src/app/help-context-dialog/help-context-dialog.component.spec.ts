import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpContextDialogComponent } from './help-context-dialog.component';

describe('HelpContextDialogComponent', () => {
  let component: HelpContextDialogComponent;
  let fixture: ComponentFixture<HelpContextDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HelpContextDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HelpContextDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
