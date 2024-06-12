using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Swashbuckle.AspNetCore.SwaggerGen;
using Synith.Core.Injection;
using Synith.Core.Middleware;
using Synith.Core.Swagger;
using Synith.Security.Extensions;

namespace Synith.Core.Extensions;
public static class BuilderExtension
{
    public static void ConfigureSynith(this WebApplicationBuilder builder, string serviceName)
    {
        builder.ConfigureAutofac(serviceName)
               .ConfigureLogging(serviceName)
               .ConfigureServices()
               .ConfigureCors()
               .Build()
               .ConfigureApp()
               .Run();
    }

    private static WebApplicationBuilder ConfigureAutofac(this WebApplicationBuilder builder, string serviceName)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new AutofacModule(serviceName)));
        return builder;
    }

    private static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder, string serviceName)
    {
        Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        builder.Host.UseSerilog((context, configuration) =>
        {
            string? elasticServer = context.Configuration["Elastic:Server"];
            if (!string.IsNullOrEmpty(elasticServer))
            {
                configuration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticServer))
                {
                    IndexFormat = $"{serviceName.ToLower()}-logs-{DateTime.UtcNow:yyyy-MM-dd}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                });
            }

            configuration.ReadFrom.Configuration(context.Configuration);
        });
        return builder;
    }

    private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddJwtAuthentication(builder.Configuration);
        builder.Services.AddMemoryCache();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient();
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(setup =>
        {
            setup.CustomOperationIds(selector =>
            {
                return (selector.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)!.ActionName;
            });
        });

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return builder;
    }

    private static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(p => p.AddPolicy("cors", policy =>
        {
            var origins = builder.Configuration.GetSection("CorsOrigins").GetChildren().Select(x => x.Value);
            if (origins != null)
            {
                policy.WithOrigins(origins.ToArray()!).AllowAnyMethod().AllowAnyHeader();
            }
        }));

        return builder;
    }

    private static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<RequestLoggingMiddleware>();

        app.UseCors("cors");
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
