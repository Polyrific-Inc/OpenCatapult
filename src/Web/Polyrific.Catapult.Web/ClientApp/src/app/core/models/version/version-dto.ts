import { EngineDto } from '../engine/engine-dto';
import { TaskProviderDto } from '../task-provider/task-provider-dto';

export interface VersionDto {
  apiVersion: string;
  engines: EngineDto[];
  taskProviders: TaskProviderDto[];
}
