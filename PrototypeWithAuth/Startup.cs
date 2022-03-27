using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization; //in order to allow for authirization policy builder
using Microsoft.AspNetCore.Mvc.Authorization; // in order to allow for authorize 
using Microsoft.AspNetCore.Diagnostics;
using PrototypeWithAuth.AppData;
using Microsoft.Extensions.Logging;
using PrototypeWithAuth.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Diagnostics;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using React.AspNet;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.IO;

namespace PrototypeWithAuth
{
    public class Startup
    {
        private const String ConfirmEmailProvider = "CustomEmailConfirmation";
        private readonly IWebHostEnvironment _hostEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddConsole()
            //        .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
            //    loggingBuilder.AddDebug();
            //});

            ////Set database Connection from application json file

            //add identity

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Tokens.ProviderMap.Add(ConfirmEmailProvider,
                    new TokenProviderDescriptor(
                        typeof(CustomEmailConfirmationTokenProvider<IdentityUser>)));
                options.Tokens.EmailConfirmationTokenProvider = ConfirmEmailProvider;
            }).AddDefaultTokenProviders()
    .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>(ConfirmEmailProvider)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("ElixirAzureConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DevelopersDB"), sqlServerOptions => sqlServerOptions.CommandTimeout(120));
                    options.EnableSensitiveDataLogging(true);   
            });

            services.AddControllersWithViews();
            services.AddReact();
            
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName)
            .AddV8();

            // Add framework services.
            services.AddMvc();

            // Enable Razor pages, but in the Debug configuration, compile the views at runtime, for ease of development
            IMvcBuilder builder = services.AddRazorPages();
#if DEBUG
            builder.AddRazorRuntimeCompilation();
#endif
            //services.AddApplicationInsightsTelemetry();

            // in order to be able to customize the aspnetcore identity
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => false; // consent required
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddSession(opts =>
            //{
            //    opts.Cookie.IsEssential = true;
            //    opts.IdleTimeout = TimeSpan.FromHours(10);
            //    opts.IOTimeout = TimeSpan.FromHours(10);
            //    //opts.Cookie.HttpOnly = false;
            //    //opts.Cookie.Name = "Sessions" + Guid.NewGuid().ToString();
            //});
            services.AddMvc(config => //this creates a global authorzation - meaning only registered users can use view the application
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                // config.AllowValidatingTopLevelNodes = true;
            });
            services.AddDistributedMemoryCache();
            services.AddSession();
            ////allow for data anotations validations
            //services.AddMvcCore()
            //   .AddDataAnnotations();
            // //.AddMvcOptions(opt =>
            // //       opt.Filters.Add<RequestFilterAttribute>());


            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(5);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie.Name = "LoginCookie";
            });

            //CookieOptions cookieOptions = new CookieOptions();
            //string GUID = new Guid().ToString();
            //string Key = "SessionCookie";

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<CustomEmailConfirmationTokenProvider<IdentityUser>>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles(); //may be here for other reasons but also need to download pdf files



            app.UseReact(config =>
            {
                config.SetLoadBabel(true)
                  .SetLoadReact(false)
                  .SetReactAppBuildPath("~/dist");
                config.UseServerSideRendering = false;
                config.JsonSerializerSettings = new JsonSerializerSettings
                {
                    Converters= new List<JsonConverter> { new StringEnumConverter() },
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });



            //ChangePassword(serviceProvider).Wait();

            CreateRoles(serviceProvider).Wait();

            if (_hostEnvironment.IsDevelopment())
            {
                WriteAppUtilityJsx();
                WriteElixirStringsJsx();
            }


            //AddRoles(serviceProvider).Wait();

            //app.UseApplicationInsightsRequestTelemetry();
            //app.UseApplicationInsightsExceptionTelemetry();
        }

        //private async Task ChangePassword(IServiceProvider serviceProvider)
        //{
        //    var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    var user = await _userManager.FindByEmailAsync("debbie@centarix.com");
        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //    var result = await _userManager.ResetPasswordAsync(user, code, "Centarix.2020");
        //}


        //Seed database with new roles

        //private async Task AddRoles(IServiceProvider serviceProvider)
        //{
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    var user = await UserManager.FindByEmailAsync("adina@centarix.com");
        //    await UserManager.AddToRoleAsync(user, "LabManagement");
        //    ////await UserManager.AddToRoleAsync(user, "CEO");
        //    //var code = await UserManager.GeneratePasswordResetTokenAsync(user);
        //    //var result = await UserManager.ResetPasswordAsync(user, code, "adinabCE2063*");
        //}

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            List<string> roleNames = AppUtility.RequestRoleEnums().Select(x => x.RoleDefinition).ToList();
            roleNames.Concat(AppUtility.OperationRoleEnums().Select(x => x.RoleDefinition).ToList());
            roleNames.Concat(AppUtility.ProtocolRoleEnums().Select(x => x.RoleDefinition).ToList());
            roleNames.Concat(AppUtility.BiomarkerRoleEnums().Select(x => x.RoleDefinition).ToList());
            roleNames.Concat(AppUtility.TimekeeperRoleEnums().Select(x => x.RoleDefinition).ToList());
            roleNames.Concat(AppUtility.LabManagementRoleEnums().Select(x => x.RoleDefinition).ToList());
            roleNames.Concat(AppUtility.AccountingRoleEnums().Select(x => x.RoleDefinition).ToList());
            roleNames.Concat(AppUtility.ReportsRoleEnums().Select(x => x.RoleDefinition).ToList());
            roleNames.Concat(AppUtility.IncomeRoleEnums().Select(x => x.RoleDefinition).ToList());
            roleNames.Concat(AppUtility.UsersRoleEnums().Select(x => x.RoleDefinition).ToList());

            IdentityResult roleResult;
            //var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            //if (!roleCheck)
            //{
            //    roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            //}
            foreach (var roleName in roleNames)
            {
                bool roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            await CreateAdminUser(serviceProvider, "adinasilberberg@gmail.com", "ElixirTestSA2063*", "Adina", "Gayer");

            //string[] roleNames1 = Enum.GetNames(typeof(AppUtility.MenuItems)).Cast<string>().Select(x => x.ToString()).ToArray();
            //string[] roleNames2 = Enum.GetNames(typeof(AppUtility.RoleItems)).Cast<string>().Select(x => x.ToString()).ToArray();
            //string[] roleNames = new string[roleNames1.Length + roleNames2.Length];
            //roleNames1.CopyTo(roleNames, 0);
            //roleNames2.CopyTo(roleNames, roleNames1.Length);

            //IdentityResult roleResult;
            //var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            //if (!roleCheck)
            //{
            //    roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            //}
            //foreach (var roleName in roleNames)
            //{
            //    bool roleExist = await RoleManager.RoleExistsAsync(roleName);
            //    if (!roleExist)
            //    {
            //        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
            //    }
            //}
            //var poweruser = new ApplicationUser();
            //poweruser = await UserManager.FindByEmailAsync("adinasilberberg@gmail.com");

            //await UserManager.AddToRoleAsync(poweruser, "Admin");



            //var poweruser = await UserManager.FindByEmailAsync("adinasilberberg@gmail.com");
            ////{
            ////    UserName = Configuration.GetSection("UserSettings")["UserEmail"],
            ////    Email = Configuration.GetSection("UserSettings")["UserEmail"]
            ////};
            //string UserPassword = /*Configuration.GetSection("UserSettings")["UserEmail"]*/ "adinabCE2063*!";
            //var _user = await UserManager.FindByEmailAsync("adinasilberberg@gmail.com");
            //if (_user == null)
            //{
            //    var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
            //    if (createPowerUser.Succeeded)
            //    {
            //        await UserManager.AddToRoleAsync(poweruser, "Admin");
            //    }
            //}
        }

        private async Task CreateAdminUser(IServiceProvider serviceProvider, String email, String password, String FirstName, String LastName)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminuser = new Employee()
            {
                UserName = email,
                Email = email,
                FirstName = "Adina",
                LastName = "Gayer",
                EmailConfirmed = true,
                TwoFactorEnabled = true,
                EmployeeStatusID = 4,
                LockoutEnabled = true,
                LockoutEnd = new DateTime(2999, 01, 01),
                NeedsToResetPassword = true,
                UserNum = 1,
                IsUser = true,
            };
            var createAdminUser = await UserManager.CreateAsync(adminuser, password);
            adminuser.EmailConfirmed = true;
            var result = await UserManager.UpdateAsync(adminuser);
            if (createAdminUser.Succeeded)
            {
                await UserManager.AddToRoleAsync(adminuser, "Users");
            }
        } 

        private void WriteAppUtilityJsx()
        {
            var type = Type.GetType("PrototypeWithAuth.AppData.AppUtility");
            var enums = Assembly.Load("PrototypeWithAuth").GetTypes().Where(t => t.IsEnum && t.DeclaringType == type);
            string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "ReactViews\\Constants");

            string fileName = Path.Combine(uploadFolder, "AppUtility.jsx");

            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                foreach (var e in enums)
                {
                    writer.WriteLine("export const " + e.Name + " = {");
                    foreach (var value in e.GetEnumValues())
                    {
                        writer.WriteLine(Enum.GetName(e, value) + " : '" + Enum.GetName(e, value) + "',");
                    }
                    writer.WriteLine("}");
                    writer.WriteLine();
                }
            }

        }

        private void WriteElixirStringsJsx()
        {
            var type = Type.GetType("PrototypeWithAuth.AppData.UtilityModels.ElixirStrings");
            var elixirStrings = Assembly.Load("PrototypeWithAuth").GetTypes().Where(t => t.FullName == type.FullName).FirstOrDefault().GetMembers().Where(e => e.MemberType == MemberTypes.Field);

            string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "ReactViews\\Constants");

            string fileName = Path.Combine(uploadFolder, "ElixirStrings.jsx");

            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                foreach (var es in elixirStrings)
                {
                    writer.WriteLine("export const " + es.Name + " = '" + ((FieldInfo)es).GetValue(typeof(String)) + "';");
                }
            }

        }
        //

        // var poweruser = await UserManager.FindByEmailAsync("faigew@gmail.com");
        // //{
        // //    UserName = Configuration.GetSection("UserSettings")["UserEmail"],
        // //    Email = Configuration.GetSection("UserSettings")["UserEmail"]
        // //};
        //string UserPassword = /*Configuration.GetSection("UserSettings")["UserEmail"]*/ "AkivaH1!";
        // var _user = await UserManager.FindByEmailAsync("faigew@gmail.com");
        // if (_user == null)
        // {
        //     var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
        //     if (createPowerUser.Succeeded)
        //     {
        //         await UserManager.AddToRoleAsync(poweruser, "Admin");
        //     }
        // }
        //  }

        //private async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();//replaced Idetntiy user with application user

        //    IdentityResult roleResult;
        //    //here in this line we are adding Admin Role
        //    var roleCheck = await RoleManager.RoleExistsAsync("Users");
        //    if (!roleCheck)
        //    {
        //        //here in this line we are creating admin role and seed it to the database
        //        roleResult = await RoleManager.CreateAsync(new IdentityRole { Name = "Users" });
        //    }
        //    //here we are assigning the Admin role to the User that we have registered above
        //    //Now, we are assinging admin role to this user("Ali@gmail.com").When will we run this project then it will
        //    //be assigned to that user.
        //    ApplicationUser user = await UserManager.FindByEmailAsync("adinagayer55@gmail.com");
        //    var User = new IdentityUser();
        //    await UserManager.AddToRoleAsync(user, "Users");
        //}


        /*              app.UseRouting();
                        app.UseAuthentication();
                        app.UseAuthorization();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=Home}/{action=Index}/{id?}");
                            endpoints.MapRazorPages();
                        });
                        app.UseEndpoints();*/
    }

}
