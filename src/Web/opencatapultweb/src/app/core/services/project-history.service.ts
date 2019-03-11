import { Injectable } from '@angular/core';
import { ProjectDto } from '../models/project/project-dto';

const localStorageKey = 'projectHistory';

@Injectable()
export class ProjectHistoryService {
  history: {id: number, name: string}[];

  constructor() {
    const historyString = localStorage.getItem(localStorageKey);

    if (historyString) {
      this.history = JSON.parse(historyString);
    } else {
      this.history = [];
    }
  }

  addProjectHistory(dto: ProjectDto) {
    this.history = this.history.filter(p => p.id !== dto.id);
    this.history.splice(0, 0, {id: dto.id, name: dto.name});
    localStorage.setItem(localStorageKey, JSON.stringify(this.history));
  }
}
