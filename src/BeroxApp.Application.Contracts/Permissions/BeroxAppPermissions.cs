namespace BeroxApp.Permissions;

public static class BeroxAppPermissions
{
    public const string GroupName = "BeroxApp";


    // Service Permissions
    public static class Services
    {
        public const string Default = GroupName + ".Services";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    // Customer Permissions
    public static class Customers
    {
        public const string Default = GroupName + ".Customers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    // Employee Permissions
    public static class Employees
    {
        public const string Default = GroupName + ".Employees";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    // Reservation Permissions
    public static class Reservations
    {
        public const string Default = GroupName + ".Reservations";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Approve = Default + ".Approve";
        public const string Complete = Default + ".Complete";
        public const string Cancel = Default + ".Cancel";
    }

    // Finance Permissions
    public static class Finance
    {
        public const string Default = GroupName + ".Finance";
        public const string Expenses = Default + ".Expenses";
        public const string CreateExpense = Expenses + ".Create";
        public const string EditExpense = Expenses + ".Edit";
        public const string DeleteExpense = Expenses + ".Delete";
        public const string Reports = Default + ".Reports";
    }
}
