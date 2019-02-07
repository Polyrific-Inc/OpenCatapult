import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EngineComponent } from './engine.component';

describe('EngineComponent', () => {
  let component: EngineComponent;
  let fixture: ComponentFixture<EngineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EngineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EngineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
