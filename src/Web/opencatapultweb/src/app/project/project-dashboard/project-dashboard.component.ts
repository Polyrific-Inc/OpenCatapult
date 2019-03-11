import { Component, OnInit } from '@angular/core';
import { ProjectHistoryService } from '@app/core';

@Component({
  selector: 'app-project-dashboard',
  templateUrl: './project-dashboard.component.html',
  styleUrls: ['./project-dashboard.component.css']
})
export class ProjectDashboardComponent implements OnInit {
  history = this.projectHistoryService.history;
  shownHistory = this.projectHistoryService.history;
  shownHistoryNumber = 5;

  constructor(
    private projectHistoryService: ProjectHistoryService
    ) { }

  ngOnInit() {
    this.toggleShowAll(false);
  }

  toggleShowAll(showAll: boolean) {
    if (showAll) {
      this.shownHistory = this.history;
    } else {
      this.shownHistory = this.history.slice(0, this.shownHistoryNumber);
    }
  }

}
