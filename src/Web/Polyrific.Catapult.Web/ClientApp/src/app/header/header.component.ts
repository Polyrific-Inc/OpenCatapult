import { Component, OnInit, Input, Inject, Renderer2 } from '@angular/core';
import { AuthService } from '../core/auth/auth.service';
import { Router } from '@angular/router';
import { AuthorizePolicy } from '../core/auth/authorize-policy';
import { MatSidenav, MatDialog, DialogPosition } from '@angular/material';
import { AccountService, ManagedFileService, HelpContextDto } from '@app/core';
import { HelpContextService } from '@app/core/services/help-context.service';
import { HelpContextDialogComponent } from '@app/help-context-dialog/help-context-dialog.component';
import { DOCUMENT } from '@angular/common';

interface HelpContextViewModel extends HelpContextDto {
  element: Element;
}

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  @Input() sidenav: MatSidenav;
  authorizePolicyEnum = AuthorizePolicy;
  greetingsName: string;
  avatarImage: any;
  loading: boolean;

  constructor(
      private authService: AuthService,
      private accountService: AccountService,
      private router: Router,
      private dialog: MatDialog,
      private managedFileService: ManagedFileService,
      private helpContextService: HelpContextService,
      private renderer: Renderer2,
      @Inject(DOCUMENT) private document: Document
    ) { }

  ngOnInit() {
    this.authService.currentUser.subscribe(user => {
      if (user) {
        this.greetingsName = '';
        if (user.firstName) {
          this.greetingsName += user.firstName;
        }

        if (user.lastName) {
          this.greetingsName += ` ${user.lastName}`;
        }

        if (!this.greetingsName) {
          this.greetingsName = user.userName;
        }

        this.accountService.getUserByUserName(user.userName)
        .subscribe(data => {
          if (data.avatarFileId) {
            this.avatarImage = this.managedFileService.getImageUrl(data.avatarFileId);
          }
        });
      }
    });
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  isQuickAddMenuShown() {
    return this.authService.checkRoleAuthorization(AuthorizePolicy.UserRoleAdminAccess, null) ||
      this.authService.checkRoleAuthorization(AuthorizePolicy.UserRoleBasicAccess, null);
  }

  onHelpClick() {
    this.loading = true;
    const section = this.helpContextService.getSectionByActiveRoute(this.router.url);
    this.helpContextService.getHelpContextsBySection(section)
      .subscribe(data => {
        this.loading = false;

        if (!data || data.length === 0) {
          this.dialog.open(HelpContextDialogComponent);
        } else {
          const helpContexts: HelpContextViewModel[] = data.map(h => ({
            element: null,
            ...h
          }));
          const helpContentElements = this.document.getElementsByClassName(`help-element-${section}`);
          for (let i = 0; i < helpContentElements.length; i++) {
            const element = helpContentElements.item(i);
            const elSubSection = element.getAttribute('data-sub-section');

            const item = elSubSection ? helpContexts.find(s => s.subSection === elSubSection) :
              helpContexts.find(s => s.subSection == null);

            item.element = item.element || element;
          }

          this.openHelpContext(helpContexts, 0);
        }
      });
  }

  private openHelpContext(helpContexts: HelpContextViewModel[], index: number) {
    if (index < helpContexts.length) {
      const helpContext = helpContexts[index];
      let position: DialogPosition = null;

      if (helpContext.element) {
        const elemPosition = helpContext.element.getBoundingClientRect();
        position = {
          top: `${elemPosition.top + helpContext.element.clientHeight}px`
        };

        if (elemPosition.left < window.innerWidth * 0.6) {
          position.left = `${elemPosition.left}px`;
        } else {
          position.right = `${window.innerWidth - elemPosition.right}px`;
        }
      }

      const dialogRef = this.dialog.open(HelpContextDialogComponent, {
        data: {
          item: helpContexts[index],
          last: index === helpContexts.length - 1
        },
        position: position,
        maxWidth: 640,
        hasBackdrop: false
      });

      dialogRef.afterClosed().subscribe(confirmed => {
        if (confirmed) {
          this.openHelpContext(helpContexts, ++index);
        }
      });
    }
  }
}
