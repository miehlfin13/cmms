import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SidenavType } from '@synith/shared/constants';
import { Sidenav } from '@synith/modules/user-account/dto';
import { TokenService } from '@synith/modules/user-account/services';
import { BreadcrumbService, SidenavService } from '@synith/shared/services';
import { UserSettingName } from '@synith/modules/user-account/constants';

@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [
    RouterOutlet,
    TranslateModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule
  ],
  templateUrl: './sidenav.component.html',
  styleUrl: './sidenav.component.scss'
})
export class SidenavComponent implements OnInit {

  public readonly token = inject(TokenService);
  public readonly router = inject(Router);
  private readonly service = inject(SidenavService);
  private readonly breadcrumb = inject(BreadcrumbService);

  currentModule: string = 'dashboard';

  sidenavList: Sidenav[] = [];

  iconMini = 'arrow_forward_ios';
  iconNormal = 'arrow_back_ios';

  type = SidenavType.normal;
  icon = this.iconNormal;

  constructor() {
    if (this.token.isLoggedOn() && !this.token.isExpired()) {
      if (this.token.getSetting(UserSettingName.sidenav) == SidenavType.mini) {
        this.type = SidenavType.mini;
        this.icon = this.iconMini;
        this.service.minified.next(true);
      }
    }
  }

  ngOnInit(): void {
    this.addHomeSidenav();
    this.addUserAccessSidenav();
    this.breadcrumb.moduleChanged.subscribe((currentModule: string) => this.onModuleChanged(currentModule));
  }

  toggleSidenav() {
    if (this.type == SidenavType.normal) {
      this.type = SidenavType.mini;
      this.icon = this.iconMini;
      this.service.minified.next(true);
    } else {
      this.type = SidenavType.normal;
      this.icon = this.iconNormal;
      this.service.minified.next(false);
    }

    if (this.token.isLoggedOn()) {
      this.token.updateSetting(UserSettingName.sidenav, this.type);
    }
  }

  isTooltipDisabled() {
    return this.type != SidenavType.mini;
  }

  toggleModule(module: string) {
    let nav = this.sidenavList.find(x => x.moduleName == module)!;
    nav.isExpanded = !nav.isExpanded;
  }

  private addHomeSidenav() {
    const home = new Sidenav('home', 'GENERAL.HOME', '', 'home')
    home.children = [
      new Sidenav('dashboard', 'GENERAL.DASHBOARD', '', 'dashboard')
    ];
    this.sidenavList.push(home);
  }

  private addUserAccessSidenav() {
    const userAccess = new Sidenav('userAccess', 'GENERAL.USERACCOUNTS', 'USER_VIEW', 'manage_accounts');
    userAccess.children = [
      new Sidenav('role', 'GENERAL.ROLES', 'ROLE_VIEW', 'groups'),
      new Sidenav('user', 'GENERAL.USERS', 'USER_VIEW', 'person')
    ];
    this.sidenavList.push(userAccess);
  }

  private onModuleChanged(currentModule: string) {
    this.currentModule = currentModule;

    this.sidenavList.forEach(sidenav => {
      let child = sidenav.children.find(x => x.moduleName == currentModule);
      sidenav.isExpanded = child != null;
    });

  }
}
