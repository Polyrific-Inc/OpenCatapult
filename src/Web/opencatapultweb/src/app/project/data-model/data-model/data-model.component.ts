import { Component, OnInit } from '@angular/core';
import { DataModelDto, DataModelService } from '@app/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-data-model',
  templateUrl: './data-model.component.html',
  styleUrls: ['./data-model.component.css']
})
export class DataModelComponent implements OnInit {
  dataModels: DataModelDto[];

  constructor(
    private route: ActivatedRoute,
    private dataModelService: DataModelService
  ) { }

  ngOnInit() {
    this.getDataModels();
  }

  getDataModels() {
    const projectId = +this.route.parent.parent.snapshot.params.id;
    this.dataModelService.getDataModels(projectId, true)
      .subscribe(data => this.dataModels = data);
  }

  onModelInfoClick(model: DataModelDto) {  }

  onModelDeleteClick(model: DataModelDto) {  }

  onModelPropertyAddClick(model: DataModelDto) {  }

}
