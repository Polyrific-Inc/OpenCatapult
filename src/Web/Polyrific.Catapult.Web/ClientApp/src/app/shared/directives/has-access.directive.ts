import { Directive, TemplateRef, ViewContainerRef, Input } from '@angular/core';
import { AuthService } from '@app/core/auth/auth.service';
import { ProjectService, AuthorizePolicy } from '@app/core';

@Directive({
  selector: '[appHasAccess]'
})
export class HasAccessDirective {
  private hasView: boolean;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService,
    private projectService: ProjectService) { }

    @Input() set appHasAccess(authPolicy: AuthorizePolicy) {
      const hasAccess = this.authService.checkRoleAuthorization(authPolicy, this.projectService.currentProjectId);

      if (hasAccess && !this.hasView) {
        this.viewContainer.createEmbeddedView(this.templateRef);
        this.hasView = true;
      } else if (hasAccess && this.hasView) {
        this.viewContainer.clear();
        this.hasView = false;
      }
    }

}
