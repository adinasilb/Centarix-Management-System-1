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


            //Set database Connection from application json file
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));


            //add identity
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
               

            

            services.AddControllersWithViews();
            services.AddRazorPages();
           // in order to be able to customize the aspnetcore identity
            
            services.AddMvc(config => //this creates a global authorzation - meaning only registered users can use view the application
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseSession();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
           //app.UseStaticFiles();

            //app.UseRouting();

           // app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            CreateRoles(serviceProvider).Wait();
           
        
        }

        //Seed database with new roles

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Manager", "Member" };

            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                bool roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var poweruser = new ApplicationUser();
            poweruser = await UserManager.FindByEmailAsync("adinasilberberg@gmail.com");

            await UserManager.AddToRoleAsync(poweruser, "Admin");

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
        }

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
    }
}
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
