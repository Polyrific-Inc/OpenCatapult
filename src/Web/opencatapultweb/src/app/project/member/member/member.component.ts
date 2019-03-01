import { Component, OnInit } from '@angular/core';
import { ProjectMemberDto } from '@app/core';
import { MemberService } from '@app/core/services/member.service';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material';
import { ConfirmationDialogComponent, SnackbarService } from '@app/shared';
import { MemberInfoDialogComponent } from '../components/member-info-dialog/member-info-dialog.component';
import { MemberNewDialogComponent } from '../components/member-new-dialog/member-new-dialog.component';

@Component({
  selector: 'app-member',
  templateUrl: './member.component.html',
  styleUrls: ['./member.component.css']
})
export class MemberComponent implements OnInit {
  members: ProjectMemberDto[];
  projectId: number;
  roleId = 0;

  displayedColumns: string[] = ['username', 'role', 'actions'];

  constructor(
    private route: ActivatedRoute,
    private memberService: MemberService,
    private dialog: MatDialog,
    private snackbar: SnackbarService
    ) { }

  ngOnInit() {
    this.projectId = +this.route.parent.parent.snapshot.params.id;
    this.getMembers();
  }

  getMembers() {
    this.memberService.getMembers(this.projectId, this.roleId)
      .subscribe(data => this.members = data);
  }

  onInfoClick(member: ProjectMemberDto) {

    const dialogRef = this.dialog.open(MemberInfoDialogComponent, {
      data: member
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getMembers();
      }
    });
  }

  onDeleteClick(member: ProjectMemberDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Delete Member',
        confirmationText: `Are you sure you want to remove user ${member.username} from the project?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.memberService.deleteMember(member.projectId, member.id)
          .subscribe(() => {
            this.snackbar.open('Member has been removed from the project');

            this.getMembers();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onNewMemberClick() {
    const dialogRef = this.dialog.open(MemberNewDialogComponent, {
      data: {
        projectId: this.projectId
      }
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getMembers();
      }
    });
  }

}
