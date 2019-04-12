import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { ProviderType } from '@app/core/enums/provider-type';
import { TaskProviderDto, TaskProviderService } from '@app/core';
import { FormControl, FormBuilder } from '@angular/forms';
import { MatDialog, MatAutocompleteSelectedEvent, MatAutocomplete, MatIconRegistry } from '@angular/material';
import { SnackbarService, ConfirmationDialogComponent } from '@app/shared';
import { TaskProviderRegisterDialogComponent } from '../components/task-provider-register-dialog/task-provider-register-dialog.component';
import { TaskProviderInfoDialogComponent } from '../components/task-provider-info-dialog/task-provider-info-dialog.component';
import { ActivatedRoute } from '@angular/router';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-task-provider',
  templateUrl: './task-provider.component.html',
  styleUrls: ['./task-provider.component.css']
})
export class TaskProviderComponent implements OnInit, AfterViewInit {
  taskProviders: TaskProviderDto[];
  filteredTaskProviders: TaskProviderDto[];
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

  separatorKeysCodes: number[] = [ENTER, COMMA];
  selectedTags: string[] = [];
  allTags: string[];
  autocompleteTags: Observable<string[]>;
  tagControl = new FormControl();

  displayedColumns: string[] = [
    'thumbnail',
    'displayName',
    'type',
    'author',
    'version',
    'created',
    'updated',
    'requiredServices',
    'actions'
  ];

  @ViewChild('tagInput') tagInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  constructor(
    private fb: FormBuilder,
    private taskProviderService: TaskProviderService,
    private dialog: MatDialog,
    private snackbar: SnackbarService,
    private route: ActivatedRoute,
    iconRegistry: MatIconRegistry,
    sanitizer: DomSanitizer
    ) {
      this.autocompleteTags = this.tagControl.valueChanges.pipe(
          startWith(null),
          map((tag: string | null) => tag ? this._filterAutoComplete(tag) : this.allTags));

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
        this.filteredTaskProviders = data;
        this.allTags = data.map(t => t.tags).reduce((all, tags) => all.concat(tags));
        this.allTags = Array.from(new Set(this.allTags));
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

  removeFilterTag(tag: string): void {
    const index = this.selectedTags.indexOf(tag);

    if (index >= 0) {
      this.selectedTags.splice(index, 1);
      this._filterTable();
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.selectedTags.push(event.option.viewValue);
    this.tagInput.nativeElement.value = '';
    this.tagControl.setValue(null);
    this._filterTable();
  }

  private _filterAutoComplete(value: string): string[] {
    const tagValue = value.toLowerCase();

    return this.allTags.filter(tag => tag.toLowerCase().indexOf(tagValue) === 0);
  }

  private _filterTable() {
    this.filteredTaskProviders = this.selectedTags && this.selectedTags.length > 0 ?
      this.taskProviders.filter(t => t.tags.some(tag => this.selectedTags.includes(tag))) :
      this.taskProviders;
  }

}
