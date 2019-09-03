import { Component, OnInit, Input } from '@angular/core';
import { DataModelDto } from '@app/core';

interface DataModelViewModel extends DataModelDto {
  selected: boolean;
}

@Component({
  selector: 'app-data-model',
  templateUrl: './data-model.component.html',
  styleUrls: ['./data-model.component.css']
})
export class DataModelComponent implements OnInit {
  @Input() dataModels: DataModelViewModel[];

  constructor() { }

  ngOnInit() {
  }

}
