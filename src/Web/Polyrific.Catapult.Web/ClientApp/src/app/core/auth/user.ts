import { ProjectMember } from './project-member';

export interface User {
    id: number;
    userName: string;
    firstName: string;
    lastName: string;
    password: string;
    token: string;
    role: string;
    projects: Array<ProjectMember>;
}
