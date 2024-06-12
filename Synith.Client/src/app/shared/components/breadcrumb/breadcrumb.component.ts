import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRouteSnapshot, Event, NavigationEnd, Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Breadcrumb } from '@synith/shared/dto/breadcrumb';
import { BreadcrumbService } from '@synith/shared/services';

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [
    RouterModule,
    TranslateModule
  ],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss'
})
export class BreadcrumbComponent implements OnInit {

  private readonly router = inject(Router);
  private readonly service = inject(BreadcrumbService);

  breadcrumbs: Breadcrumb[] = [];

  ngOnInit(): void {
    this.router.events.subscribe((routerEvent) => this.onRouterEvent(routerEvent));
  }

  private onRouterEvent(routerEvent: Event) {
    if (!(routerEvent instanceof NavigationEnd)) {
      return;
    }

    let route = this.router.routerState.root.snapshot;
    let url = '';

    let breadcrumbIndex = 0;
    let newCrumbs: Breadcrumb[] = [];
    let currentModule = '';

    while (route.firstChild != null) {
      route = route.firstChild;

      if (route.routeConfig === null
        || !route.routeConfig.path
        || !route.data['breadcrumb']
      ) { continue; }

      url += `/${this.createUrl(route)}`;

      if (currentModule == '') {
        currentModule = route.data['breadcrumb'];
      }

      let crumb = this.createBreadcrumb(route, url)
      if (breadcrumbIndex < this.breadcrumbs.length) {
        let existing = this.breadcrumbs[breadcrumbIndex++];

        if (existing && existing.route == route.routeConfig) {
          crumb.displayName = existing.displayName;
        }
      }

      newCrumbs.push(crumb);
    }

    this.breadcrumbs = newCrumbs;
    this.service.moduleChanged.next(currentModule.toLowerCase());
  }

  private createBreadcrumb(route: ActivatedRouteSnapshot, url: string): Breadcrumb {
    return {
      displayName: `GENERAL.${route.data['breadcrumb']}`.toUpperCase(),
      url: url,
      isCurrentPage: this.isCurrentPage(route),
      route: route.routeConfig
    };
  }

  private createUrl(route: ActivatedRouteSnapshot) {
    return route.url.map(function (s) { return s.toString(); }).join('/');
  }

  private isCurrentPage(route: ActivatedRouteSnapshot) {
    return route.firstChild === null
      || route.firstChild.routeConfig === null
      || !route.firstChild.routeConfig.path;
  }
}
