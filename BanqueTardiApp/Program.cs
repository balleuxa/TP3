using BanqueTardiApp.Donnees;
using BanqueTardiApp.Services.Implementations;
using BanqueTardiApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BanqueTardiApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IDbInitalisateurService, DbInitialisateurService>();
            builder.Services.AddScoped<ICompteBancaireService, CompteBancaireService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();

            builder.Services.AddDbContext<BanqueContexte>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var initialisateur = scope.ServiceProvider.GetRequiredService<IDbInitalisateurService>();
                await initialisateur.ReinitialiserAsync();
            }

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
                pattern: "{controller=Clients}/{action=Index}");

            await app.RunAsync();
        }
    }
}
