import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map } from 'rxjs/operators';
import { ProjectService, ProjectStatusFilterType, ProjectDto } from '@app/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {
  projects: ProjectDto[];
  archivedProjects: ProjectDto[];
  currentProjectId: number;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );

  constructor(
    private breakpointObserver: BreakpointObserver,
    private projectService: ProjectService,
    private route: ActivatedRoute
    ) { }

  ngOnInit() {
    this.getProjects();

    this.route.paramMap.subscribe(() => {
      // refresh the project when some dummy param is supplied
      this.getProjects();
    });
  }

  getProjects() {
    this.projectService.getProjects(ProjectStatusFilterType.active, true)
      .subscribe(data => this.projects = data);

    this.projectService.getProjects(ProjectStatusFilterType.archived, true)
      .subscribe(data => this.archivedProjects = data);
  }

}
