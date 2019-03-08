import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelPropertyComponent } from './data-model-property.component';
import { MatButtonModule, MatListModule, MatIconModule, MatDialogModule, MatSnackBarModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SnackbarService } from '@app/shared';
import { SharedModule } from '@app/shared/shared.module';

describe('DataModelPropertyComponent', () => {
  let component: DataModelPropertyComponent;
  let fixture: ComponentFixture<DataModelPropertyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelPropertyComponent ],
      imports: [
        MatButtonModule,
        MatListModule,
        FlexLayoutModule,
        MatIconModule,
        CoreModule,
        MatDialogModule,
        HttpClientTestingModule,
        MatSnackBarModule,
        SharedModule.forRoot()
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataModelPropertyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
