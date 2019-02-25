import { Component, OnInit } from '@angular/core';
import { DataModelDto, DataModelService } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatCheckboxChange } from '@angular/material';
import { DataModelNewDialogComponent } from '../components/data-model-new-dialog/data-model-new-dialog.component';
import { DataModelInfoDialogComponent } from '../components/data-model-info-dialog/data-model-info-dialog.component';
import { ConfirmationWithInputDialogComponent, SnackbarService } from '@app/shared';
import { DataModelPropertyNewDialogComponent } from '../components/data-model-property-new-dialog/data-model-property-new-dialog.component';

interface DataModelViewModel extends DataModelDto {
  selected: boolean;
}

@Component({
  selector: 'app-data-model',
  templateUrl: './data-model.component.html',
  styleUrls: ['./data-model.component.css']
})
export class DataModelComponent implements OnInit {
  dataModels: DataModelViewModel[];
  projectId: number;

  constructor(
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private dataModelService: DataModelService,
    private snackbar: SnackbarService
  ) { }

  ngOnInit() {
    this.projectId = +this.route.parent.parent.snapshot.params.id;
    this.getDataModels();
  }

  getDataModels() {
    this.dataModelService.getDataModels(this.projectId, true)
      .subscribe(data => {
        this.dataModels = data.map(item => ({
          selected: false,
          ...item
        }));
      });
  }

  getDataModelProperty(dataModel: DataModelDto) {
    this.dataModelService.getDataModelProperties(dataModel.projectId, dataModel.id)
      .subscribe(data => {
        this.dataModels = this.dataModels.map(model => {
          if (model.id === dataModel.id) {
            model.properties = data;
          }

          return model;
        });
      });
  }

  onNewDataModelClick() {
    const dialogRef = this.dialog.open(DataModelNewDialogComponent, {
      data: {
        projectId: this.projectId
      }
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getDataModels();
      }
    });
  }

  isModelsSelected() {
    return this.dataModels && this.dataModels.some(m => m.selected);
  }

  onBulkDeleteClick() {
    const deletingModels = this.dataModels.filter(m => m.selected);
    const deletingModelsString = deletingModels.reduce((agg, item, idx) => {
      agg += `  - ${item.name}`;

      if (idx + 1 < deletingModels.length) {
        agg += '\n';
      }

      return agg;
    }, '');

    const dialogRef = this.dialog.open(ConfirmationWithInputDialogComponent, {
      data: {
        title: 'Confirm Delete Data Model',
        confirmationText: `Please enter the text "yes" to delete the following models:`,
        subText: deletingModelsString,
        confirmationMatch: 'yes'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.dataModelService.deleteDataModels(this.projectId, deletingModels.map(m => m.id))
          .subscribe(() => {
            this.snackbar.open('Data models has been deleted');

            this.getDataModels();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onModelInfoClick(model: DataModelDto) {
    const dialogRef = this.dialog.open(DataModelInfoDialogComponent, {
      data: model
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getDataModels();
      }
    });
   }

  onModelDeleteClick(model: DataModelDto) {
    const dialogRef = this.dialog.open(ConfirmationWithInputDialogComponent, {
      data: {
        title: 'Confirm Delete Data Model',
        confirmationText: 'Please enter data model name to confirm deletion process:',
        confirmationMatch: model.name
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.dataModelService.deleteDataModel(model.projectId, model.id)
          .subscribe(() => {
            this.snackbar.open('Project has been deleted');

            this.getDataModels();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
   }

  onModelPropertyAddClick(model: DataModelDto) {
    const dialogRef = this.dialog.open(DataModelPropertyNewDialogComponent, {
      data: {
        projectId: this.projectId,
        modelId: model.id,
        modelName: model.name,
        relatedDataModels: this.dataModels.filter(m => m.id !== model.id)
      }
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getDataModelProperty(model);
      }
    });
  }

  onCheckboxAllChanged(value: MatCheckboxChange) {
    this.dataModels.forEach(m => m.selected = value.checked);
  }

  onPropertiesUpdated(model: DataModelDto) {
    this.getDataModelProperty(model);
  }
}
