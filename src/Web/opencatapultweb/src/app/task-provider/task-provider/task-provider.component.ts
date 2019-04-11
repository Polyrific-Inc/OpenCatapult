import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ProviderType } from '@app/core/enums/provider-type';
import { TaskProviderDto, TaskProviderService } from '@app/core';
import { FormControl, FormBuilder } from '@angular/forms';
import { MatDialog, MatIconRegistry } from '@angular/material';
import { SnackbarService, ConfirmationDialogComponent } from '@app/shared';
import { TaskProviderRegisterDialogComponent } from '../components/task-provider-register-dialog/task-provider-register-dialog.component';
import { TaskProviderInfoDialogComponent } from '../components/task-provider-info-dialog/task-provider-info-dialog.component';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-task-provider',
  templateUrl: './task-provider.component.html',
  styleUrls: ['./task-provider.component.css']
})
export class TaskProviderComponent implements OnInit, AfterViewInit {
  taskProviders: TaskProviderDto[];
  roleId = 0;
  taskProviderTypeFilter: FormControl;
  taskProviderTypes = [
    {text: 'All', value: 'all'},
    {text: 'Build Provider', value: ProviderType.BuildProvider},
    {text: 'Hosting Provider', value: ProviderType.HostingProvider},
    {text: 'Generator Provider', value: ProviderType.GeneratorProvider},
    {text: 'Repository Provider', value: ProviderType.RepositoryProvider},
    {text: 'Database Provider', value: ProviderType.DatabaseProvider},
    {text: 'Storage Provider', value: ProviderType.StorageProvider},
    {text: 'Test Provider', value: ProviderType.TestProvider}
  ];
  loading: boolean;

  displayedColumns: string[] = ['name', 'type', 'author', 'version', 'registrationDate', 'requiredServices', 'actions'];

  constructor(
    private fb: FormBuilder,
    private taskProviderService: TaskProviderService,
    private dialog: MatDialog,
    private snackbar: SnackbarService,
    private route: ActivatedRoute,
    iconRegistry: MatIconRegistry,
    sanitizer: DomSanitizer
    ) {
      iconRegistry.addSvgIcon(ProviderType.GeneratorProvider,
        sanitizer.bypassSecurityTrustResourceUrl('assets/img/task-provider-type/codegenerator.svg'));

      iconRegistry.addSvgIcon(ProviderType.RepositoryProvider,
        sanitizer.bypassSecurityTrustResourceUrl('assets/img/task-provider-type/repository.svg'));

      iconRegistry.addSvgIcon(ProviderType.BuildProvider,
        sanitizer.bypassSecurityTrustResourceUrl('assets/img/task-provider-type/build.svg'));

      iconRegistry.addSvgIcon(ProviderType.TestProvider,
        sanitizer.bypassSecurityTrustResourceUrl('assets/img/task-provider-type/test.svg'));

      iconRegistry.addSvgIcon(ProviderType.DatabaseProvider,
        sanitizer.bypassSecurityTrustResourceUrl('assets/img/task-provider-type/database.svg'));

      iconRegistry.addSvgIcon(ProviderType.HostingProvider,
        sanitizer.bypassSecurityTrustResourceUrl('assets/img/task-provider-type/deploy.svg'));

      iconRegistry.addSvgIcon(ProviderType.GenericTaskProvider,
        sanitizer.bypassSecurityTrustResourceUrl('assets/img/task-provider-type/generic.svg'));

      iconRegistry.addSvgIcon(ProviderType.StorageProvider,
        sanitizer.bypassSecurityTrustResourceUrl('assets/img/task-provider-type/storage.svg'));
    }

  ngOnInit() {
    this.taskProviderTypeFilter = this.fb.control('all');
    this.getTaskProviders();
  }

  ngAfterViewInit() {
    this.route.queryParams.subscribe(data => {
      if (data.newProvider) {
        setTimeout(() => {
          this.onRegisterTaskProviderClick();
        }, 0);
      }
    });
  }

  getTaskProviders() {
    this.loading = true;
    this.taskProviderService.getTaskProviders(this.taskProviderTypeFilter.value)
      .subscribe(data => {
        this.taskProviders = data;
        this.loading = false;
      });
  }

  onDeleteClick(taskProvider: TaskProviderDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Delete Task Provider',
        confirmationText: `Are you sure you want to remove taskProvider ${taskProvider.name}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.taskProviderService.deleteTaskProvider(taskProvider.id)
          .subscribe(() => {
            this.snackbar.open('Task Provider has been removed');

            this.getTaskProviders();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onRegisterTaskProviderClick() {
    const dialogRef = this.dialog.open(TaskProviderRegisterDialogComponent);

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getTaskProviders();
      }
    });
  }

  onInfoClick(taskProvider: TaskProviderDto) {
    this.dialog.open(TaskProviderInfoDialogComponent, {
      data: taskProvider
    });
  }

  onTypeChanged() {
    this.getTaskProviders();
  }

}
