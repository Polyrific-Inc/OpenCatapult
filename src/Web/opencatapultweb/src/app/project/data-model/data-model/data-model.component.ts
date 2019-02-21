import { Component, OnInit } from '@angular/core';
import { DataModelDto, DataModelService } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatCheckboxChange } from '@angular/material';
import { DataModelNewDialogComponent } from '../components/data-model-new-dialog/data-model-new-dialog.component';
import { DataModelInfoDialogComponent } from '../components/data-model-info-dialog/data-model-info-dialog.component';
import { ConfirmationWithInputDialogComponent, SnackbarService } from '@app/shared';

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
    const dialogRef = this.dialog.open(ConfirmationWithInputDialogComponent, {
      data: {
        title: 'Confirm Delete Data Model',
        confirmationText: 'Please enter the text "yes" to confirm deletion process:',
        confirmationMatch: 'yes'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        // TODO: delete the models
        // this.dataModelService.deleteDataModel(this.projectId, model.id)
        //   .subscribe(() => {
        //     this.snackbar.open('Project has been deleted');

        //     this.getDataModels();
        //   });
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
          });
      }
    });
   }

  onModelPropertyAddClick(model: DataModelDto) {  }

  onCheckboxAllChanged(value: MatCheckboxChange) {
    this.dataModels.forEach(m => m.selected = value.checked);
  }
}
