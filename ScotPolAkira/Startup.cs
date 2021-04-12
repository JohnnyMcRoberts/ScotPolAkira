

namespace ScotPolAkira
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Headers;
    using Microsoft.AspNetCore.Identity;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    
    using Microsoft.Net.Http.Headers;

    using Microsoft.OpenApi.Models;
    
    using AspNet.Security.OAuth.Validation;

    using ElectionDataTypes.Settings;


    using Swashbuckle.AspNetCore.Swagger;




    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Microsoft.Net.Http.Headers;

    using AspNet.Security.OAuth.Validation;

    using Swashbuckle.AspNetCore.Swagger;

    using Microsoft.AspNetCore.Http.Headers;


    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration/*, IHostingEnvironment env*/)
        {
            Configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ViewModels.MapperVm.AutoMapperConfig.Init();

            // Configure Identity options and password complexity here
            services.Configure<IdentityOptions>(options => options.User.RequireUniqueEmail = true);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OAuthValidationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OAuthValidationDefaults.AuthenticationScheme;
            }).AddOAuthValidation();

            // Add cors
            services.AddCors();

            // Add framework services.
            //services.AddMvc(); 
            services.AddMvc(option => option.EnableEndpointRouting = false);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(
                configuration => configuration.RootPath = "ClientApp/dist");

            // Add our Config object so it can be injected
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "AngularMongoBooks3 API", Version = "v1" }
                    //new Info { Title = "AngularMongoBooks3 API", Version = "v1" }
                    );

                //c.AddSecurityDefinition("OpenID Connect", 
                //    new OpenApiSecurityScheme()
                //    {
                //        Type = SecuritySchemeType.OAuth2,
                //        Flows = new OpenApiOAuthFlows() { }
                        

                //    });
                //    new OAuth2Scheme
                //{
                //    Type = "oauth2",
                //    Flow = "password",
                //    TokenUrl = "/connect/token"
                //}
            //        );
            //
            //
            });
        }





        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {

            //loggerFactory.

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug(LogLevel.Warning);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Enforce https during production
                app.UseExceptionHandler("/Home/Error");
            }


            //Configure Cors
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = (context) =>
                {
                    ResponseHeaders headers = context.Context.Response.GetTypedHeaders();

                    headers.CacheControl = new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(365)
                    };
                }
            });
            app.UseSpaStaticFiles();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AngularMongoBooks3 API V1");
            });

            app.UseMvc(routes => routes.MapRoute(
                name: "default",
                template: "{controller}/{action=Index}/{id?}"));

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:5678");
                }
            });
        }
    }
}
