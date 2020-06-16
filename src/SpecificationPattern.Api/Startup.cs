using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpecificationPattern.Application.ApplicationServices;
using SpecificationPattern.Application.ViewModelServices;
using SpecificationPattern.Core.Interfaces;
using SpecificationPattern.Infrastructure.Sql;
using SpecificationPattern.Shared.Interfaces;
using System;

namespace SpecificationPattern
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
            services.AddControllers().AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
                config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                config.ImplicitlyValidateChildProperties = true;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IShowMenuItemViewModelService, ShowMenuItemViewModelService>();
            services.AddScoped<IMenuItemViewModelService, MenuItemViewModelService>();
            services.AddScoped<IShowMenuItemService, ShowMenuItemService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
