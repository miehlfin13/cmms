import { Injectable, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { CookiesKey, LocalStorageKey } from '@synith/shared/constants';
import { JwtClaim, UserSettingName } from '@synith/modules/user-account/constants';
import { Permission, RoleService, UserService, UserSetting } from '@synith/openapi';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  private readonly userApi = inject(UserService);
  private readonly roleApi = inject(RoleService);
  private readonly cookies = inject(CookieService);

  private permissions: Permission[] = [];
  private settings: UserSetting[] = [];

  changed = new BehaviorSubject<boolean>(false);

  getAccessToken() {
    return localStorage.getItem(LocalStorageKey.token) ?? '';
  }

  setAccessToken(token: string) {
    localStorage.setItem(LocalStorageKey.token, token);
    this.retrievePermissions();
    this.retrieveSettings();
  }

  clearAccessToken() {
    localStorage.removeItem(LocalStorageKey.token);
    this.permissions = [];
    this.settings = [];
    this.changed.next(false);
  }

  isLoggedOn() {
    return this.getUsername() != '';
  }

  isExpired() {
    const token = this.getAccessToken();
    if (token == '') return true;
    const jwt = JSON.parse(window.atob(token.split('.')[1])) as JwtClaim;
    return Math.floor((new Date).getTime() / 1000) >= jwt.exp;
  }

  getUsername() {
    const token = this.getAccessToken();
    if (token == '') return '';
    const jwt = JSON.parse(window.atob(token.split('.')[1])) as JwtClaim;
    return jwt.username;
  }

  getUserId() {
    const token = this.getAccessToken();
    if (token == '') return 0;
    const jwt = JSON.parse(window.atob(token.split('.')[1])) as JwtClaim;
    return parseInt(jwt.user_id);
  }

  getRoleIds() {
    const token = this.getAccessToken();
    if (token == '') return [];
    const jwt = JSON.parse(window.atob(token.split('.')[1])) as JwtClaim;
    return jwt.role_ids.split(',').map(id => parseInt(id));
  }

  getLanguageCode() {
    const token = this.getAccessToken();
    if (token == '') return '';
    const jwt = JSON.parse(window.atob(token.split('.')[1])) as JwtClaim;
    return jwt.language_code.toLowerCase();
  }
  
  hasPermission(code: string) {
    if (code == '') {
      return true;
    }

    if (this.permissions.length == 0) {
      const json = this.cookies.get(CookiesKey.permissions);

      if (json != '') {
        this.permissions = JSON.parse(json);
      }

      if (this.permissions.length == 0) {
        this.retrievePermissions();
      }
    }
    return this.permissions.find(x => x.code == code) != undefined;
  }

  getSetting(name: string) {
    if (this.settings.length == 0) {
      const json = this.cookies.get(CookiesKey.settings);

      if (json != '') {
        this.settings = JSON.parse(json);
      }

      if (this.settings.length == 0) {
        this.retrieveSettings();
      }
    }

    return this.settings.find(x => x.name == name)?.value ?? '';
  }

  updateSetting(name: string, value: string) {
    const setting: UserSetting = {
      userId: this.getUserId(),
      name: name,
      value: value
    };

    this.userApi.updateSetting(setting).subscribe({
      next: () => this.retrieveSettings()
    });
  }

  private retrievePermissions() {
    this.getRoleIds().forEach(roleId => {
      this.roleApi.retrieveRolePermissions(roleId).subscribe({
        next: (response) => {
          response.forEach(permission => {
            if (this.permissions.find(x => x.code == permission.code) == undefined) {
              this.permissions.push(permission);
            }
          });

          this.cookies.set(CookiesKey.permissions, JSON.stringify(this.permissions));
          this.changed.next(true);
        }
      });
    });
  }

  private retrieveSettings() {
    this.userApi.retrieveSettings(this.getUserId()).subscribe({
      next: (response) => {
        this.settings = [];
        this.settings.push(...response);

        const theme = this.getSetting(UserSettingName.theme);
        if (theme != '') {
          localStorage.setItem(LocalStorageKey.theme, theme);
        }
        this.cookies.set(CookiesKey.settings, JSON.stringify(this.settings));
        this.changed.next(true);
      }
    });
  }
}
