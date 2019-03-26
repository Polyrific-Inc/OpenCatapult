import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map } from 'rxjs/operators';
import { ProjectService, ProjectStatusFilterType, ProjectDto, AuthorizePolicy } from '@app/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit, OnDestroy {
  projects: ProjectDto[];
  shownProjects: ProjectDto[];
  archivedProjects: ProjectDto[];
  deletingProjects: ProjectDto[];
  currentProjectId: number;
  authorizePolicy = AuthorizePolicy;
  shownProjectNumber = 10;
  private routerSubscription: Subscription;

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
    this.routerSubscription = this.route.paramMap.subscribe(() => {
      // refresh the project when some dummy param is supplied
      this.getProjects();
    });
  }

  ngOnDestroy() {
    this.routerSubscription.unsubscribe();
  }

  getProjects() {
    this.projectService.getProjects(ProjectStatusFilterType.all, true)
      .subscribe(data => {
        this.projects = data.filter(p => p.status === ProjectStatusFilterType.active);
        this.toggleShowAll(false);

        this.archivedProjects = data.filter(p => p.status === ProjectStatusFilterType.archived);

        this.deletingProjects = data.filter(p => p.status === ProjectStatusFilterType.deleting);
      });
  }

  toggleShowAll(showAll: boolean) {
    if (showAll) {
      this.shownProjects = this.projects;
    } else {
      this.shownProjects = this.projects.slice(0, this.shownProjectNumber);
    }
  }

}
