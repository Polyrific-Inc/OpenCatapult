 export interface NewProjectMemberDto {
     userId: number;
     projetMemberRoleId: number;
     externalAccountIds: { [key: string]: string };
 }
