using MedicalClinicApp;
using MedicalClinicApp.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<PatientRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<DoctorScheduleRepository>();
builder.Services.AddScoped<AppointmentRepository>();
builder.Services.AddScoped<MedicalHistoryRepository>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MedicalClinicDBContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/Account/Auth");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "1"));
    options.AddPolicy("Employee", policy => policy.RequireClaim("Role", "2"));
    options.AddPolicy("Patient", policy => policy.RequireClaim("Role", "3"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Auth";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.Redirect("/Home/NotFound");
    }
});

app.Run();