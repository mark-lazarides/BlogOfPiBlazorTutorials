using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorBlogOfPiGettingStarted.Areas.Identity;
using BlazorBlogOfPiGettingStarted.Data;
using BlazorBlogOfPiGettingStarted.Services;

namespace BlazorBlogOfPiGettingStarted
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(
              Configuration.GetConnectionString("DefaultConnection")));
      services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddRoles<IdentityRole>()  
        .AddEntityFrameworkStores<ApplicationDbContext>();
      services.AddRazorPages();
      services.AddServerSideBlazor();
      services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
      services.AddSingleton<WeatherForecastService>();
      services.AddTransient<IToDoListService, ToDoListService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
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

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });

      CreateUserAndRoles(serviceProvider).Wait();
    }

    private async Task CreateUserAndRoles(IServiceProvider serviceProvider)
    {
      // init custom roles
      var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
      string[] roleNames = { "Admin", "User" };
      IdentityResult roleResult;

      foreach (var roleName in roleNames)
      {
        var roleExists = await RoleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
          roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        }
      }
      IdentityUser user = await UserManager.FindByEmailAsync("mark@intecre.com");

      if (user == null)
      {
        user = new IdentityUser()
        {
          UserName = "mark@intecre.com",
          Email = "mark@intecre.com"
        };

        await UserManager.CreateAsync(user, "P4ssword!");
      }

      await UserManager.AddToRoleAsync(user, "Admin");

      IdentityUser user1 = await UserManager.FindByEmailAsync("jane.doe@intecre.com");

      if (user1 == null)
      {
        user1 = new IdentityUser()
        {
          UserName = "jane.doe@intecre.com",
          Email = "jane.doe@intecre.com"
        };
        await UserManager.CreateAsync(user1, "Test@246");
      }
      await UserManager.AddToRoleAsync(user1, "User");

    }
  }
}
