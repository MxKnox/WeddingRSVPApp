using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Components;
using WeddingApp.Components.Account;
using WeddingApp.Data;
using WeddingApp.Data.Auditing;
using WeddingApp.Identity;
using WeddingApp.Mail;
using WeddingApp.Options;

namespace WeddingApp;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<WeddingAppUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
            })
            .AddMicrosoftAccount(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"] ?? "";
                options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"] ?? "";
            })
            .AddFacebook(options =>
            {
                options.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? "";
                options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? "";
            })
            .AddIdentityCookies();

// Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException(
                                   "Connection string 'DefaultConnection' not found.");
        
// Add HttpContextAccessor for audit interceptor
        builder.Services.AddHttpContextAccessor();

// Register the audit interceptor
        builder.Services.AddScoped<AuditInterceptor>();

// Configure DbContext with audit interceptor
        builder.Services.AddDbContext<WeddingDbContext>((serviceProvider, options) =>
        {
            options.UseSqlite(connectionString);

            // Add the audit interceptor
            var auditInterceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
            options.AddInterceptors(auditInterceptor);
        });

// Add Identity services
        builder.Services.AddIdentityCore<WeddingAppUser>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddUserManager<UserManager<WeddingAppUser>>()
            .AddRoles<IdentityRole>()
            .AddSignInManager<SignInManager<WeddingAppUser>>()
            .AddEntityFrameworkStores<WeddingDbContext>()
            .AddDefaultTokenProviders();

// Add Email Service (simplified example - you'd implement IEmailSender)
        builder.Services.AddTransient<IEmailSender<WeddingAppUser>, EmailService>();

// Configure Email Service based on provider
        builder.Services.Configure<WeddingOptions>(builder.Configuration.GetSection("Wedding"));
        builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
        builder.Services.Configure<GoogleMapsOptions>(builder.Configuration.GetSection("GoogleMaps"));
        builder.Services.AddScoped<IConfirmationService, ConfirmationService>();
        builder.Services.AddRazorComponents()
            
            .AddInteractiveServerComponents();

// Add Server-side Blazor services
        builder.Services.AddServerSideBlazor();

// Add Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("GuestPolicy", policy =>
                policy.RequireRole(
                    nameof(Roles.Guest),
                    nameof(Roles.Planner),
                    nameof(Roles.Admin)
                )
            );

            options.AddPolicy("PlannerPolicy", policy =>
                policy.RequireRole(
                    nameof(Roles.Planner),
                    nameof(Roles.Admin)
                )
            );

            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireRole( nameof(Roles.Admin)));
        });

// Configure CORS if needed
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                var origins = builder.Configuration.GetSection("Security:CorsOrigins").Get<string[]>() ??
                              Array.Empty<string>();
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            
            app.UseHsts();
            app.UseHttpsRedirection();
        }
        else
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }
        
        app.UseHttpsRedirection();
        
        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            
            .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        await app.SeedIdentity();

        await app.RunAsync();
        
    }
}