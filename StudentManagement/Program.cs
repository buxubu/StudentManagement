using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Stripe;
using StudentManagement.Models;
using StudentManagement.Services.IAccount;
using StudentManagement.Services.ICourse;
using StudentManagement.Services.IFee;
using StudentManagement.Services.IStudent;
using StudentManagement.Services.IStudentFee;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<DbStuManageContext>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Admin/Accounts/Login";
        option.SlidingExpiration = true;
        option.ExpireTimeSpan = TimeSpan.FromDays(2);
    });
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();


builder.Services.AddScoped<IStudent, RepoStudent>();
builder.Services.AddScoped<ICourse, RepoCourse>();
builder.Services.AddScoped<IAccount, RepoAccount>();
builder.Services.AddScoped<IFee, RepoFee>();
builder.Services.AddScoped<IStudentFee, RepoStudentFee>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy();

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "Admin",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=HomeDashboard}/{action=Index}/{id?}"
    );

    //endpoints.MapControllerRoute(
    //  name: "Login",
    //  pattern: "{area:exists}/{controller=Accounts}/{action=Login}/{id?}"
    //);

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Homepage}/{action=Index}/{id?}");


    endpoints.MapRazorPages();
});



app.Run();
