import { Component, OnInit } from '@angular/core';
import { ProjectDto } from '@app/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project-deleting',
  templateUrl: './project-deleting.component.html',
  styleUrls: ['./project-deleting.component.css']
})
export class ProjectDeletingComponent implements OnInit {
  project: ProjectDto;

  constructor(
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.getProject();
  }

  getProject(): void {
    this.route.data.subscribe((data: {project: ProjectDto}) => {
      this.project = data.project;
    });
  }

}
