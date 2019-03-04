import { Component, OnInit } from '@angular/core';
import { ExternalServiceDto, ExternalServiceService } from '@app/core';
import { MatDialog } from '@angular/material';
import { SnackbarService, ConfirmationDialogComponent } from '@app/shared';
import { ExternalServiceInfoDialogComponent } from '../components/external-service-info-dialog/external-service-info-dialog.component';
import { ExternalServiceNewDialogComponent } from '../components/external-service-new-dialog/external-service-new-dialog.component';

@Component({
  selector: 'app-external-service',
  templateUrl: './external-service.component.html',
  styleUrls: ['./external-service.component.css']
})
export class ExternalServiceComponent implements OnInit {
  externalServices: ExternalServiceDto[];
  projectId: number;
  roleId = 0;

  displayedColumns: string[] = ['name', 'description', 'externalServiceTypeName', 'actions'];

  constructor(
    private externalServiceService: ExternalServiceService,
    private dialog: MatDialog,
    private snackbar: SnackbarService
    ) { }

  ngOnInit() {
    this.getExternalServices();
  }

  getExternalServices() {
    this.externalServiceService.getExternalServices()
      .subscribe(data => this.externalServices = data);
  }

  onInfoClick(externalService: ExternalServiceDto) {

    const dialogRef = this.dialog.open(ExternalServiceInfoDialogComponent, {
      data: externalService
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getExternalServices();
      }
    });
  }

  onDeleteClick(externalService: ExternalServiceDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Delete External Service',
        confirmationText: `Are you sure you want to remove external service ${externalService.name}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.externalServiceService.deleteExternalService(externalService.id)
          .subscribe(() => {
            this.snackbar.open('External service has been deleted');

            this.getExternalServices();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onNewExternalServiceClick() {
    const dialogRef = this.dialog.open(ExternalServiceNewDialogComponent, {
      data: {
        projectId: this.projectId
      }
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getExternalServices();
      }
    });
  }

}
