using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Monappoly_ASP.Areas.Admin.Services;
using Monappoly_ASP.Authentication.Tenant;
using Monappoly_ASP.Authentication.User;
using Monappoly_ASP.Data;
using Monappoly_ASP.Middleware;
using MonappolyLibrary;
using MonappolyLibrary.Data;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.FileManagement;
using MonappolyLibrary.GameServices.Cards;
using MonappolyLibrary.Services;
using UserClaimsPrincipalFactory = Monappoly_ASP.Authentication.User.UserClaims.UserClaimsPrincipalFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddSingleton<Monappoly_ASP.Version>();
builder.Services.AddScoped<AdminService>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; //TODO: Implement email confirmation
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

builder.Services.AddScoped<UserInfo>(sp => new UserInfo());
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory>();

var libString = builder.Configuration.GetConnectionString("LibraryConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<MonappolyDbContext>((sp, options) =>
{
    var userInfo = sp.GetRequiredService<UserInfo>();
    options.UseSqlite(libString);
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.GetServices();
builder.Services.GetGameService();

builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseUserInfo();

app.MapRazorPages();
app.MapControllers();
app.MapControllerRoute(name: "user", pattern: "{controller=User}/{action=Index}");

Defaults().Wait();
app.Run();

async Task Defaults()
{
    using var scope = app.Services.CreateScope();
    var sp = scope.ServiceProvider;
    var context = sp.GetRequiredService<ApplicationDbContext>();
    var libDb = sp.GetRequiredService<MonappolyDbContext>();
    
    var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

    await context.Database.MigrateAsync();
    await libDb.Database.MigrateAsync();
    
    app.UseUserInfo();
    
    async Task ConfirmRoleSetup(string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    
    //Create tenant:
    var noTenant = await context.Tenants.FirstOrDefaultAsync(t => t.TenantName == "NO_TENANT");
    if (noTenant == null)
    {
        noTenant = new Tenant
        {
            TenantName = "NO_TENANT",
            DateCreated = DateTime.Now,
            IsDeleted = false
        };
        await context.Tenants.AddAsync(noTenant);
        await context.SaveChangesAsync();
    }

    //Create roles:
    await ConfirmRoleSetup(UserRoles.ServerAdmin);
    await ConfirmRoleSetup(UserRoles.TenantAdmin);
    
    //Create admin user:
    var adminUser = await userManager.FindByNameAsync("systemadmin");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            Email = "jcraik23@gmail.com",
            UserName = "systemadmin",
            EmailConfirmed = true,
            TwoFactorEnabled = false,
            DisplayName = "Admin",
            TenantId = 1
        };
        await userManager.CreateAsync(adminUser);
        await userManager.AddToRoleAsync(adminUser, UserRoles.ServerAdmin);
        var p = "TempPassword23@Helperv1.2";
        await userManager.AddPasswordAsync(adminUser, p);
        Console.WriteLine("=============================");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("-----------------------------");
        Console.WriteLine(p);
        Console.WriteLine("-----------------------------");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("=============================");
    }
    else if (!await userManager.IsInRoleAsync(adminUser, UserRoles.ServerAdmin))
    {
        await userManager.AddToRoleAsync(adminUser, UserRoles.ServerAdmin);
    }
    
    //Monopoly Defaults:
    var defaults = new DefaultsService(libDb);
    await defaults.EnsureDefaults();
}