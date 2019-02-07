import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EngineRoutingModule } from './engine-routing.module';
import { EngineComponent } from './engine/engine.component';

@NgModule({
  declarations: [EngineComponent],
  imports: [
    CommonModule,
    EngineRoutingModule
  ]
})
export class EngineModule { }
