namespace BeroxApp.Permissions;


    public static class BeroxAppPermissions
    {
        public static class Services
        {
            public const string GroupName = "BeroxApp.Services";
            public const string Default = GroupName;
            public const string Create = GroupName + ".Create";
            public const string Edit = GroupName + ".Edit";
            public const string Delete = GroupName + ".Delete";
        }

        public static class Customers
        {
            public const string GroupName = "BeroxApp.Customers";
            public const string Default = GroupName;
            public const string Create = GroupName + ".Create";
            public const string Edit = GroupName + ".Edit";
            public const string Delete = GroupName + ".Delete";
        }

        public static class Employees
        {
            public const string GroupName = "BeroxApp.Employees";
            public const string Default = GroupName;
            public const string Create = GroupName + ".Create";
            public const string Edit = GroupName + ".Edit";
            public const string Delete = GroupName + ".Delete";
        }

        public static class Reservations
        {
            public const string GroupName = "BeroxApp.Reservations";
            public const string Default = GroupName;
            public const string Create = GroupName + ".Create";
            public const string Edit = GroupName + ".Edit";
            public const string Delete = GroupName + ".Delete";
            public const string Approve = GroupName + ".Approve";
            public const string Complete = GroupName + ".Complete";
            public const string Cancel = GroupName + ".Cancel";
        }

        public static class Finance
        {
            public const string GroupName = "BeroxApp.Finance";
            public const string Default = GroupName;

            public const string Expenses = GroupName + ".Expenses";
            public const string CreateExpense = Expenses + ".Create";
            public const string EditExpense = Expenses + ".Edit";
            public const string DeleteExpense = Expenses + ".Delete";

            public const string Reports = GroupName + ".Reports";
        }

}
