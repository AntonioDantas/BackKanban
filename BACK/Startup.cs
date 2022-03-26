using BACK.Data;
using BACK.Models;
using BACK.Repository;
using BACK.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BACK
{
    /// <summary>
    /// Início do sistema.
    /// </summary>
    public class Startup
    {
        private readonly string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: myAllowSpecificOrigins,
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "LetsCodeAntonioDantasDb"), ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            services.AddControllers();

            _ = services.AddSwaggerGen(c =>
              {
                  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kanban", Version = "v1" });
                  c.AddSecurityDefinition("jwt_auth", new OpenApiSecurityScheme
                  {
                      Description = "JWT Authorization header using the Bearer scheme",
                      Name = "Authorization",
                      Type = SecuritySchemeType.Http,
                      BearerFormat = "JWT",
                      In = ParameterLocation.Header,
                      Scheme = "bearer"
                  });

                // Make sure swagger UI requires a Bearer token specified
                  OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                  {
                      Reference = new OpenApiReference()
                      {
                          Id = "jwt_auth",
                          Type = ReferenceType.SecurityScheme,
                      },
                  };
                  OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                  {
                    {securityScheme, new string[] { } },
                  };
                  c.AddSecurityRequirement(securityRequirements);

                  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                  c.IncludeXmlComments(xmlPath);
              });

            var appAccessSection = Configuration.GetSection("AppAccess");
            services.Configure<AppAccess>(appAccessSection);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddScoped<ILogin, LoginRepository>();
            services.AddScoped<ICards, CardRepository>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = appSettings.Issuer,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            if (this.Environment.IsDevelopment())
            {
                Log.Information("Ambiente de desenvolvimento");
            }
            else
            {
                Log.Information("Ambiente de produção");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use((httpContext, next) => // For the oauth2-less!
            {
                if (httpContext.Request.Headers["X-Authorization"].Any())
                {
                    httpContext.Request.Headers.Add("Authorization", httpContext.Request.Headers["X-Authorization"]);
                }

                return next();
            });

            app.UseCors(myAllowSpecificOrigins);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kanban Api v1"));

            app.UseRouting();
            app.UseCors(myAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
