// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc.ProjectGenerators
{
    internal class InfrastructureProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;
        private readonly ILogger _logger;

        public const string InfrastructureProject = "Infrastructure";

        private string Name => $"{_projectName}.{InfrastructureProject}";

        public InfrastructureProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models, ILogger logger)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
            _logger = logger;
        }

        public async Task<string> Initialize()
        {
            var infrastructureProjectReferences = new string[]
            {
                _projectHelper.GetProjectFullPath($"{_projectName}.{CoreProjectGenerator.CoreProject}"),
                _projectHelper.GetProjectFullPath($"{_projectName}.{DataProjectGenerator.DataProject}")
            };

            var infrastructureProjectPackages = new (string, string)[]
            {
                ("Microsoft.AspNetCore.Identity", "2.1.3"),
                ("Microsoft.AspNetCore.Identity.EntityFrameworkCore", "2.1.3"),
                ("Microsoft.AspNetCore.Identity.UI", "2.1.3")
            };

            return await _projectHelper.CreateProject($"{_projectName}.{InfrastructureProject}", "classlib", infrastructureProjectReferences, infrastructureProjectPackages);
        }

        public Task<string> GenerateDbContextInjection()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine($"using {_projectName}.{DataProjectGenerator.DataProject};");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public static class DbContextInjection");
            sb.AppendLine("    {");
            sb.AppendLine("        public static void RegisterDbContext(this IServiceCollection services, string connectionString)");
            sb.AppendLine("        {");
            sb.AppendLine("            services.AddDbContext<ApplicationDbContext>(options =>");
            sb.AppendLine("            {");
            sb.AppendLine("                options.UseSqlServer(connectionString);");
            sb.AppendLine("            });");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"DbContextInjection.cs", sb.ToString());

            return Task.FromResult("DbContextInjection generated");
        }

        public Task<string> GenerateRepositoryInjection()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Repositories;");
            sb.AppendLine($"using {_projectName}.{DataProjectGenerator.DataProject};");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public static class RepositoryInjection");
            sb.AppendLine("    {");
            sb.AppendLine("        public static void RegisterRepositories(this IServiceCollection services)");
            sb.AppendLine("        {");
            foreach (var model in _models)
                sb.AppendLine($"            services.AddScoped<I{model.Name}Repository, {model.Name}Repository>();");

            sb.AppendLine($"            services.AddScoped<IUserRepository, UserRepository>();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"RepositoryInjection.cs", sb.ToString());

            return Task.FromResult("RepositoryInjection generated");
        }

        public Task<string> GenerateIdentityIjection()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.AspNetCore.Identity;");
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine($"using {_projectName}.{DataProjectGenerator.DataProject};");
            sb.AppendLine($"using {_projectName}.{DataProjectGenerator.DataProject}.Identity;");
            sb.AppendLine("");
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public static class IdentityInjection");
            sb.AppendLine("    {");
            sb.AppendLine("        public static void AddAppIdentity(this IServiceCollection services)");
            sb.AppendLine("        {");
            sb.AppendLine("            services.AddDefaultIdentity<ApplicationUser>()");
            sb.AppendLine("                .AddRoles<ApplicationRole>()");
            sb.AppendLine("                .AddEntityFrameworkStores<ApplicationDbContext>();");
            sb.AppendLine();
            sb.AppendLine("            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"IdentityInjection.cs", sb.ToString());

            return Task.FromResult("IdentityInjection generated");
        }
    }
}
