using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TaskManager.DAL.Commands;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Queries;
using TaskManager.Db;
using Swashbuckle.AspNetCore.Swagger;
namespace TaskManagerAPI
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
            services.ConfigureCors();
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options => 
                {
                    options.SuppressModelStateInvalidFilter = true;
                    //options.SuppressMapClientErrors = true;
                    })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    //JsonSerializerOptions.DefaultIgnoreCondition -> .NET 5
                }
                )
                ;
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                
               // options.
               options.SwaggerDoc("v1", info: new OpenApiInfo
               {
                   Title = "TaskManagerAPI",
                   Version = "v1",
                   Contact = new OpenApiContact
                   {
                       Name = "Timur", 
                       Email = "t1maaa@mail.ru", 
                       Url = new Uri("http://github.com/t1maaa")

                   },
                   Description = "API for coding practice purpose"
                });

               options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });
         /*   services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //
                });*/
            

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            
            RegisterCommandAndQueries(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {//TODO: ADD GraphQL, Logger, Swagger, SignalR/web sockets, кеширование запросов/. endpoint for strt and stop timetracking
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors("dev");
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DefaultModelsExpandDepth(-1);
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                options.RoutePrefix = string.Empty;

            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            /*app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
            });*/
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterCommandAndQueries(IServiceCollection service)
        {
            service.AddScoped<ICreateTaskCommand, CreateTaskCommand>();
            service.AddScoped<ITaskListQuery, TaskListQuery>();
            service.AddScoped<IDeleteTaskCommand, DeleteTaskCommand>();
        }
    }
}
