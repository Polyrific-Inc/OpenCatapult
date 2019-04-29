import { ProjectMember } from './project-member';

export interface User {
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
    password: string;
    token: string;
    role: string;
    projects: Array<ProjectMember>;
}
