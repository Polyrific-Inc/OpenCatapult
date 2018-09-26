// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc.ProjectGenerators
{
    internal class MainProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;

        private string Name => $"{_projectName}";

        public MainProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
        }

        public async Task<string> Initialize()
        {
            var mainProjectReferences = new string[]
            {
                _projectHelper.GetProjectFullPath($"{_projectName}.{CoreProjectGenerator.CoreProject}"),
                _projectHelper.GetProjectFullPath($"{_projectName}.{InfrastructureProjectGenerator.InfrastructureProject}")
            };
            return await _projectHelper.CreateProject($"{_projectName}", "mvc", mainProjectReferences);
        }

        public Task<string> GenerateControllers()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateViews()
        {
            return Task.FromResult("");
        }

        public Task<string> GenerateServiceInjection()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Services;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public static class ServiceInjection");
            sb.AppendLine("    {");
            sb.AppendLine("        public static void RegisterServices(this IServiceCollection services)");
            sb.AppendLine("        {");
            foreach (var model in _models)
                sb.AppendLine($"            services.AddTransient<I{model.Name}Service, {model.Name}Service>();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"ServiceInjection.cs", sb.ToString());

            return Task.FromResult("ServiceInjection generated");
        }

        public Task<string> GenerateStartupClass()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.AspNetCore.Builder;");
            sb.AppendLine("using Microsoft.AspNetCore.Hosting;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using Microsoft.Extensions.Configuration;");
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine($"using {_projectName}.{InfrastructureProjectGenerator.InfrastructureProject};");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public class Startup");
            sb.AppendLine("    {");
            sb.AppendLine("        private readonly IHostingEnvironment _hostingEnvironment;");
            sb.AppendLine();
            sb.AppendLine("        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)");
            sb.AppendLine("        {");
            sb.AppendLine("            Configuration = configuration;");
            sb.AppendLine("            _hostingEnvironment = hostingEnvironment;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public IConfiguration Configuration { get; }");
            sb.AppendLine();
            sb.AppendLine("        // This method gets called by the runtime. Use this method to add services to the container.");
            sb.AppendLine("        public void ConfigureServices(IServiceCollection services)");
            sb.AppendLine("        {");
            sb.AppendLine("            services.AddSingleton(Configuration);");
            sb.AppendLine();
            sb.AppendLine("            services.RegisterDbContext(Configuration.GetConnectionString(\"DefaultConnection\"));");
            sb.AppendLine();
            sb.AppendLine("            services.RegisterRepositories();");
            sb.AppendLine("            services.RegisterServices();");
            sb.AppendLine();
            sb.AppendLine("            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.");
            sb.AppendLine("        public void Configure(IApplicationBuilder app, IHostingEnvironment env)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (env.IsDevelopment())");
            sb.AppendLine("            {");
            sb.AppendLine("                app.UseDeveloperExceptionPage();");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                app.UseExceptionHandler(\"/Home/Error\");");
            sb.AppendLine("                app.UseHsts();");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            app.UseHttpsRedirection();");
            sb.AppendLine("            app.UseStaticFiles();");
            sb.AppendLine();
            sb.AppendLine("            app.UseMvc(routes =>");
            sb.AppendLine("            {");
            sb.AppendLine("                routes.MapRoute(");
            sb.AppendLine("                    name: \"default\"");
            sb.AppendLine("                    template: \"{controller=Home}/{action=Index}/{id?}\");");
            sb.AppendLine("            });");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Startup.cs", sb.ToString(), true);
            return Task.FromResult("Startup class generated");
        }

        public Task<string> GenerateProgramClass()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.AspNetCore;");
            sb.AppendLine("using Microsoft.AspNetCore.Hosting;");
            sb.AppendLine("using Microsoft.Extensions.Configuration;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.IO;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public class Program");
            sb.AppendLine("    {");
            sb.AppendLine("        public static void Main(string[] args)");
            sb.AppendLine("        {");
            sb.AppendLine("            CreateWebHostBuilder(args).Build().Run();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>");
            sb.AppendLine("            WebHost.CreateDefaultBuilder(args)");
            sb.AppendLine("                .UseStartup<Startup>()");
            sb.AppendLine("                .UseConfiguration(Configuration);");
            sb.AppendLine();
            sb.AppendLine("        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()");
            sb.AppendLine("            .SetBasePath(Directory.GetCurrentDirectory())");
            sb.AppendLine("            .AddJsonFile(\"appsettings.json\", optional: false, reloadOnChange: true)");
            sb.AppendLine("            .AddJsonFile(");
            sb.AppendLine("                $\"appsettings.{ Environment.GetEnvironmentVariable(\"ASPNETCORE_ENVIRONMENT\") ?? \"Production\"}.json\",");
            sb.AppendLine("                optional: true)");
            sb.AppendLine("            .AddEnvironmentVariables()");
            sb.AppendLine("            .Build();");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Program.cs", sb.ToString(), true);
            return Task.FromResult("Program class generated");
        }

        public Task<string> GenerateRepositoryInjection()
        {
            return Task.FromResult("");
        }
    }
}
