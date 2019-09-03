import { Component, OnInit, Input } from '@angular/core';
import { DataModelPropertyDto } from '@app/core';

@Component({
  selector: 'app-data-model-property',
  templateUrl: './data-model-property.component.html',
  styleUrls: ['./data-model-property.component.css']
})
export class DataModelPropertyComponent implements OnInit {
  @Input() properties: DataModelPropertyDto[];

  constructor() { }

  ngOnInit() {
  }

}
