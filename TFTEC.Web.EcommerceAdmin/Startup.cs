using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Azure;
using Azure.Core.Extensions;
using Microsoft.Extensions.Options;
using System.Configuration;
using TFTEC.Web.EcommerceAdmin.Context;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web.UI;
using TFTEC.Web.EcommerceAdmin.Models;
using TFTEC.Web.EcommerceAdmin.Servicos;

namespace TFTEC.Web.EcommerceAdmin;
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
        // Add services to the container.
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //services.AddIdentity<IdentityUser, IdentityRole>()
        //     .AddEntityFrameworkStores<AppDbContext>()
        //     .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AccessDenied");

        services.Configure<ConfigurationImagens>(Configuration.GetSection("ConfigurationPastaImagens"));


        //services.AddTransient<IProdutoRepository, ProdutoRepository>();
        //services.AddTransient<ICategoriaRepository, CategoriaRepository>();
        //services.AddTransient<IPedidoRepository, PedidoRepository>();
        services.AddScoped<RelatorioVendasService>();
        services.AddScoped<GraficoVendasService>();
        services.AddRazorPages()
        .AddMicrosoftIdentityUI();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddAuthorization(options =>
        {
            // By default, all incoming requests will be authorized according to the default policy.
            //options.FallbackPolicy = options.DefaultPolicy;
        });

        services.AddControllersWithViews();

        services.AddPaging(options =>
        {
            options.ViewName = "Bootstrap4";
            options.PageParameterName = "pageindex";
        });

        //services.AddMemoryCache();
        //services.AddDistributedMemoryCache();

        services.AddSession();
        //{
        //    options.IdleTimeout = TimeSpan.FromSeconds(10);
        //    options.Cookie.HttpOnly = true;
        //    options.Cookie.IsEssential = true;
        //});
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseRouting();

        app.UseSession();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });
    }
}