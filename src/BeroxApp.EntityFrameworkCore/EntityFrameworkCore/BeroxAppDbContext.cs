using BeroxApp.Customers;
using BeroxApp.Employees;
using BeroxApp.Finance;
using BeroxApp.Reservations;
using BeroxApp.Services;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace BeroxApp.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class BeroxAppDbContext :
    AbpDbContext<BeroxAppDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion




    public DbSet<Service> Services { get; set; }
    public DbSet<ServicePriceHistory> ServicePriceHistories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeService> EmployeeServices { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    public BeroxAppDbContext(DbContextOptions<BeroxAppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(BeroxAppConsts.DbTablePrefix + "YourEntities", BeroxAppConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});


        // Service Configuration
        builder.Entity<Service>(b =>
        {
            b.ToTable(BeroxAppConsts.DbTablePrefix + "Services", BeroxAppConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(512);
            b.Property(x => x.Price).HasPrecision(18, 2);

            b.HasMany(x => x.PriceHistories).WithOne(x => x.Service).HasForeignKey(x => x.ServiceId);

            b.HasIndex(x => x.Name);
        });

        // Service Price History Configuration
        builder.Entity<ServicePriceHistory>(b =>
        {
            b.ToTable(BeroxAppConsts.DbTablePrefix + "ServicePriceHistories", BeroxAppConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.OldPrice).HasPrecision(18, 2);
            b.Property(x => x.NewPrice).HasPrecision(18, 2);

            b.HasIndex(x => x.ServiceId);
        });

        // Customer Configuration
        builder.Entity<Customer>(b =>
        {
            b.ToTable(BeroxAppConsts.DbTablePrefix + "Customers", BeroxAppConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.FirstName).IsRequired().HasMaxLength(64);
            b.Property(x => x.LastName).IsRequired().HasMaxLength(64);
            b.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);
            b.Property(x => x.Email).HasMaxLength(128);
            b.Property(x => x.Notes).HasMaxLength(1024);

            b.HasMany(x => x.Reservations).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId);

            b.HasIndex(x => x.PhoneNumber);
        });

        // Employee Configuration
        builder.Entity<Employee>(b =>
        {
            b.ToTable(BeroxAppConsts.DbTablePrefix + "Employees", BeroxAppConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.FirstName).IsRequired().HasMaxLength(64);
            b.Property(x => x.LastName).IsRequired().HasMaxLength(64);
            b.Property(x => x.PhoneNumber).HasMaxLength(20);
            b.Property(x => x.Email).HasMaxLength(128);
            b.Property(x => x.MonthlySalary).HasPrecision(18, 2);

            b.HasMany(x => x.Reservations).WithOne(x => x.Employee).HasForeignKey(x => x.EmployeeId);
            b.HasMany(x => x.EmployeeServices).WithOne(x => x.Employee).HasForeignKey(x => x.EmployeeId);

            b.HasIndex(x => x.UserId);
        });

        // Employee Service Configuration
        builder.Entity<EmployeeService>(b =>
        {
            b.ToTable(BeroxAppConsts.DbTablePrefix + "EmployeeServices", BeroxAppConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasKey(x => new { x.EmployeeId, x.ServiceId });

            b.HasOne(x => x.Employee).WithMany(x => x.EmployeeServices).HasForeignKey(x => x.EmployeeId);
            b.HasOne(x => x.Service).WithMany().HasForeignKey(x => x.ServiceId);
        });

        // Reservation Configuration
        builder.Entity<Reservation>(b =>
        {
            b.ToTable(BeroxAppConsts.DbTablePrefix + "Reservations", BeroxAppConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.ServicePrice).HasPrecision(18, 2);
            b.Property(x => x.FinalPrice).HasPrecision(18, 2);
            b.Property(x => x.DiscountAmount).HasPrecision(18, 2);
            b.Property(x => x.ExtraAmount).HasPrecision(18, 2);
            b.Property(x => x.Notes).HasMaxLength(1024);

            b.HasOne(x => x.Customer).WithMany(x => x.Reservations).HasForeignKey(x => x.CustomerId);
            b.HasOne(x => x.Employee).WithMany(x => x.Reservations).HasForeignKey(x => x.EmployeeId);
            b.HasOne(x => x.Service).WithMany().HasForeignKey(x => x.ServiceId);

            b.HasIndex(x => x.ReservationDate);
            b.HasIndex(x => x.Status);
        });

        // Expense Configuration
        builder.Entity<Expense>(b =>
        {
            b.ToTable(BeroxAppConsts.DbTablePrefix + "Expenses", BeroxAppConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Title).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(512);
            b.Property(x => x.Amount).HasPrecision(18, 2);

            b.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId);

            b.HasIndex(x => x.ExpenseDate);
            b.HasIndex(x => x.Type);
        });

    }
}
