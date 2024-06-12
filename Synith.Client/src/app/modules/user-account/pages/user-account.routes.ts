import { Routes } from "@angular/router";
import { canActivate } from "@synith/auth.guard";
import { RolePermissionCode, UserPermissionCode } from "@synith/modules/user-account/constants";

export const UserAccountRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'login',
    loadComponent: () => import('./login/login.component').then(m => m.LoginComponent)
  },

  {
    path: 'role',
    data: { breadcrumb: 'role' },
    children: [
      {
        path: '',
        loadComponent: () => import('./role/role-list.component').then(m => m.RoleListComponent),
        canActivate: [canActivate(RolePermissionCode.View)]
      },
      {
        path: 'add',
        data: { breadcrumb: 'add' },
        loadComponent: () => import('./role/role-add.component').then(m => m.RoleAddComponent),
        canActivate: [canActivate(RolePermissionCode.Add)]
      },
      {
        path: 'edit/:id',
        data: { breadcrumb: 'edit' },
        loadComponent: () => import('./role/role-edit.component').then(m => m.RoleEditComponent),
        canActivate: [canActivate(RolePermissionCode.Edit)]
      },
      {
        path: 'view/:id',
        data: { breadcrumb: 'view' },
        loadComponent: () => import('./role/role-view.component').then(m => m.RoleViewComponent),
        canActivate: [canActivate(RolePermissionCode.View)]
      }
    ]
  },

  {
    path: 'user',
    data: { breadcrumb: 'user' },
    children: [
      {
        path: '',
        loadComponent: () => import('./user/user-list.component').then(m => m.UserListComponent),
        canActivate: [canActivate(UserPermissionCode.View)]
      },
      {
        path: 'add',
        data: { breadcrumb: 'add' },
        loadComponent: () => import('./user/user-add.component').then(m => m.UserAddComponent),
        canActivate: [canActivate(UserPermissionCode.Add)]
      },
      {
        path: 'edit/:id',
        data: { breadcrumb: 'edit' },
        loadComponent: () => import('./user/user-edit.component').then(m => m.UserEditComponent),
        canActivate: [canActivate(UserPermissionCode.Edit)]
      },
      {
        path: 'view/:id',
        data: { breadcrumb: 'view' },
        loadComponent: () => import('./user/user-view.component').then(m => m.UserViewComponent),
        canActivate: [canActivate(UserPermissionCode.View)]
      }
    ]
  }
];
