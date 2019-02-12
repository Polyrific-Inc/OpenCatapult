export interface ProjectDto {
    id: number;
    name: string;
    displayName: string;
    client: string;
    status: string;
    created: Date;
    config: Map<string, string>;
}