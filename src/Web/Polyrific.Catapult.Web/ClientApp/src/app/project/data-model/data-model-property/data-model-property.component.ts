import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DataModelPropertyDto, DataModelDto, DataModelService, AuthorizePolicy } from '@app/core';
import { MatDialog } from '@angular/material';
import {
  DataModelPropertyInfoDialogComponent
} from '../components/data-model-property-info-dialog/data-model-property-info-dialog.component';
import { ConfirmationWithInputDialogComponent, SnackbarService } from '@app/shared';

@Component({
  selector: 'app-data-model-property',
  templateUrl: './data-model-property.component.html',
  styleUrls: ['./data-model-property.component.css']
})
export class DataModelPropertyComponent implements OnInit {
  @Input() properties: DataModelPropertyDto[];
  @Input() dataModel: DataModelDto;
  @Input() dataModels: DataModelDto[];
  @Output() propertiesChanged = new EventEmitter<DataModelDto>();
  authorizePolicy = AuthorizePolicy;
  constructor(
    private dialog: MatDialog,
    private dataModelService: DataModelService,
    private snackbar: SnackbarService
    ) { }

  ngOnInit() {
  }

  onPropertyInfoClick(property: DataModelPropertyDto) {
    const dialogRef = this.dialog.open(DataModelPropertyInfoDialogComponent, {
      data: {
        projectId: this.dataModel.projectId,
        dataModelName: this.dataModel.name,
        relatedDataModels: this.dataModels.filter(m => m.id !== this.dataModel.id),
        ...property
      }
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.propertiesChanged.emit(this.dataModel);
      }
    });
  }

  onPropertyDeleteClick(property: DataModelPropertyDto) {
    const dialogRef = this.dialog.open(ConfirmationWithInputDialogComponent, {
      data: {
        title: 'Confirm Delete Data Model Property',
        confirmationText: `Please enter data model property name (${property.name}) to confirm deletion process:`,
        confirmationMatch: property.name
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.dataModelService.deleteDataModelProperty(this.dataModel.projectId, property.projectDataModelId, property.id)
          .subscribe(() => {
            this.snackbar.open('Property has been deleted');

            this.propertiesChanged.emit(this.dataModel);
          });
      }
    });
  }

}
