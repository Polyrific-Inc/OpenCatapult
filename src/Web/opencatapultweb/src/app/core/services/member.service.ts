import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { ProjectMemberDto } from '../models/member/project-member-dto';
import { NewProjectMemberDto } from '../models/member/new-project-member-dto';
import { UpdateProjectMemberDto } from '../models/member/update-project-member-dto';

@Injectable()
export class MemberService {

  constructor(private api: ApiService) { }

  getMembers(projectId: number, roleId: number) {
    let path = `project/${projectId}/member`;

    if (roleId > 0) {
      path = `${path}?roleId=${roleId}`;
    }

    return this.api.get<ProjectMemberDto[]>(path);
  }

  createMember(projectId: number, dto: NewProjectMemberDto) {
    return this.api.post<ProjectMemberDto>(`project/${projectId}/member`, dto);
  }

  updateMember(projectId: number, memberId: number, dto: UpdateProjectMemberDto) {
    return this.api.put(`project/${projectId}/member/${memberId}`, dto);
  }

  deleteMember(projectId: number, memberId: number) {
    return this.api.delete(`project/${projectId}/member/${memberId}`);
  }
}
