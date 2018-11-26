// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.Plugins.AspNetCoreMvc.ProjectGenerators
{
    internal class DataProjectGenerator
    {
        private readonly string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;
        private readonly ILogger _logger;
        private readonly string _adminEmail;

        public const string DataProject = "Data";

        private string Name => $"{_projectName}.{DataProject}";

        public DataProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models,
            string adminEmail, ILogger logger)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
            _adminEmail = adminEmail;
            _logger = logger;
        }

        public async Task<string> Initialize()
        {
            var dataProjectReferences = new string[]
            {
                _projectHelper.GetProjectFullPath($"{_projectName}.{CoreProjectGenerator.CoreProject}")
            };
            var dataProjectPackages = new (string, string)[]
            {
                ("AutoMapper", "7.0.1"),
                ("Microsoft.AspNetCore.Identity", "2.1.3"),
                ("Microsoft.AspNetCore.Identity.EntityFrameworkCore", "2.1.3"),
                ("Microsoft.EntityFrameworkCore.SqlServer", "2.1.1"),
                ("Microsoft.EntityFrameworkCore.Tools", "2.1.1")
            };

            return await _projectHelper.CreateProject($"{_projectName}.{DataProject}", "classlib", dataProjectReferences, dataProjectPackages);
        }

        public Task<string> GenerateEntityConfigs()
        {
            GenerateBaseEntityConfig();

            foreach (var model in _models)
                if (model.Name != CoreProjectGenerator.UserModel)
                    GenerateEntityConfig(model);

            CleanUpEntityConfigs();

            return Task.FromResult($"{_models.Count} entity config(s) generated");
        }

        private void GenerateBaseEntityConfig()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.EntityConfigs");
            sb.AppendLine("{");
            sb.AppendLine("    public abstract class BaseEntityConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity");
            sb.AppendLine("    {");
            sb.AppendLine("        public virtual void Configure(EntityTypeBuilder<TEntity> builder)");
            sb.AppendLine("        {");
            sb.AppendLine("            builder.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, "EntityConfigs/BaseEntityConfig.cs", sb.ToString());
        }

        private void GenerateEntityConfig(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.EntityConfigs");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {model.Name}Config : BaseEntityConfig<{model.Name}>");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"EntityConfigs/{model.Name}Config.cs", sb.ToString(), modelId: model.Id);
        }

        private void CleanUpEntityConfigs()
        {
            _projectHelper.CleanUpFiles(Name, "EntityConfigs", _models.Select(m => m.Id).ToArray());
        }

        public Task<string> GenerateDbContext()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.AspNetCore.Identity.EntityFrameworkCore;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine($"using {Name}.EntityConfigs;");
            sb.AppendLine($"using {Name}.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>");
            sb.AppendLine("    {");
            sb.AppendLine("        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine();

            foreach (var model in _models)
                if (model.Name != CoreProjectGenerator.UserModel)
                    sb.AppendLine($"        public virtual DbSet<{model.Name}> {TextHelper.Pluralize(model.Name)} {{ get; set; }}");            

            sb.AppendLine();
            sb.AppendLine("        protected override void OnModelCreating(ModelBuilder modelBuilder)");
            sb.AppendLine("        {");
            sb.AppendLine("            base.OnModelCreating(modelBuilder);");
            sb.AppendLine();

            foreach (var model in _models)
                if (model.Name != CoreProjectGenerator.UserModel)
                    sb.AppendLine($"            modelBuilder.ApplyConfiguration(new {model.Name}Config());");

            sb.AppendLine($"            modelBuilder.ApplyConfiguration(new ApplicationUserConfig());");
            sb.AppendLine($"            modelBuilder.ApplyConfiguration(new ApplicationRoleConfig());");
            sb.AppendLine($"            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfig());");
            sb.AppendLine($"            modelBuilder.ApplyConfiguration(new ApplicationUserClaimConfig());");
            sb.AppendLine($"            modelBuilder.ApplyConfiguration(new ApplicationUserLoginConfig());");
            sb.AppendLine($"            modelBuilder.ApplyConfiguration(new ApplicationRoleClaimConfig());");
            sb.AppendLine($"            modelBuilder.ApplyConfiguration(new ApplicationUserTokenConfig());");

            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, "ApplicationDbContext.cs", sb.ToString(), true);

            return Task.FromResult("Db context generated");
        }

        public Task<string> GenerateRepositoryClass()
        {
            GenerateBaseRepositoryClass();

            foreach (var model in _models)
                if (model.Name != CoreProjectGenerator.UserModel)
                    GenerateRepositoryClass(model);

            GenerateUserRepository();

            CleanUpRepositories();

            return Task.FromResult($"{_models.Count} repository class(es) generated");
        }

        private void GenerateRepositoryClass(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Repositories;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {model.Name}Repository : BaseRepository<{model.Name}>, I{model.Name}Repository");
            sb.AppendLine("    {");
            sb.AppendLine($"        public {model.Name}Repository(ApplicationDbContext dbContext) : base(dbContext)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"{model.Name}Repository.cs", sb.ToString(), modelId: model.Id);
        }

        private void CleanUpRepositories()
        {
            _projectHelper.CleanUpFiles(Name, "", _models.Select(m => m.Id).ToArray());
        }

        private void GenerateBaseRepositoryClass()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Threading;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Repositories;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity");
            sb.AppendLine("    {");
            sb.AppendLine("        protected readonly ApplicationDbContext Db;");
            sb.AppendLine();
            sb.AppendLine("        protected BaseRepository(ApplicationDbContext dbContext)");
            sb.AppendLine("        {");
            sb.AppendLine("            Db = dbContext;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<int> CountBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            // fetch a Queryable that includes all expression-based includes");
            sb.AppendLine("            var queryableResultWithIncludes = spec.Includes");
            sb.AppendLine("                .Aggregate(Db.Set<TEntity>().AsQueryable(),");
            sb.AppendLine("                    (current, include) => current.Include(include));");
            sb.AppendLine();
            sb.AppendLine("            // modify the IQueryable to include any string-based include statements");
            sb.AppendLine("            var secondaryResult = spec.IncludeStrings");
            sb.AppendLine("                .Aggregate(queryableResultWithIncludes,");
            sb.AppendLine("                    (current, include) => current.Include(include));");
            sb.AppendLine();
            sb.AppendLine("            return await secondaryResult.CountAsync(spec.Criteria, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<int> Create(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            entity.Created = DateTime.UtcNow;");
            sb.AppendLine("            Db.Set<TEntity>().Add(entity);");
            sb.AppendLine("            await Db.SaveChangesAsync(cancellationToken);");
            sb.AppendLine();
            sb.AppendLine("            return entity.Id;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task Delete(int id, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var dbSet = Db.Set<TEntity>();");
            sb.AppendLine("            var entity = await dbSet.FindAsync(id);");
            sb.AppendLine("            dbSet.Remove(entity);");
            sb.AppendLine("            await Db.SaveChangesAsync(cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<TEntity> GetById(int id, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            return await Db.Set<TEntity>().FindAsync(id);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<List<TEntity>> GetAll(CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            return await Db.Set<TEntity>().ToListAsync(cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<IEnumerable<TEntity>> GetBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            // fetch a Queryable that includes all expression-based includes");
            sb.AppendLine("            var queryableResultWithIncludes = spec.Includes");
            sb.AppendLine("                .Aggregate(Db.Set<TEntity>().AsQueryable(),");
            sb.AppendLine("                    (current, include) => current.Include(include));");
            sb.AppendLine();
            sb.AppendLine("            // modify the IQueryable to include any string-based include statements");
            sb.AppendLine("            var secondaryResult = spec.IncludeStrings");
            sb.AppendLine("                .Aggregate(queryableResultWithIncludes,");
            sb.AppendLine("                    (current, include) => current.Include(include));");
            sb.AppendLine();
            sb.AppendLine("            // add order by to query");
            sb.AppendLine("            if (spec.OrderBy != null)");
            sb.AppendLine("            {");
            sb.AppendLine("                secondaryResult = secondaryResult.OrderBy(spec.OrderBy);");
            sb.AppendLine("            }");
            sb.AppendLine("            else if (spec.OrderByDescending != null)");
            sb.AppendLine("            {");
            sb.AppendLine("                secondaryResult = secondaryResult.OrderByDescending(spec.OrderByDescending);");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            // return the result of the query using the specification's criteria expression");
            sb.AppendLine("            return await secondaryResult");
            sb.AppendLine("                .Where(spec.Criteria)");
            sb.AppendLine("                .ToListAsync(cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<TEntity> GetSingleBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            // fetch a Queryable that includes all expression-based includes");
            sb.AppendLine("            var queryableResultWithIncludes = spec.Includes");
            sb.AppendLine("                .Aggregate(Db.Set<TEntity>().AsQueryable(),");
            sb.AppendLine("                    (current, include) => current.Include(include));");
            sb.AppendLine();
            sb.AppendLine("            // modify the IQueryable to include any string-based include statements");
            sb.AppendLine("            var secondaryResult = spec.IncludeStrings");
            sb.AppendLine("                .Aggregate(queryableResultWithIncludes,");
            sb.AppendLine("                    (current, include) => current.Include(include));");
            sb.AppendLine();
            sb.AppendLine("            // add order by to query");
            sb.AppendLine("            if (spec.OrderBy != null)");
            sb.AppendLine("            {");
            sb.AppendLine("                secondaryResult = secondaryResult.OrderBy(spec.OrderBy);");
            sb.AppendLine("            }");
            sb.AppendLine("            else if (spec.OrderByDescending != null)");
            sb.AppendLine("            {");
            sb.AppendLine("                secondaryResult = secondaryResult.OrderByDescending(spec.OrderByDescending);");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            // return the result of the query using the specification's criteria expression");
            sb.AppendLine("            return await secondaryResult");
            sb.AppendLine("                .FirstOrDefaultAsync(spec.Criteria, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task Update(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            entity.Updated = DateTime.UtcNow;");
            sb.AppendLine("            entity.ConcurrencyStamp = Guid.NewGuid().ToString();");
            sb.AppendLine("            Db.Entry(entity).State = EntityState.Modified;");
            sb.AppendLine("            await Db.SaveChangesAsync(cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"BaseRepository.cs", sb.ToString());
        }

        #region Identities
        public Task<string> GenerateIdentityClasses()
        {
            GenerateApplicationRole();
            GenerateApplicationRoleClaim();
            GenerateApplicationUser();
            GenerateApplicationUserClaim();
            GenerateApplicationUserLogin();
            GenerateApplicationUserRole();
            GenerateApplicationUserToken();
            GenerateIdentityAutoMapperProfile();
            GenerateIdentityEntityConfigs();
            GenerateIdentityResultExtensions();
            GenerateApplicationUserClaimsPrincipalFactory();

            return Task.FromResult("Identity classes generated");
        }

        private void GenerateApplicationUser()
        {
            var userModel = _models.FirstOrDefault(m => m.Name == CoreProjectGenerator.UserModel);

            var sb = new StringBuilder();
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using Microsoft.AspNetCore.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationUser : IdentityUser<int>");
            sb.AppendLine("    {");
            sb.AppendLine("        public ApplicationUser()");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public ApplicationUser(string userName) : base(userName)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public ApplicationUser(int userId, string userEmail) : base(userEmail)");
            sb.AppendLine("        {");
            sb.AppendLine("            Id = userId;");
            sb.AppendLine("            NormalizedUserName = userEmail.ToUpper();");
            sb.AppendLine("            Email = userEmail;");
            sb.AppendLine("            NormalizedEmail = userEmail.ToUpper();");
            sb.AppendLine("        }");

            if (userModel != null && userModel.Properties != null)
            {
                foreach (var property in userModel.Properties)
                {
                    if (CoreProjectGenerator.UserModelProperties.Contains(property.Name))
                        continue;

                    if (!string.IsNullOrEmpty(property.RelatedProjectDataModelName))
                    {
                        if (property.RelationalType == PropertyRelationalType.OneToOne)
                        {
                            sb.AppendLine($"        public int {property.Name}Id {{ get; set; }}");
                            sb.AppendLine($"        public {property.RelatedProjectDataModelName} {property.Name} {{ get; set; }}");
                        }
                        else if (property.RelationalType == PropertyRelationalType.OneToMany)
                        {
                            sb.AppendLine($"        public ICollection<{property.RelatedProjectDataModelName}> {property.Name} {{ get; set; }}");
                        }
                        else if (property.RelationalType == PropertyRelationalType.ManyToMany)
                        {
                            // TODO: Implement this later as many-to-many relationship in ef core is not straightforward
                        }
                    }
                    else
                    {
                        sb.AppendLine($"        public {property.DataType} {property.Name} {{ get; set; }}");
                    }
                }
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, "Identity/ApplicationUser.cs", sb.ToString());
        }

        private void GenerateApplicationRole()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using Microsoft.AspNetCore.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationRole : IdentityRole<int>");
            sb.AppendLine("    {");
            sb.AppendLine("        public ApplicationRole()");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public ApplicationRole(string roleName) : base(roleName)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public ApplicationRole(int roleId, string roleName) : base(roleName)");
            sb.AppendLine("        {");
            sb.AppendLine("            Id = roleId;");
            sb.AppendLine("            NormalizedName = roleName.ToUpper();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, "Identity/ApplicationRole.cs", sb.ToString());
        }

        private void GenerateApplicationRoleClaim()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using Microsoft.AspNetCore.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationRoleClaim : IdentityRoleClaim<int>");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, "Identity/ApplicationRoleClaim.cs", sb.ToString());
        }

        private void GenerateApplicationUserClaim()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using Microsoft.AspNetCore.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationUserClaim : IdentityUserClaim<int>");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, "Identity/ApplicationUserClaim.cs", sb.ToString());
        }

        private void GenerateApplicationUserLogin()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using Microsoft.AspNetCore.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationUserLogin : IdentityUserLogin<int>");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, "Identity/ApplicationUserLogin.cs", sb.ToString());
        }

        private void GenerateApplicationUserRole()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using Microsoft.AspNetCore.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationUserRole : IdentityUserRole<int>");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine("        public ApplicationUserRole()");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public ApplicationUserRole(int userId, int roleId)");
            sb.AppendLine("        {");
            sb.AppendLine("            UserId = userId;");
            sb.AppendLine("            RoleId = roleId;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, "Identity/ApplicationUserRole.cs", sb.ToString());
        }

        private void GenerateApplicationUserToken()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using Microsoft.AspNetCore.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationUserToken : IdentityUserToken<int>");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, "Identity/ApplicationUserToken.cs", sb.ToString());
        }

        private void GenerateIdentityAutoMapperProfile()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine($"using AutoMapper;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class IdentityAutoMapperProfile : Profile");
            sb.AppendLine("    {");
            sb.AppendLine("        public IdentityAutoMapperProfile()");
            sb.AppendLine("        {");
            sb.AppendLine("            CreateMap<User, ApplicationUser>()");
            sb.AppendLine("              .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())");
            sb.AppendLine("              .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())");
            sb.AppendLine("	             .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore());");
            sb.AppendLine("            CreateMap<ApplicationUser, User>();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, "Identity/IdentityAutoMapperProfile.cs", sb.ToString());
        }

        private void GenerateIdentityEntityConfigs()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Constants;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>");
            sb.AppendLine("    {");
            sb.AppendLine("        public void Configure(EntityTypeBuilder<ApplicationUser> builder)");
            sb.AppendLine("        {");
            sb.AppendLine("            builder.ToTable(\"Users\");");
            sb.AppendLine();
            sb.AppendLine("            builder.HasData(CreateInitialUser());");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private ApplicationUser CreateInitialUser()");
            sb.AppendLine("        {");
            sb.AppendLine($"            var user = new ApplicationUser(1, \"{_adminEmail}\")");
            sb.AppendLine("            {");
            sb.AppendLine("                EmailConfirmed = true,");
            sb.AppendLine();
            sb.AppendLine("                // ideally these values don't need to be set here,");
            sb.AppendLine("                // it's just a workaround because of a bug in ef core 2.1 which prevents migrations to work as expected");
            sb.AppendLine("                ConcurrencyStamp = \"6e60fade-1c1f-4f6a-ab7e-768358780783\",");
            sb.AppendLine("                SecurityStamp = \"D4ZMGAXVOVP33V5FMDWVCZ7ZMH5R2JCK\"");
            sb.AppendLine("            };");
            sb.AppendLine();
            sb.AppendLine("            return user;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>");
            sb.AppendLine("    {");
            sb.AppendLine("        public void Configure(EntityTypeBuilder<ApplicationRole> builder)");
            sb.AppendLine("        {");
            sb.AppendLine("            builder.ToTable(\"Roles\");");
            sb.AppendLine();
            sb.AppendLine("            builder.HasData(");
            sb.AppendLine("                new ApplicationRole(1, UserRole.Administrator){ConcurrencyStamp = \"f8025fee-dec6-4528-9514-58339adc3383\"},");
            sb.AppendLine("                new ApplicationRole(2, UserRole.Guest) {ConcurrencyStamp = \"18f44ef4-86b2-4ebb-a400-b2615c9715e0\" }");
            sb.AppendLine("            );");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    public class ApplicationUserRoleConfig : IEntityTypeConfiguration<ApplicationUserRole>");
            sb.AppendLine("    {");
            sb.AppendLine("        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)");
            sb.AppendLine("        {");
            sb.AppendLine("            builder.ToTable(\"UserRoles\");");
            sb.AppendLine();
            sb.AppendLine("            builder.HasData(new ApplicationUserRole(1, 1));");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    public class ApplicationUserClaimConfig : IEntityTypeConfiguration<ApplicationUserClaim>");
            sb.AppendLine("    {");
            sb.AppendLine("        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)");
            sb.AppendLine("        {");
            sb.AppendLine("            builder.ToTable(\"UserClaims\");");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    public class ApplicationUserLoginConfig : IEntityTypeConfiguration<ApplicationUserLogin>");
            sb.AppendLine("    {");
            sb.AppendLine("        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)");
            sb.AppendLine("        {");
            sb.AppendLine("            builder.ToTable(\"UserLogins\");");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>");
            sb.AppendLine("    {");
            sb.AppendLine("        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)");
            sb.AppendLine("        {");
            sb.AppendLine("            builder.ToTable(\"RoleClaims\");");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    public class ApplicationUserTokenConfig : IEntityTypeConfiguration<ApplicationUserToken>");
            sb.AppendLine("    {");
            sb.AppendLine("        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)");
            sb.AppendLine("        {");
            sb.AppendLine("            builder.ToTable(\"UserTokens\");");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, "Identity/IdentityEntityConfigs.cs", sb.ToString());
        }

        private void GenerateIdentityResultExtensions()
        {

            var sb = new StringBuilder();
            sb.AppendLine($"using System;");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine("using Microsoft.AspNetCore.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public static class IdentityResultExtensions");
            sb.AppendLine("    {");
            sb.AppendLine("        public static void ThrowErrorException(this IdentityResult result)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (result.Errors.Any())");
            sb.AppendLine("            {");
            sb.AppendLine("                throw new Exception(string.Join(\" \", result.Errors.Select(err => err.Description)));");
            sb.AppendLine("	           }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, "Identity/IdentityResultExtensions.cs", sb.ToString());
        }

        private void GenerateUserRepository()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using AutoMapper;");
            sb.AppendLine("using Microsoft.AspNetCore.Identity;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Repositories;");
            sb.AppendLine($"using {Name}.Identity;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Security.Claims;");
            sb.AppendLine("using System.Threading;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public class UserRepository : IUserRepository");
            sb.AppendLine("    {");
            sb.AppendLine("        private readonly UserManager<ApplicationUser> _userManager;");
            sb.AppendLine("        private readonly SignInManager<ApplicationUser> _signInManager;");
            sb.AppendLine("        private readonly RoleManager<ApplicationRole> _roleManager;");
            sb.AppendLine("        private readonly IMapper _mapper;");
            sb.AppendLine();
            sb.AppendLine("        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IMapper mapper)");
            sb.AppendLine("        {");
            sb.AppendLine("            _userManager = userManager;");
            sb.AppendLine("            _signInManager = signInManager;");
            sb.AppendLine("            _roleManager = roleManager;");
            sb.AppendLine("            _mapper = mapper;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<Core.Entities.SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);");
            sb.AppendLine("            return _mapper.Map<Core.Entities.SignInResult>(result);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = await _userManager.FindByIdAsync(userId.ToString());");
            sb.AppendLine("            if (user != null)");
            sb.AppendLine("            {");
            sb.AppendLine("                var result = await _userManager.ConfirmEmailAsync(user, token);");
            sb.AppendLine("                if (!result.Succeeded)");
            sb.AppendLine("                    result.ThrowErrorException();");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public Task<int> CountBySpec(ISpecification<User> spec, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            throw new System.NotImplementedException();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<int> Create(User entity, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = _mapper.Map<ApplicationUser>(entity);");
            sb.AppendLine();
            sb.AppendLine("            var result = await _userManager.CreateAsync(user);");
            sb.AppendLine("            if (!result.Succeeded)");
            sb.AppendLine("                result.ThrowErrorException();");
            sb.AppendLine();
            sb.AppendLine("            return user.Id;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<int> Create(User entity, string password, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = _mapper.Map<ApplicationUser>(entity);");
            sb.AppendLine();
            sb.AppendLine("            var result = await _userManager.CreateAsync(user, password);");
            sb.AppendLine("            if (!result.Succeeded)");
            sb.AppendLine("                result.ThrowErrorException();");
            sb.AppendLine();
            sb.AppendLine("            return user.Id;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task Delete(int id, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);");
            sb.AppendLine("            if (user != null)");
            sb.AppendLine("            {");
            sb.AppendLine("                var result = await _userManager.DeleteAsync(user);");
            sb.AppendLine("                if (!result.Succeeded)");
            sb.AppendLine("                    result.ThrowErrorException();");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = await _userManager.FindByIdAsync(userId.ToString());");
            sb.AppendLine("            if (user != null)");
            sb.AppendLine("                return await _userManager.GenerateEmailConfirmationTokenAsync(user);");
            sb.AppendLine();
            sb.AppendLine("            return \"\";");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<List<User>> GetUsers(CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var users = await _userManager.Users.ToListAsync();");
            sb.AppendLine();
            sb.AppendLine("            return _mapper.Map<List<User>>(users);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<User> GetById(int id, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);");
            sb.AppendLine("            if (user != null)");
            sb.AppendLine("                return _mapper.Map<User>(user);");
            sb.AppendLine();
            sb.AppendLine("            return await Task.FromResult((User) null);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<User> GetByEmail(string email, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);");
            sb.AppendLine("            if (user != null)");
            sb.AppendLine("                return _mapper.Map<User>(user);");
            sb.AppendLine();
            sb.AppendLine("            return await Task.FromResult((User)null);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<User> GetByPrincipal(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var appUser = await _userManager.FindByNameAsync(principal.Identity.Name);");
            sb.AppendLine();
            sb.AppendLine("            return _mapper.Map<User>(appUser);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public Task<IEnumerable<User>> GetBySpec(ISpecification<User> spec, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            throw new System.NotImplementedException();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public Task<User> GetSingleBySpec(ISpecification<User> spec, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            throw new System.NotImplementedException();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);");
            sb.AppendLine();
            sb.AppendLine("            return _mapper.Map<User>(appUser);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = await _userManager.FindByNameAsync(userName);");
            sb.AppendLine("            if (user != null && user.EmailConfirmed)");
            sb.AppendLine("                return await _userManager.CheckPasswordAsync(user, password);");
            sb.AppendLine();
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("		   public async Task Update(User entity, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == entity.Id);");
            sb.AppendLine("            if (user != null)");
            sb.AppendLine("            {");
            sb.AppendLine("                user = _mapper.Map(entity, user);");
            sb.AppendLine("                await _userManager.UpdateAsync(user);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<List<User>> GetAll(CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var users = await _userManager.Users.ToListAsync();");
            sb.AppendLine();
            sb.AppendLine("            return _mapper.Map<List<User>>(users);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine("            var user = await _userManager.FindByIdAsync(userId.ToString());");
            sb.AppendLine("            return await _userManager.GeneratePasswordResetTokenAsync(user);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine("            var user = await _userManager.FindByIdAsync(userId.ToString());");
            sb.AppendLine("            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);");
            sb.AppendLine("            if (!result.Succeeded)");
            sb.AppendLine("                result.ThrowErrorException();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken)");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine("            var user = await _userManager.FindByIdAsync(userId.ToString());");
            sb.AppendLine("            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);");
            sb.AppendLine("            if (!result.Succeeded)");
            sb.AppendLine("                result.ThrowErrorException();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            _projectHelper.AddFileToProject(Name, "UserRepository.cs", sb.ToString());
        }

        private void GenerateApplicationUserClaimsPrincipalFactory()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.AspNetCore.Identity;");
            sb.AppendLine("using Microsoft.Extensions.Options;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Security.Claims;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("");
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>");
            sb.AppendLine("    {");
            sb.AppendLine("");
            sb.AppendLine("        public ApplicationUserClaimsPrincipalFactory(");
            sb.AppendLine("            UserManager<ApplicationUser> userManager,");
            sb.AppendLine("            RoleManager<ApplicationRole> roleManager,");
            sb.AppendLine("            IOptions<IdentityOptions> optionsAccessor)");
            sb.AppendLine("            : base(userManager, roleManager, optionsAccessor)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)");
            sb.AppendLine("        {");
            sb.AppendLine("            var identity = await base.GenerateClaimsAsync(user);");
            sb.AppendLine("            var roles = await UserManager.GetRolesAsync(user);");
            sb.AppendLine("            identity.AddClaim(new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? \"\"));");
            sb.AppendLine("            return identity;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, "Identity/ApplicationUserClaimsPrincipalFactory.cs", sb.ToString());
        }
        #endregion // Identities
    }
}
