import { Route } from "@angular/router";

export class Breadcrumb {
  displayName = '';
  isCurrentPage = true;
  url = '';
  route: Route | null = null;
}
