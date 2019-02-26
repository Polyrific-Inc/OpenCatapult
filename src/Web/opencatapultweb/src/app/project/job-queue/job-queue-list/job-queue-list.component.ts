import { Component, OnInit, Input, ViewChild, OnChanges, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { JobDto } from '@app/core';
import { MatPaginator, MatTableDataSource, MatSort } from '@angular/material';

@Component({
  selector: 'app-job-queue-list',
  templateUrl: './job-queue-list.component.html',
  styleUrls: ['./job-queue-list.component.css']
})
export class JobQueueListComponent implements OnInit {
  @Input() jobList: JobDto[];
  @Input() usePaging: boolean;
  paginator: MatPaginator;
  sort: MatSort;
  dataSource: MatTableDataSource<JobDto>;

  displayedColumns: string[] = ['jobDefinitionName', 'status', 'created', 'actions'];

  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.dataSource.sort = this.sort;
  }

  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.dataSource.paginator = this.paginator;
  }

  constructor() {
   }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<JobDto>(this.jobList);
  }

  onInfoClick(job: JobDto) {

  }

  onLogClick(job: JobDto) {

  }
}
