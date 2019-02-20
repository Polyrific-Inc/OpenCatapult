import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectComponent } from './project.component';
import { CoreModule } from '@app/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatExpansionModule, MatListModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ProjectComponent', () => {
  let component: ProjectComponent;
  let fixture: ComponentFixture<ProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectComponent ],
      imports: [
        BrowserAnimationsModule,
        RouterTestingModule,
        HttpClientTestingModule,
        CoreModule,
        MatExpansionModule,
        MatListModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
