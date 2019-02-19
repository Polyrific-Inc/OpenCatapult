export interface CloneProjectOptionDto {
    newProjectName: string;
    displayName: string;
    client: string;
    includeMembers: boolean;
    includeJobDefinitions: boolean;
}