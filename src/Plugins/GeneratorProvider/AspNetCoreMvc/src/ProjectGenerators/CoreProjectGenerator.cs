// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc.ProjectGenerators
{
    internal class CoreProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;

        public const string CoreProject = "Core";

        private string Name => $"{_projectName}.{CoreProject}";

        public CoreProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
        }

        public async Task<string> Initialize()
        {
            return await _projectHelper.CreateProject(Name, "classlib");
        }

        #region Repostories
        public Task<string> GenerateRepositoryInterface()
        {
            GenerateBaseSpecificationInterface();
            GenerateBaseRepositoryInterface();
            GenerateBaseSpecificationClass();
            foreach (var model in _models)
                GenerateRepositoryInterface(model);

            return Task.FromResult($"{_models.Count} repository interface(s) generated");
        }

        private void GenerateRepositoryInterface(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using {Name}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Repositories");
            sb.AppendLine("{");
            sb.AppendLine($"    public interface I{model.Name}Repository: IRepository<{model.Name}>");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Repositories/I{model.Name}Repository.cs", sb.ToString());
        }

        private void GenerateBaseRepositoryInterface()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine($"using {Name}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Repositories");
            sb.AppendLine("{");
            sb.AppendLine("    public interface IRepository<TEntity> where TEntity : BaseEntity");
            sb.AppendLine("    {");
            sb.AppendLine("        Task<TEntity> GetById(int id, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("        Task<List<TEntity>> GetAll(CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("        Task<IEnumerable<TEntity>> GetBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("        Task<TEntity> GetSingleBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("        Task<int> CountBySpec(ISpecification<TEntity> spec, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("        Task<int> Create(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("        Task Update(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("        Task Delete(int id, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Repositories/IRepository.cs", sb.ToString());
        }

        private void GenerateBaseSpecificationInterface()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine($"using {Name}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Repositories");
            sb.AppendLine("{");
            sb.AppendLine("    public interface ISpecification<TEntity> where TEntity : BaseEntity");
            sb.AppendLine("    {");
            sb.AppendLine("        Expression<Func<TEntity, bool>> Criteria { get; }");
            sb.AppendLine("        Expression<Func<TEntity, object>> OrderBy { get; }");
            sb.AppendLine("        Expression<Func<TEntity, object>> OrderByDescending { get; }");
            sb.AppendLine("        List<Expression<Func<TEntity, object>>> Includes { get; }");
            sb.AppendLine("        List<string> IncludeStrings { get; }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Repositories/ISpecification.cs", sb.ToString());
        }

        private void GenerateBaseSpecificationClass()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine($"using {Name}.Entities;");
            sb.AppendLine($"using {Name}.Repositories;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Specifications");
            sb.AppendLine("{");
            sb.AppendLine("    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity");
            sb.AppendLine("    {");
            sb.AppendLine("        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)");
            sb.AppendLine("        {");
            sb.AppendLine("            Criteria = criteria;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> orderBy, bool orderDesc = false)");
            sb.AppendLine("        {");
            sb.AppendLine("            Criteria = criteria;");
            sb.AppendLine();
            sb.AppendLine("            if (orderDesc)");
            sb.AppendLine("            {");
            sb.AppendLine("                OrderByDescending = orderBy;");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                OrderBy = orderBy;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public Expression<Func<TEntity, bool>> Criteria { get; }");
            sb.AppendLine("        public Expression<Func<TEntity, object>> OrderBy { get; }");
            sb.AppendLine("        public Expression<Func<TEntity, object>> OrderByDescending { get; }");
            sb.AppendLine("        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();");
            sb.AppendLine("        public List<string> IncludeStrings { get; } = new List<string>();");
            sb.AppendLine();
            sb.AppendLine("        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)");
            sb.AppendLine("        {");
            sb.AppendLine("            Includes.Add(includeExpression);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        protected virtual void AddInclude(string includeString)");
            sb.AppendLine("        {");
            sb.AppendLine("            IncludeStrings.Add(includeString);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Specifications/BaseSpecification.cs", sb.ToString());
        }
        #endregion // repositories

        #region services
        public Task<string> GenerateServices()
        {
            foreach (var model in _models)
            {
                GenerateServiceInterface(model);
                GenerateServiceClass(model);
            }

            return Task.FromResult($"{_models.Count} service(s) generated");
        }

        private void GenerateServiceInterface(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Threading;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.AppendLine($"using {Name}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Services");
            sb.AppendLine("{");
            sb.AppendLine($"    public interface I{model.Name}Service");
            sb.AppendLine("    {");
            sb.AppendLine($"        Task<{model.Name}> Get{model.Name}ById(int id, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine($"        Task<List<{model.Name}>> Get{TextHelper.Pluralize(model.Name)}(CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine($"        Task<int> Create{model.Name}({model.Name} entity, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine($"        Task Update{model.Name}({model.Name} entity, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine($"        Task Delete{model.Name}(int id, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Services/I{model.Name}Service.cs", sb.ToString());
        }

        private void GenerateServiceClass(ProjectDataModelDto model)
        {
            var camelizedName = TextHelper.Camelize(model.Name);
            var sb = new StringBuilder();
            sb.AppendLine($"using System.Collections.Generic;");
            sb.AppendLine($"using System.Linq;");
            sb.AppendLine($"using System.Threading;");
            sb.AppendLine($"using System.Threading.Tasks;");
            sb.AppendLine($"using {Name}.Entities;");
            sb.AppendLine($"using {Name}.Repositories;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Services");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {model.Name}Service : I{model.Name}Service");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly I{model.Name}Repository _{camelizedName}Repository;");
            sb.AppendLine();
            sb.AppendLine($"        public {model.Name}Service(I{model.Name}Repository {camelizedName}Repository)");
            sb.AppendLine("        {");
            sb.AppendLine($"            _{camelizedName}Repository = {camelizedName}Repository;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        public async Task<{model.Name}> Get{model.Name}ById(int id, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine($"            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine($"            return await _{camelizedName}Repository.GetById(id, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        public async Task<List<{model.Name}>> Get{TextHelper.Pluralize(model.Name)}(CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine($"            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine($"            return await _{camelizedName}Repository.GetAll(cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        public async Task<int> Create{model.Name}({model.Name} entity, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine($"            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine($"            return await _{camelizedName}Repository.Create(entity, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        public async Task Update{model.Name}({model.Name} updatedEntity, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine($"            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine($"            var entity = await _{camelizedName}Repository.GetById(updatedEntity.Id, cancellationToken);");
            foreach (var property in model.Properties)
                sb.AppendLine($"            entity.{property.Name} = updatedEntity.{property.Name};");
            sb.AppendLine($"            await _{camelizedName}Repository.Update(entity, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        public async Task Delete{model.Name}(int id, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine($"            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine($"            await _{camelizedName}Repository.Delete(id, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Services/{model.Name}Service.cs", sb.ToString());
        }

        #endregion //services

        #region Models
        public Task<string> GenerateModels()
        {
            GenerateBaseModel();
            foreach (var model in _models)
                GenerateModel(model);

            return Task.FromResult($"{_models.Count} model(s) generated");
        }

        private void GenerateModel(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Entities");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {model.Name} : BaseEntity");
            sb.AppendLine("    {");

            foreach (var property in model.Properties)
            {
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

            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"Entities/{model.Name}.cs", sb.ToString(), true);
        }

        private void GenerateBaseModel()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Entities");
            sb.AppendLine("{");
            sb.AppendLine("    public abstract class BaseEntity");
            sb.AppendLine("    {");
            sb.AppendLine("        public int Id { get; set; }");
            sb.AppendLine("        public DateTime Created { get; set; }");
            sb.AppendLine("        public DateTime? Updated { get; set; }");
            sb.AppendLine("        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Entities/BaseEntity.cs", sb.ToString());
        }
        #endregion // models
    }
}
