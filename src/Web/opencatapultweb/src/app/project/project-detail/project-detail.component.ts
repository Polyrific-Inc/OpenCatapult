import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectService, ProjectDto } from '@app/core';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent implements OnInit {
  project: ProjectDto;
  activeLink: string;

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectService
  ) { }

  ngOnInit() {
    this.getProject();
    this.activeLink = this.route.firstChild.snapshot.url.pop().path;
  }

  getProject() : void {
    this.route.params.subscribe(params => {      
      this.activeLink = 'info';

      const id = +params.id;
      this.projectService.getProject(id)
        .subscribe(project => this.project = project);
    })
  }

}
