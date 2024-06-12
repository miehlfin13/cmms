namespace Synith.Security.Constants;

public static class PermissionCode
{
    #region Organization
    public static class Company
    {
        public const string View = "COMPANY_VIEW";
        public const string Add = "COMPANY_ADD";
        public const string Edit = "COMPANY_EDIT";
        public const string Deactivate = "COMPANY_DEACTIVATE";
    }

    public static class Area
    {
        public const string View = "AREA_VIEW";
        public const string Add = "AREA_ADD";
        public const string Edit = "AREA_EDIT";
        public const string Deactivate = "AREA_DEACTIVATE";
    }

    public static class Branch
    {
        public const string View = "BRANCH_VIEW";
        public const string Add = "BRANCH_ADD";
        public const string Edit = "BRANCH_EDIT";
        public const string Deactivate = "BRANCH_DEACTIVATE";
    }
    #endregion

    #region User Access
    public static class Role
    {
        public const string View = "ROLE_VIEW";
        public const string Add = "ROLE_ADD";
        public const string Edit = "ROLE_EDIT";
        public const string Deactivate = "ROLE_DEACTIVATE";
    }

    public static class User
    {
        public const string View = "USER_VIEW";
        public const string Add = "USER_ADD";
        public const string Edit = "USER_EDIT";
        public const string Deactivate = "USER_DEACTIVATE";
    }
    #endregion

    #region Employee
    public static class Employee
    {
        public const string View = "EMPLOYEE_VIEW";
        public const string Add = "EMPLOYEE_ADD";
        public const string Edit = "EMPLOYEE_EDIT";
        public const string Deactivate = "EMPLOYEE_DEACTIVATE";
    }
    #endregion
}
