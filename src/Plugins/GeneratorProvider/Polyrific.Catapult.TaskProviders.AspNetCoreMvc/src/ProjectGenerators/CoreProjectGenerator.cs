// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.TaskProviders.AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.TaskProviders.AspNetCoreMvc.ProjectGenerators
{
    internal class CoreProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;
        private readonly ILogger _logger;
        private static readonly List<ProjectDataModelDto> _builtInModel = new List<ProjectDataModelDto>
        {
            new ProjectDataModelDto
            {
                Name = UserModel,
                Label = "User",
                Description = "User of the application",
                Properties = new List<ProjectDataModelPropertyDto>()
                {
                    new ProjectDataModelPropertyDto
                    {
                        Name = "Email",
                        Label = "Email",
                        DataType = "string",
                        ControlType = "input-text"
                    },
                    new ProjectDataModelPropertyDto
                    {
                        Name = "UserName",
                        Label = "UserName",
                        DataType = "string",
                        ControlType = "input-text"
                    }
                }
            }
        };
        private readonly string[] _baseProperties = new string[] { "Id", "Created", "Updated", "ConcurrencyStamp"  };

        public const string CoreProject = "Core";
        public const string UserModel = "User";

        private string Name => $"{_projectName}.{CoreProject}";
        public static List<string> UserModelProperties => _builtInModel.First(m => m.Name == UserModel).Properties.Select(p => p.Name).ToList();

        public CoreProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models, ILogger logger)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
            _logger = logger;

            var addedModel = _builtInModel.Where(m => !models.Any(pm => pm.Name == m.Name));
            _models = models.Concat(addedModel).ToList();
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
            {
                if (_builtInModel.Any(m => m.Name == model.Name))
                    continue;
                else
                    GenerateRepositoryInterface(model);
            }
            
            GenerateUserRepositoryInterface();

            CleanUpRepositoryInterfaces();

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

            _projectHelper.AddFileToProject(Name, $"Repositories/I{model.Name}Repository.cs", sb.ToString(), modelId: model.Id);
        }

        private void GenerateUserRepositoryInterface()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Security.Claims;");
            sb.AppendLine("using System.Threading;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine($"using {Name}.Entities;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Repositories");
            sb.AppendLine("{");
            sb.AppendLine("    public interface IUserRepository : IRepository<User>");
            sb.AppendLine("    {");
            sb.AppendLine("        Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<User> GetByEmail(string email, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<int> Create(User entity, string password, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<User> GetByPrincipal(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<List<User>> GetUsers(CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"Repositories/IUserRepository.cs", sb.ToString());
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

        private void CleanUpRepositoryInterfaces()
        {
            _projectHelper.CleanUpFiles(Name, "Repositories", _models.Select(m => m.Id).ToArray());
        }
        #endregion // repositories

        #region services
        public Task<string> GenerateServices()
        {
            foreach (var model in _models)
            {
                if (_builtInModel.Any(m => m.Name == model.Name))
                    continue;

                GenerateServiceInterface(model);
                GenerateServiceClass(model);
            }

            GenerateUserServiceInterface();
            GenerateUserServiceClass();

            CleanUpServices();

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

            _projectHelper.AddFileToProject(Name, $"Services/Interface/I{model.Name}Service.cs", sb.ToString(), modelId: model.Id);
        }

        private void GenerateUserServiceInterface()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using {Name}.Entities;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Security.Claims;");
            sb.AppendLine("using System.Threading;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Services");
            sb.AppendLine("{");
            sb.AppendLine("    public interface IUserService");
            sb.AppendLine("    {");
            sb.AppendLine("        Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<User> CreateUser(string email, string password, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<User> CreateUser(User user, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task UpdateUser(User user, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task DeleteUser(int userId, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<User> GetUserById(int userId, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<int> GetUserId(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<string> GetUserEmail(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<User> GetUser(string userName, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<List<User>> GetUsers(CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine();
            sb.AppendLine("        Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken));");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            _projectHelper.AddFileToProject(Name, $"Services/Interface/IUserService.cs", sb.ToString());
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

            _projectHelper.AddFileToProject(Name, $"Services/{model.Name}Service.cs", sb.ToString(), modelId: model.Id);
        }

        private void GenerateUserServiceClass()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using {Name}.Entities;");
            sb.AppendLine($"using {Name}.Repositories;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Security.Claims;");
            sb.AppendLine("using System.Threading;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Services");
            sb.AppendLine("{");
            sb.AppendLine("    public class UserService : IUserService");
            sb.AppendLine("    {");
            sb.AppendLine("        private readonly IUserRepository _userRepository;");
            sb.AppendLine();
            sb.AppendLine("        public UserService(IUserRepository userRepository)");
            sb.AppendLine("        {");
            sb.AppendLine("            _userRepository = userRepository;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            return _userRepository.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            await _userRepository.ConfirmEmail(userId, token, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<User> CreateUser(string email, string password, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = new User");
            sb.AppendLine("            {");
            sb.AppendLine("                UserName = email,");
            sb.AppendLine("                Email = email");
            sb.AppendLine("            };");
            sb.AppendLine();
            sb.AppendLine("            var id = await _userRepository.Create(user, password, cancellationToken);");
            sb.AppendLine("            if (id > 0)");
            sb.AppendLine("                user.Id = id;");
            sb.AppendLine();
            sb.AppendLine("            return user;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<User> CreateUser(User user, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var id = await _userRepository.Create(user, null, cancellationToken);");
            sb.AppendLine("            if (id > 0)");
            sb.AppendLine("                user.Id = id;");
            sb.AppendLine();
            sb.AppendLine("            return user;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task DeleteUser(int userId, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            await _userRepository.Delete(userId, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            return _userRepository.GenerateConfirmationToken(userId, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<User> GetUser(string userName, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            return await _userRepository.GetByUserName(userName, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<User> GetUserById(int userId, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            return await _userRepository.GetById(userId, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            return await _userRepository.GetByEmail(email, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<int> GetUserId(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = await _userRepository.GetByPrincipal(principal, cancellationToken);");
            sb.AppendLine();
            sb.AppendLine("            return user?.Id ?? 0;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<string> GetUserEmail(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            var user = await _userRepository.GetByPrincipal(principal, cancellationToken);");
            sb.AppendLine();
            sb.AppendLine("            return user?.Email;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<List<User>> GetUsers(CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine();
            sb.AppendLine("            return await _userRepository.GetUsers();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine("            await _userRepository.ResetPassword(userId, token, newPassword, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine("            return await _userRepository.GetResetPasswordToken(userId, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            cancellationToken.ThrowIfCancellationRequested();");
            sb.AppendLine("            await _userRepository.UpdatePassword(userId, oldPassword, newPassword, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task UpdateUser(User user, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            await _userRepository.Update(user, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("        {");
            sb.AppendLine("            return await _userRepository.ValidateUserPassword(userName, password, cancellationToken);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"Services/UserService.cs", sb.ToString());
        }

        private void CleanUpServices()
        {
            _projectHelper.CleanUpFiles(Name, "Services", _models.Select(m => m.Id).ToArray());
        }
        #endregion //services

        #region Models
        public Task<string> GenerateModels()
        {
            GenerateBaseModel();
            foreach (var model in _models)
                GenerateModel(model);

            GenerateUserRoleConstant();
            GenerateSignInResult();

            CleanUpModels();

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
                if (_baseProperties.Contains(property.Name))
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

            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"Entities/{model.Name}.cs", sb.ToString(), modelId: model.Id);
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
        
        private void GenerateUserRoleConstant()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"namespace {Name}.Constants");
            sb.AppendLine("{");
            sb.AppendLine("    public static class UserRole");
            sb.AppendLine("    {");
            sb.AppendLine("        public const string Administrator = \"Administrator\";");
            sb.AppendLine("        public const string Guest = \"Guest\";");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Constants/UserRole.cs", sb.ToString());
        }

        private void GenerateSignInResult()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"namespace {Name}.Entities");
            sb.AppendLine("{");
            sb.AppendLine("    public class SignInResult");
            sb.AppendLine("    {");
            sb.AppendLine("        public bool Succeeded { get; set; }");
            sb.AppendLine("");
            sb.AppendLine("        public bool IsLockedOut { get; set; }");
            sb.AppendLine("");
            sb.AppendLine("        public bool IsNotAllowed { get; set; }");
            sb.AppendLine("");
            sb.AppendLine("        public bool RequiresTwoFactor { get; set; }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"Entities/SignInResult.cs", sb.ToString());
        }

        private void CleanUpModels()
        {
            _projectHelper.CleanUpFiles(Name, "Entities", _models.Select(m => m.Id).ToArray());
        }
        #endregion // models
    }
}
