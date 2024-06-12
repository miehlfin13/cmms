import { CommonModule } from '@angular/common';
import { Component, Renderer2, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Language, UserService } from '@synith/openapi';
import { PasswordChangeService, TokenService } from '@synith/modules/user-account/services';
import { LocalStorageKey } from '@synith/shared/constants';
import { UserSettingName } from '@synith/modules/user-account/constants';
import { BreadcrumbComponent } from '@synith/shared/components';
import { ConfirmDialogService, SidenavService } from '@synith/shared/services';
import { environment } from '@synith/../environments/environment';

@Component({
  selector: 'app-toolbar',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatToolbarModule,
    MatButtonModule,
    MatMenuModule,
    MatIconModule,
    MatDividerModule,
    BreadcrumbComponent
  ],
  templateUrl: './toolbar.component.html',
  styleUrl: './toolbar.component.scss'
})
export class ToolbarComponent {

  public readonly token = inject(TokenService);
  public readonly passwordChange = inject(PasswordChangeService);
  private readonly renderer = inject(Renderer2);
  private readonly translate = inject(TranslateService);
  private readonly userApi = inject(UserService);
  private readonly router = inject(Router);
  private readonly sidenav = inject(SidenavService);
  private readonly confirmDialog = inject(ConfirmDialogService);

  lightTheme = 'theme-light';
  darkTheme = 'theme-dark';

  lightIcon = 'light_mode';
  darkIcon = 'dark_mode';
  
  lightTooltip = 'Switch to light mode';
  darkTooltip = 'Switch to dark mode';

  theme = '';
  icon = '';
  tooltip = '';

  languages: Language[] = [];
  language = '';

  isSidenavMinified = false;

  constructor() {
    this.loadLanguages();
    this.translate.defaultLang = environment.defaultLanguage.toLowerCase();

    this.refreshTheme();
    this.changeLanguageByCode(localStorage.getItem(LocalStorageKey.language) ?? this.translate.defaultLang);

    this.token.changed.subscribe(hasToken => {
      if (hasToken) {
        this.refreshTheme();
        this.changeLanguageByCode(this.token.getLanguageCode());
      }
    });

    this.sidenav.minified.subscribe(isMinified => this.isSidenavMinified = isMinified);
  }

  switchTheme() {
    if (this.theme == this.lightTheme) {
      this.theme = this.darkTheme;
    } else {
      this.theme = this.lightTheme;
    }
    localStorage.setItem(LocalStorageKey.theme, this.theme);
    this.refreshTheme();

    if (this.token.isLoggedOn()) {
      this.token.updateSetting(UserSettingName.theme, this.theme ?? this.lightTheme);
    }
  }

  refreshTheme() {
    this.theme = localStorage.getItem(LocalStorageKey.theme) ?? this.lightTheme;
    if (this.theme == this.lightTheme) {
      this.renderer.addClass(document.body, this.lightTheme)
      this.renderer.removeClass(document.body, this.darkTheme)
      this.icon = this.lightIcon;
      this.tooltip = this.darkTooltip;
    } else {
      this.renderer.addClass(document.body, this.darkTheme)
      this.renderer.removeClass(document.body, this.lightTheme)
      this.icon = this.darkIcon;
      this.tooltip = this.lightTooltip;
    }
  }

  loadLanguages() {
    if (this.languages.length == 0) {
      this.userApi.retrieveLanguages().subscribe({
        next: (response) => {
          this.languages = response;
        }
      })
    }
  }

  changeLanguageByCode(code: string) {
    this.language = code.toLowerCase();
    localStorage.setItem(LocalStorageKey.language, this.language);
    this.translate.use(this.language);
  }

  changeLanguage(language: Language) {
    this.changeLanguageByCode(language.code!);
  }

  logout() {
    this.translate.get('CONFIRM.LOGOUT').subscribe(text => {
      this.confirmDialog.show(text, () => {
        this.sidenav.minified.next(false);
        this.token.clearAccessToken();
        this.router.navigate(['']);
      });
    })
  }
}
