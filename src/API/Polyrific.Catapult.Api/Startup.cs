// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using CorrelationId;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Polyrific.Catapult.Api.Hubs;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Api.Infrastructure;
using Polyrific.Catapult.Shared.Common;
using Polyrific.Catapult.Shared.Common.Interface;
using Polyrific.Catapult.Shared.Dto.Constants;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Polyrific.Catapult.Api
{
    public class Startup
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _logger = loggerFactory.CreateLogger<Startup>();
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationId();

            services.AddSingleton(Configuration);

            string baseUrl = _hostingEnvironment.IsDevelopment() ? Path.Combine(_hostingEnvironment.ContentRootPath, "bin") : _hostingEnvironment.WebRootPath;
            var localTextWriter = new LocalTextWriter(baseUrl);
            services.AddSingleton<ITextWriter>(localTextWriter);

            services.RegisterDbContext(Configuration.GetConnectionString("DefaultConnection"));

            services.RegisterRepositories();
            services.RegisterServices();
            services.RegisterSecretVault(Configuration["Security:Vault:Provider"]);
            
            services.AddAppIdentity();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Security:Tokens:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["Security:Tokens:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Security:Tokens:Key"])),
                        RequireExpirationTime = false
                    };
                });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAutoMapper();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizePolicy.ProjectAccess, policy => policy.Requirements.Add(new ProjectAccessRequirement()));
                options.AddPolicy(AuthorizePolicy.ProjectOwnerAccess, policy => policy.Requirements.Add(new ProjectAccessRequirement(MemberRole.Owner)));
                options.AddPolicy(AuthorizePolicy.ProjectMaintainerAccess, policy => policy.Requirements.Add(new ProjectAccessRequirement(MemberRole.Maintainer)));
                options.AddPolicy(AuthorizePolicy.ProjectContributorAccess, policy => policy.Requirements.Add(new ProjectAccessRequirement(MemberRole.Contributor)));
                options.AddPolicy(AuthorizePolicy.ProjectMemberAccess, policy => policy.Requirements.Add(new ProjectAccessRequirement(MemberRole.Member)));
                options.AddPolicy(AuthorizePolicy.UserRoleAdminAccess, policy => policy.RequireRole(UserRole.Administrator));
                options.AddPolicy(AuthorizePolicy.UserRoleBasicAccess, policy => policy.RequireRole(UserRole.Administrator, UserRole.Basic));
                options.AddPolicy(AuthorizePolicy.UserRoleGuestAccess, policy => policy.RequireRole(UserRole.Administrator, UserRole.Basic, UserRole.Guest));
                options.AddPolicy(AuthorizePolicy.UserRoleEngineAccess, policy => policy.RequireRole(UserRole.Administrator, UserRole.Engine));
            });
            
            services.AddSingleton<IAuthorizationHandler, ProjectAccessHandler>();
            services.AddSingleton<IAuthorizationHandler, ProjectEngineAccessHandler>();

            services.AddSignalR();

            services.AddNotifications(Configuration);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "OpenCatapult API", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCorrelationId();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.ConfigureExceptionHandler(_logger, env.IsDevelopment());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenCatapult API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<JobQueueHub>("/jobQueueHub");
            });
        }
    }
}
