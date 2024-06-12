import { Routes } from "@angular/router";
import { canActivate } from "@synith/auth.guard";

export const HomeRoutes: Routes = [
  {
    path: 'dashboard',
    data: { breadcrumb: 'dashboard' },
    loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [canActivate()]
  }
];
