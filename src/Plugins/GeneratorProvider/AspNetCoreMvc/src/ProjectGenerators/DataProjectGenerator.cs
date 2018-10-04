// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc.ProjectGenerators
{
    internal class DataProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;
        private readonly ILogger _logger;

        public const string DataProject = "Data";

        private string Name => $"{_projectName}.{DataProject}";

        public DataProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models, ILogger logger)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
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
                ("Microsoft.EntityFrameworkCore.SqlServer", "2.1.1"),
                ("Microsoft.EntityFrameworkCore.Tools", "2.1.1")
            };

            return await _projectHelper.CreateProject($"{_projectName}.{DataProject}", "classlib", dataProjectReferences, dataProjectPackages);
        }

        public Task<string> GenerateEntityConfigs()
        {
            GenerateBaseEntityConfig();
            foreach (var model in _models)
                GenerateEntityConfig(model);

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

            _projectHelper.AddFileToProject(Name, $"EntityConfigs/{model.Name}Config.cs", sb.ToString());
        }

        public Task<string> GenerateDbContext()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine($"using {Name}.EntityConfigs;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public class ApplicationDbContext : DbContext");
            sb.AppendLine("    {");
            sb.AppendLine("        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine();

            foreach (var model in _models)
                sb.AppendLine($"        public virtual DbSet<{model.Name}> {TextHelper.Pluralize(model.Name)} {{ get; set; }}");            

            sb.AppendLine();
            sb.AppendLine("        protected override void OnModelCreating(ModelBuilder modelBuilder)");
            sb.AppendLine("        {");
            sb.AppendLine("            base.OnModelCreating(modelBuilder);");
            sb.AppendLine();

            foreach (var model in _models)
                sb.AppendLine($"            modelBuilder.ApplyConfiguration(new {model.Name}Config());");

            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, "ApplicationDbContext.cs", sb.ToString());

            return Task.FromResult("Db context generated");
        }

        public Task<string> GenerateRepositoryClass()
        {
            GenerateBaseRepositoryClass();
            foreach (var model in _models)
                GenerateRepositoryClass(model);

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

            _projectHelper.AddFileToProject(Name, $"{model.Name}Repository.cs", sb.ToString());
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
    }
}
