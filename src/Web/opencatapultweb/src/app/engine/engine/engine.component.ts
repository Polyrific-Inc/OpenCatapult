import { Component, OnInit, AfterViewInit } from '@angular/core';
import { EngineService, EngineStatus, EngineDto } from '@app/core';
import { ConfirmationDialogComponent, SnackbarService } from '@app/shared';
import { MatDialog } from '@angular/material';
import { FormControl, FormBuilder } from '@angular/forms';
import { EngineTokenDialogComponent } from '../components/engine-token-dialog/engine-token-dialog.component';
import { EngineRegisterDialogComponent } from '../components/engine-register-dialog/engine-register-dialog.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-engine',
  templateUrl: './engine.component.html',
  styleUrls: ['./engine.component.css']
})
export class EngineComponent implements OnInit, AfterViewInit {
  engines: EngineDto[];
  roleId = 0;
  statusFilter: FormControl;
  engineStatus = [
    {text: 'Active', value: EngineStatus.active},
    {text: 'Suspended', value: EngineStatus.suspended},
    {text: 'Running', value: EngineStatus.running}
  ];

  displayedColumns: string[] = ['name', 'version', 'lastSeen', 'actions'];

  constructor(
    private fb: FormBuilder,
    private engineService: EngineService,
    private dialog: MatDialog,
    private snackbar: SnackbarService,
    private route: ActivatedRoute
    ) { }

  ngOnInit() {
    this.statusFilter = this.fb.control(EngineStatus.active);
    this.getEngines();
  }

  ngAfterViewInit() {
    this.route.queryParams.subscribe(data => {
      if (data.newEngine) {
        setTimeout(() => {
          this.onRegisterEngineClick();
        }, 0);
      }
    });
  }

  getEngines() {
    this.engineService.getEngines(this.statusFilter.value)
      .subscribe(data => this.engines = data);
  }

  onTokenClick(engine: EngineDto) {

    const dialogRef = this.dialog.open(EngineTokenDialogComponent, {
      data: engine
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getEngines();
      }
    });
  }

  onActivateClick(engine: EngineDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Activate Engine',
        confirmationText: `Are you sure you want to activate engine ${engine.name}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.engineService.activate(engine.id)
          .subscribe(() => {
            this.snackbar.open('Engine has been activated');

            this.getEngines();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onSuspendClick(engine: EngineDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Suspend Engine',
        confirmationText: `Are you sure you want to suspend engine ${engine.name}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.engineService.suspend(engine.id)
          .subscribe(() => {
            this.snackbar.open('Engine has been suspended');

            this.getEngines();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onDeleteClick(engine: EngineDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Delete Engine',
        confirmationText: `Are you sure you want to remove engine ${engine.name}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.engineService.remove(engine.id)
          .subscribe(() => {
            this.snackbar.open('Engine has been removed');

            this.getEngines();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onRegisterEngineClick() {
    const dialogRef = this.dialog.open(EngineRegisterDialogComponent);

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getEngines();
      }
    });
  }

  onStatusFilterChanged() {
    this.getEngines();
  }

}
