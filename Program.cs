using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AgentManagementAPI.Controllers;
using AgentManagementAPI.Data;
using AgentManagementAPI.Middlewares.Global;
using AgentManagementAPI.Services;

namespace AgentManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AgentManagementAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")).UseValidationCheckConstraints());



            // Add services to the container.
            builder.Services.AddScoped<TargetMissionsCreator>();
            builder.Services.AddScoped<AgentMissionsCreator>();
            builder.Services.AddScoped<StatusUpdateMission>();
            builder.Services.AddScoped<ModelSearchor>();
            builder.Services.AddScoped<MoveService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
     


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // app.UseHttpsRedirection();
            app.UseMiddleware<GlobalLoggingMiddleware>();

            app.UseWhen(
                context =>
                context.Request.Path.StartsWithSegments("/Login"),
                appBuilder =>
                {
                    //appBuilder.UseMiddleware<JwtValidationMiddleware>();
                    appBuilder.UseMiddleware<GlobalLoggingMiddleware>();
                });
            app.MapControllers();



            app.Run();
        }
    }
}
