using Garden.Data;
using Garden.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Garden
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                    //var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    // 기본 Seed 추가
                    SeedData.Initialize(services);
                    //역할 추가
                    SeedData.SeedRoles(roleManager);
                    //관리자 추가
                    SeedData.SeedSystemAccount(userManager);
                
                }
                catch
                {

                    //var logger = services.GetRequiredService<ILogger<Program>>();
                    //logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
