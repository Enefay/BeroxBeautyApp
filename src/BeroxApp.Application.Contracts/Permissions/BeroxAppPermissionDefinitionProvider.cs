﻿using BeroxApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BeroxApp.Permissions;

public class BeroxAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        // Service Permissions
        var servicesGroup = context.AddGroup(BeroxAppPermissions.Services.GroupName);
        var servicesPermission = servicesGroup.AddPermission(BeroxAppPermissions.Services.Default, L("Permission:Services"));
        servicesPermission.AddChild(BeroxAppPermissions.Services.Create, L("Permission:Services.Create"));
        servicesPermission.AddChild(BeroxAppPermissions.Services.Edit, L("Permission:Services.Edit"));
        servicesPermission.AddChild(BeroxAppPermissions.Services.Delete, L("Permission:Services.Delete"));

        // Customer Permissions
        var customersGroup = context.AddGroup(BeroxAppPermissions.Customers.GroupName);
        var customersPermission = customersGroup.AddPermission(BeroxAppPermissions.Customers.Default, L("Permission:Customers"));
        customersPermission.AddChild(BeroxAppPermissions.Customers.Create, L("Permission:Customers.Create"));
        customersPermission.AddChild(BeroxAppPermissions.Customers.Edit, L("Permission:Customers.Edit"));
        customersPermission.AddChild(BeroxAppPermissions.Customers.Delete, L("Permission:Customers.Delete"));

        // Employee Permissions
        var employeesGroup = context.AddGroup(BeroxAppPermissions.Employees.GroupName);
        var employeesPermission = employeesGroup.AddPermission(BeroxAppPermissions.Employees.Default, L("Permission:Employees"));
        employeesPermission.AddChild(BeroxAppPermissions.Employees.Create, L("Permission:Employees.Create"));
        employeesPermission.AddChild(BeroxAppPermissions.Employees.Edit, L("Permission:Employees.Edit"));
        employeesPermission.AddChild(BeroxAppPermissions.Employees.Delete, L("Permission:Employees.Delete"));

        // Reservation Permissions
        var reservationsGroup = context.AddGroup(BeroxAppPermissions.Reservations.GroupName);
        var reservationsPermission = reservationsGroup.AddPermission(BeroxAppPermissions.Reservations.Default, L("Permission:Reservations"));
        reservationsPermission.AddChild(BeroxAppPermissions.Reservations.Create, L("Permission:Reservations.Create"));
        reservationsPermission.AddChild(BeroxAppPermissions.Reservations.Edit, L("Permission:Reservations.Edit"));
        reservationsPermission.AddChild(BeroxAppPermissions.Reservations.Delete, L("Permission:Reservations.Delete"));
        reservationsPermission.AddChild(BeroxAppPermissions.Reservations.Approve, L("Permission:Reservations.Approve"));
        reservationsPermission.AddChild(BeroxAppPermissions.Reservations.Complete, L("Permission:Reservations.Complete"));
        reservationsPermission.AddChild(BeroxAppPermissions.Reservations.Cancel, L("Permission:Reservations.Cancel"));

        // Finance Permissions
        var financeGroup = context.AddGroup(BeroxAppPermissions.Finance.GroupName);
        var financePermission = financeGroup.AddPermission(BeroxAppPermissions.Finance.Default, L("Permission:Finance"));

        var expensesPermission = financePermission.AddChild(BeroxAppPermissions.Finance.Expenses, L("Permission:Finance.Expenses"));
        expensesPermission.AddChild(BeroxAppPermissions.Finance.CreateExpense, L("Permission:Finance.CreateExpense"));
        expensesPermission.AddChild(BeroxAppPermissions.Finance.EditExpense, L("Permission:Finance.EditExpense"));
        expensesPermission.AddChild(BeroxAppPermissions.Finance.DeleteExpense, L("Permission:Finance.DeleteExpense"));

        financePermission.AddChild(BeroxAppPermissions.Finance.Reports, L("Permission:Finance.Reports"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BeroxAppResource>(name);
    }
}
