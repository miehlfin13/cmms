import { Routes } from '@angular/router';
import { HomeRoutes } from '@synith/modules/home/pages';
import { UserAccountRoutes } from '@synith/modules/user-account/pages';
import { invalidPage } from '@synith/auth.guard';

export const routes: Routes = [
  ...HomeRoutes,
  ...UserAccountRoutes,
  {
    path: "**",
    loadComponent: () => import('@synith/modules/user-account/pages/login/login.component').then(m => m.LoginComponent),
    canActivate: [invalidPage]
  }
];
