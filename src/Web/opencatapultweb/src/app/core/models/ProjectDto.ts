export interface ProjectDto {
    id: number;
    name: string;
    client: string;
    status: string;
    config: Map<string, string>;
}