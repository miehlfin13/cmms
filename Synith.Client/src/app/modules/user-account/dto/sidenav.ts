export class Sidenav {
  moduleName: string;
  icon: string;
  label: string;
  permissionCode: string;

  isExpanded: boolean = false;
  children: Sidenav[] = [];

  constructor(moduleName: string, label: string, permissionCode: string, icon: string) {
    this.moduleName = moduleName;
    this.label = label;
    this.permissionCode = permissionCode;
    this.icon = icon;
  }
}
