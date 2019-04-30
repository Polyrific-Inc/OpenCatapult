import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelComponent } from './data-model.component';
import { MatButtonModule, MatExpansionModule, MatListModule,
  MatIconModule, MatCheckboxModule, MatDialogModule, MatSnackBarModule, MatProgressSpinnerModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { DataModelPropertyComponent } from '../data-model-property/data-model-property.component';
import { RouterTestingModule } from '@angular/router/testing';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { SnackbarService } from '@app/shared';
import { SharedModule } from '@app/shared/shared.module';
import { AuthService } from '@app/core/auth/auth.service';

describe('DataModelComponent', () => {
  let component: DataModelComponent;
  let fixture: ComponentFixture<DataModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelComponent, DataModelPropertyComponent ],
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        MatButtonModule,
        MatExpansionModule,
        MatListModule,
        FlexLayoutModule,
        MatIconModule,
        CoreModule,
        MatCheckboxModule,
        FormsModule,
        MatDialogModule,
        MatSnackBarModule,
        MatProgressSpinnerModule,
        SharedModule.forRoot()
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            parent: {
              parent: {
                snapshot: { params: of({ id: 1}) }
              }
            }
          }
        },
        {
          provide: AuthService, useValue: {
            currentUserValue: {
              role: 'Administrator'
            },
            checkRoleAuthorization: function(test, test2) {

            }
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
