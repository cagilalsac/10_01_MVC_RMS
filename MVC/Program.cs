using BLL.DAL;
using BLL.Models;
using BLL.Services;
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
// Way 3:
builder.Services.AddScoped<IRoleService, RoleService>();



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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
