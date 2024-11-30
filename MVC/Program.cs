using BLL.DAL;
using BLL.Models;
using BLL.Services;
using BLL.Services.Bases;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



// RMS Configuration:
// Application configuration settings can be read from sections such as AppSettings in the appsettings.json file.
// Way 1:
//builder.Configuration.GetSection("AppSettings").Bind(new AppSettings());
// Way 2:
builder.Configuration.GetSection(nameof(AppSettings)).Bind(new AppSettings()); // this method will fill the only one instance of type AppSettings with data of the
                                                                               // AppSettings section of the appsettings.json file

// IoC (Inversion of Control) Container:
// IoC Container manages the initialization operations of the objects which are injected to classes by Constructor Injection.
// Alternatively Autofac or Ninject libraries can also be used under the BLL (Business Logic Layer).
// "Unable to reslove service..." exceptions should be resolved here.
// BLL Project reference must be added to the MVC Project, therefore we can use the classes and class libraries inside the BLL Project.
builder.Services.AddDbContext<Db>(options => options // options used in the AddDbContext method is a delegate of type DbContextOptionsBuilder.
                                                     // This delegate is called an Action in C#, which doesn't return anything. Actions are generally used for configurations.
                                                     // Through the Actions, properties or methods (such as UseSqlServer method) can be used therefore the Actions
                                                     // can provide object instances to the methods (AddDbContext) which they are used in.
                                                     // We are saying that call UseSqlServer method with the provided connection string through the options Action
                                                     // for the AddDbContext method which has the generic type Db.
                                                     // We must provide the type of our Db class inherited from DbContext to the AddDbContext method.
    // Way 1: with connection string
    //.UseSqlServer("server=(localdb)\\mssqllocaldb;database=RMSDB;trusted_connection=true;"));
    // Way 2:
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // appsettings.json -> ConnectionStrings section
                                                                                    // Connection string should be read from the ConnectionStrings section of the
                                                                                    // appsettings.json file by using the connection string name (DefaultConnection).

// AddScoped: The object's reference (usually an interface or abstract class) is used to instantiate an object through constructor injection
// when a request is received and the object lives until the response is returned. AddDbContext is a scoped method by default.
// AddSingleton: The object's reference is used to instantiate an object through constructor injection when a request is received and
// the only one object lives throughout the application's lifetime (as long as the application is running).
// AddTransient: The object is instantiated every time whenever a constructor injection through the object's reference is used, independent from the request.
// Generally the AddScoped method is used.
// Way 1:
// builder.Services.AddSingleton<IRoleService, RoleService>();
// Way 2:
// builder.Services.AddTransient<IRoleService, RoleService>();
// Way 3 (Abstract Classes or Interfaces Way 1):
//builder.Services.AddScoped<IRoleService, RoleService>();
// Abstract Classes or Interfaces Way 2:
builder.Services.AddScoped<IService<Role, RoleModel>, RoleService>();

builder.Services.AddScoped<IService<User, UserModel>, UserService>();
builder.Services.AddScoped<IService<Resource, ResourceModel>, ResourceService>();

// Authentication:
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) // we are adding authentication to the project using default Cookie authentication
    .AddCookie(config => // we configure the authentication cookie to be created through the config Action delegate
    {
        config.LoginPath = "/Users/Login"; // if an operation is attempted without logging into the system, redirect the user to the Users controller's Login action
        config.AccessDeniedPath = "/Users/Login"; // if an unauthorized operation is attempted after logging into the system, redirect the user to the Users controller's Login action
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30); // allow the cookie created after logging into the system to be valid for 30 minutes, default value is 20 minutes,
                                                          // TimeSpan type is used for duration management in C#
        config.SlidingExpiration = true; // when set to true, the user's cookie expiration is extended by a specific duration every time they perform an action in the system,
                                         // if set to false, the user's cookie lifespan ends after the duration specified above after the initial login, then user needs to log in again
    });
// If authentication is added here, it must be used below after building the application.



// HTTP Service injection for HTTP related operations such as session or cookie operations:
// Session:
builder.Services.AddSession(config => // we configure the session timeout value through the config Action delegate
{
    config.IdleTimeout = TimeSpan.FromHours(1); // default value is 20 minutes
});

builder.Services.AddHttpContextAccessor(); // for the injection of object of type IHttpContextAccessor in our service classes
builder.Services.AddSingleton<HttpServiceBase, HttpService>(); // we only need one instance of HttpService object
                                                               // through the application's life-time therefore
                                                               // we use the AddSingleton method



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) // for the Production Environment (two environments are generally used: Development and Production)
{
    app.UseExceptionHandler("/Home/Error"); // don't show the exception details, redirect users to the Home/Error action so they see the Error view under Views/Shared folder
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // for using https (Secured Hypertext Transfer Protocol)
app.UseStaticFiles(); // for using files such as Javascript, CSS, HTML, etc. under wwwroot

app.UseRouting(); // for enabling the MVC routing mechanism through controllers, actions and optionally action parameters



// Authentication:
app.UseAuthentication(); // must be written to activate authentication configured above, authentication is asking "who are you?"



app.UseAuthorization(); // for enabling users perform specific operations in the system according to their priviliges, authorization is asking "what can you do?"



// Session:
app.UseSession();



app.MapControllerRoute( // mapping the default route of the application which is controller/action/id, id is optional therefore marked with "?"
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Custom routes can also be added by using the MapControllerRoute method before default routing above. 

app.Run();
