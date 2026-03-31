using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Razor Pages ───────────────────────────────────────────────
builder.Services.AddRazorPages(options =>
{
    // Require login for all pages except public-facing ones
    options.Conventions.AuthorizeFolder("/OfficeAssistant", "OfficeAssistant");
    options.Conventions.AuthorizeFolder("/APCCF", "APCCF");
    options.Conventions.AuthorizeFolder("/FieldOfficer", "FieldOfficer");
    options.Conventions.AllowAnonymousToFolder("/Public");
    options.Conventions.AllowAnonymousToFolder("/Account");
    options.Conventions.AllowAnonymousToFolder("/Portal");
    options.Conventions.AuthorizeFolder("/Manager", "Manager");
});

// ── Database ──────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Cookie Authentication ─────────────────────────────────────
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

// ── Authorization Policies ────────────────────────────────────
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OfficeAssistant", policy =>
        policy.RequireRole("OfficeAssistant"));
    options.AddPolicy("APCCF", policy =>
        policy.RequireRole("APCCF"));
    options.AddPolicy("FieldOfficer", policy =>
        policy.RequireRole("CircleOfficer", "DivisionOfficer", "FMSOfficer"));
    options.AddPolicy("Manager", policy =>
    policy.RequireRole("Manager"));
});

// ── SMS HTTP Client ───────────────────────────────────────────
// Full URL is already in NotificationService (http://65.2.76.193/index.php/sendmsg)
// No base address needed here
builder.Services.AddHttpClient("SmsClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});

// ── Application Services ──────────────────────────────────────
builder.Services.AddScoped<IPetitionService, PetitionService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// ── Session (for OTP flow) ────────────────────────────────────
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ── Middleware Pipeline ───────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// ── Auto-migrate on startup (dev only) ───────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();