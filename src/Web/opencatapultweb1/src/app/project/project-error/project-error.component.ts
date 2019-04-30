import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project-error',
  templateUrl: './project-error.component.html',
  styleUrls: ['./project-error.component.css']
})
export class ProjectErrorComponent implements OnInit {
  projectId: number;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.projectId = +this.route.snapshot.params.projectId;
  }

}
