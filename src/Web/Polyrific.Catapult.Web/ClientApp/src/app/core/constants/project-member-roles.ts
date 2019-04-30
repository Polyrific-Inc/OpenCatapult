import { ProjectMemberRole } from '../auth/project-member-role';

export const projectMemberRoles = [
  [1, ProjectMemberRole.Owner],
  [2, ProjectMemberRole.Maintainer],
  [3, ProjectMemberRole.Contributor],
  [4, ProjectMemberRole.Member]
];
