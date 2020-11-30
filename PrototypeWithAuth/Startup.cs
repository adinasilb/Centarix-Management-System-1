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
using Microsoft.AspNetCore.Mvc.Authorization; // in order to allow for authorize filter
using Microsoft.AspNetCore.Diagnostics;
using PrototypeWithAuth.AppData;
using Microsoft.Extensions.Logging;
using PrototypeWithAuth.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace PrototypeWithAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ElixirAzureConnection")));

            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection"));
            //    options.EnableSensitiveDataLogging(true);
            //});


            services.AddControllersWithViews();


            // Enable Razor pages, but in the Debug configuration, compile the views at runtime, for ease of development
            IMvcBuilder builder = services.AddRazorPages();
            //#if DEBUG
            //            builder.AddRazorRuntimeCompilation();
            //#endif
            services.AddApplicationInsightsTelemetry();

            // in order to be able to customize the aspnetcore identity

            services.AddMvc(config => //this creates a global authorzation - meaning only registered users can use view the application
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                // config.AllowValidatingTopLevelNodes = true;
            });

            ////allow for data anotations validations
            //services.AddMvcCore()
            //   .AddDataAnnotations();
            // //.AddMvcOptions(opt =>
            // //       opt.Filters.Add<RequestFilterAttribute>());

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;

            });
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
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
           // app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //ChangePassword(serviceProvider).Wait();
            //CreateRoles(serviceProvider).Wait();
            //AddRole(serviceProvider).Wait();

            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();


        }

        //private async Task ChangePassword(IServiceProvider serviceProvider)
        //{
        //    var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    var user = await _userManager.FindByEmailAsync("adina@centarix.com");
        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //    var result = _userManager.ResetPasswordAsync(user, code, "adinabCE2063*");
        //}


        //Seed database with new roles

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames1 = Enum.GetNames(typeof(AppUtility.MenuItems)).Cast<string>().Select(x => x.ToString()).ToArray();
            string[] roleNames2 = Enum.GetNames(typeof(AppUtility.RoleItems)).Cast<string>().Select(x => x.ToString()).ToArray();
            string[] roleNames = new string[roleNames1.Length + roleNames2.Length];
            roleNames1.CopyTo(roleNames, 0);
            roleNames2.CopyTo(roleNames, roleNames1.Length);

            IdentityResult roleResult;
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            foreach (var roleName in roleNames)
            {
                bool roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //var poweruser = new ApplicationUser();
            //poweruser = await UserManager.FindByEmailAsync("adinasilberberg@gmail.com");

            //await UserManager.AddToRoleAsync(poweruser, "Admin");

            var adminuser = new Employee()
            {
                UserName = "adinasilberberg@gmail.com",
                Email = "adinasilberberg@gmail.com",
                FirstName = "Adina",
                LastName = "Gayer",
                EmailConfirmed = true,
                TwoFactorEnabled = true,
                EmployeeStatusID = 4,
                LockoutEnabled = true,
                LockoutEnd = new DateTime(2999, 01, 01),
                NeedsToResetPassword = true,
                UserNum = 2
            };
            var createAdminUser = await UserManager.CreateAsync(adminuser, "ElixirSA29873$*");
            adminuser.EmailConfirmed = true;
            var result = await UserManager.UpdateAsync(adminuser);
            if (createAdminUser.Succeeded)
            {
                await UserManager.AddToRoleAsync(adminuser, "Admin");
                await UserManager.AddToRoleAsync(adminuser, "CEO");
            }

            //var poweruser = await UserManager.FindByEmailAsync("adinasilberberg@gmail.com");
            // //{
            // //    UserName = Configuration.GetSection("UserSettings")["UserEmail"],
            // //    Email = Configuration.GetSection("UserSettings")["UserEmail"]
            // //};
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
            //  }
        }
    }
} 

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

            /* private async Task CreateRoles(IServiceProvider serviceProvider)
             {
                 var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityUser>>(); 
                 var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();//replaced Idetntiy user with application user

                 IdentityResult roleResult;
                 //here in this line we are adding Admin Role
                 var roleCheck = await RoleManager.RoleExistsAsync("Admin");
                 if (!roleCheck)
                 {
                     //here in this line we are creating admin role and seed it to the database
                     roleResult = await RoleManager.CreateAsync(new IdentityUser("Admin"));
                 }
                 //here we are assigning the Admin role to the User that we have registered above 
                 //Now, we are assinging admin role to this user("Ali@gmail.com"). When will we run this project then it will
                 //be assigned to that user.
                 IdentityUser user = await UserManager.FindByEmailAsync("faigew@gmail.com");
                 var User = new IdentityUser();
                 await UserManager.AddToRoleAsync(user, "Admin");
             }*/
        

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
