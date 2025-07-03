using System.Threading.Tasks;
using BeroxApp.Localization;
using BeroxApp.MultiTenancy;
using BeroxApp.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace BeroxApp.Web.Menus
{
    public class BeroxAppMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<BeroxAppResource>();

            // Ana Sayfa
            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    BeroxAppMenus.Home,
                    l["Menu:Home"],
                    "~/",
                    icon: "fas fa-home",
                    order: 0
                )
            );

            // Hizmet Yönetimi
            if (await context.IsGrantedAsync(BeroxAppPermissions.Services.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        "BeroxApp.Services",
                        l["Menu:Services"],
                        url: "/Services",
                        icon: "fas fa-concierge-bell",
                        order: 1
                    )
                );
            }

            // Müşteri Yönetimi
            if (await context.IsGrantedAsync(BeroxAppPermissions.Customers.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        "BeroxApp.Customers",
                        l["Menu:Customers"],
                        url: "/Customers",
                        icon: "fas fa-users",
                        order: 2
                    )
                );
            }

            // Çalışan Yönetimi
            if (await context.IsGrantedAsync(BeroxAppPermissions.Employees.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        "BeroxApp.Employees",
                        l["Menu:Employees"],
                        url: "/Employees",
                        icon: "fas fa-user-tie",
                        order: 3
                    )
                );
            }

            // Rezervasyon Yönetimi
            if (await context.IsGrantedAsync(BeroxAppPermissions.Reservations.Default))
            {
                var reservationMenu = new ApplicationMenuItem(
                    "BeroxApp.Reservations",
                    l["Menu:Reservations"],
                    icon: "fas fa-calendar-check",
                    order: 4
                );

                reservationMenu.AddItem(
                    new ApplicationMenuItem(
                        "BeroxApp.Reservations.List",
                        l["Menu:ReservationsList"],
                        url: "/Reservations"
                    )
                );

                reservationMenu.AddItem(
                    new ApplicationMenuItem(
                        "BeroxApp.Reservations.Calendar",
                        l["Menu:ReservationsCalendar"],
                        url: "/Reservations/Calendar"
                    )
                );

                context.Menu.AddItem(reservationMenu);
            }

            // Finans Yönetimi
            if (await context.IsGrantedAsync(BeroxAppPermissions.Finance.Default))
            {
                var financeMenu = new ApplicationMenuItem(
                    "BeroxApp.Finance",
                    l["Menu:Finance"],
                    icon: "fas fa-chart-line",
                    order: 5
                );

                financeMenu.AddItem(
                    new ApplicationMenuItem(
                        "BeroxApp.Finance.Expenses",
                        l["Menu:Expenses"],
                        url: "/Finance/Expenses"
                    )
                );

                financeMenu.AddItem(
                    new ApplicationMenuItem(
                        "BeroxApp.Finance.Reports",
                        l["Menu:FinanceReports"],
                        url: "/Finance/Reports"
                    )
                );

                context.Menu.AddItem(financeMenu);
            }

            // Tenant Yönetimi
            if (MultiTenancyConsts.IsEnabled)
            {
                administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
            }
            else
            {
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            // Diğer Admin Menüleri
            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
            administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
        }
    }
}