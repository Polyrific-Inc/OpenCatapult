// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.TaskProviders.AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace Polyrific.Catapult.TaskProviders.AspNetCoreMvc.ProjectGenerators
{
    internal class MainProjectGenerator
    {
        private string _projectName;
        private string _outputLocation;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;
        private readonly ILogger _logger;

        private string Name => $"{_projectName}";

        private const string _connectionString = "Server=localhost;Database=opencatapult;User ID=sa;Password=samprod;";

        public MainProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models, string outputLocation, ILogger logger)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
            _outputLocation = outputLocation;
            _logger = logger;
        }

        public async Task<string> Initialize()
        {
            var mainProjectPackages = new (string, string)[]
            {
                ("AutoMapper", "7.0.1"),
                ("AutoMapper.Extensions.Microsoft.DependencyInjection", "5.0.1"),
                ("MailKit", "2.0.6")
            };

            var message = await _projectHelper.CreateProject($"{_projectName}", "mvc", null, mainProjectPackages);
            AddLogo();
            ModifyHomePage();

            return message;
        }

        public async Task AddProjectReferences()
        {
            var mainProjectReferences = new string[]
            {
                _projectHelper.GetProjectFullPath($"{_projectName}.{CoreProjectGenerator.CoreProject}"),
                _projectHelper.GetProjectFullPath($"{_projectName}.{InfrastructureProjectGenerator.InfrastructureProject}")
            };
            await _projectHelper.AddProjectReferences(_projectName, mainProjectReferences);
        }

        public async Task<string> UpdateMigrationScript()
        {
            var args = $"ef migrations add {DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}_CatapultUpdate -s \"{_projectHelper.GetProjectFullPath(Name)}\" -p \"{_projectHelper.GetProjectFullPath($"{_projectName}.{DataProjectGenerator.DataProject}\"")}";
            return await CommandHelper.RunDotnet(args, new Dictionary<string, string>
            {
                { "ConnectionStrings__DefaultConnection", _connectionString }
            }, _logger, workingDirectory: _outputLocation);
        }

        private void AddLogo()
        {
            var logoFile = Path.Combine(CodeGenerator.AssemblyDirectory, "Resources/Images/logo.png");
            _projectHelper.CopyFileToProject(Name, logoFile, "wwwroot/images/logo.png");
        }

        private void ModifyHomePage()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    ViewData[\"Title\"] = \"Home Page\";");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("<div class=\"jumbotron\">");
            sb.AppendLine("    <h1>Home Page</h1>");
            sb.AppendLine("    <h3>This project has been <span>CATAPULTED</span> by <a href=\"https://opencatapult.net/\" target=\"_blank\">OpenCatapult</a>.</h3>");
            sb.AppendLine("</div>");
            sb.AppendLine();
            sb.AppendLine("<p class=\"text-center\">");
            sb.AppendLine("    <img class=\"logo\" src=\"/images/logo.png\" />");
            sb.AppendLine("</p>");
            sb.AppendLine();
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("    <div class=\"col-lg-12 text-center\">");
            sb.AppendLine("        <h1>Insert home page content here</h1>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");

            _projectHelper.AddFileToProject(Name, $"Views/Home/Index.cshtml", sb.ToString(), true);
        }

        #region view models
        public Task<string> GenerateViewModels()
        {
            GenerateBaseViewModel();
            GenerateAdminBaseViewModel();

            foreach (var model in _models)
            {
                GenerateViewModel(model);
                GenerateAutoMapperProfile(model);

                GenerateAdminViewModel(model);
                GenerateAdminAutoMapperProfile(model);
            }

            CleanUpViewModels();

            return Task.FromResult($"{_models.Count} view model(s) generated");
        }

        private void GenerateBaseViewModel()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Models");
            sb.AppendLine("{");
            sb.AppendLine("    public abstract class BaseViewModel");
            sb.AppendLine("    {");
            sb.AppendLine("        public int Id { get; set; }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Models/BaseViewModel.cs", sb.ToString());
        }

        private void GenerateViewModel(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Models");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {model.Name}ViewModel : BaseViewModel");
            sb.AppendLine("    {");

            foreach (var property in model.Properties)
            {
                if (!string.IsNullOrEmpty(property.RelatedProjectDataModelName))
                {
                    if (property.RelationalType == PropertyRelationalType.OneToOne)
                    {
                        sb.AppendLine($"        public int {property.Name}Id {{ get; set; }}");
                    }
                    else if (property.RelationalType == PropertyRelationalType.OneToMany)
                    {
                        sb.AppendLine($"        public List<int> {property.Name}Ids {{ get; set; }}");
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

            _projectHelper.AddFileToProject(Name, $"Models/{model.Name}ViewModel.cs", sb.ToString(), modelId: model.Id);
        }

        private void GenerateAutoMapperProfile(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using AutoMapper;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine($"using {Name}.Models;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.AutoMapperProfiles");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {model.Name}AutoMapperProfile : Profile");
            sb.AppendLine("    {");
            sb.AppendLine($"        public {model.Name}AutoMapperProfile()");
            sb.AppendLine("        {");
            sb.AppendLine($"            CreateMap<{model.Name}, {model.Name}ViewModel>();");
            sb.AppendLine();
            sb.AppendLine($"            CreateMap<{model.Name}ViewModel, {model.Name}>();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"AutoMapperProfiles/{model.Name}AutoMapperProfile.cs", sb.ToString(), modelId: model.Id);
        }

        private void GenerateAdminBaseViewModel()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Areas.Admin.Models");
            sb.AppendLine("{");
            sb.AppendLine("    public abstract class BaseViewModel");
            sb.AppendLine("    {");
            sb.AppendLine("        public int Id { get; set; }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();

            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Models/BaseViewModel.cs", sb.ToString());
        }

        private void GenerateAdminViewModel(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Areas.Admin.Models");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {model.Name}ViewModel : BaseViewModel");
            sb.AppendLine("    {");

            foreach (var property in model.Properties)
            {
                if (!string.IsNullOrEmpty(property.RelatedProjectDataModelName))
                {
                    if (property.RelationalType == PropertyRelationalType.OneToOne)
                    {
                        sb.AppendLine($"        public int {property.Name}Id {{ get; set; }}");
                    }
                    else if (property.RelationalType == PropertyRelationalType.OneToMany)
                    {
                        sb.AppendLine($"        public List<int> {property.Name}Ids {{ get; set; }}");
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

            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Models/{model.Name}ViewModel.cs", sb.ToString(), modelId: model.Id);
        }

        private void GenerateAdminAutoMapperProfile(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using AutoMapper;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine($"using {Name}.Areas.Admin.Models;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Areas.Admin.AutoMapperProfiles");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {model.Name}AutoMapperProfile : Profile");
            sb.AppendLine("    {");
            sb.AppendLine($"        public {model.Name}AutoMapperProfile()");
            sb.AppendLine("        {");
            sb.AppendLine($"            CreateMap<{model.Name}, {model.Name}ViewModel>();");
            sb.AppendLine();
            sb.AppendLine($"            CreateMap<{model.Name}ViewModel, {model.Name}>();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"Areas/Admin/AutoMapperProfiles/{model.Name}AutoMapperProfile.cs", sb.ToString(), modelId: model.Id);
        }

        private void CleanUpViewModels()
        {
            _projectHelper.CleanUpFiles(Name, "Models", _models.Select(m => m.Id).ToArray());
            _projectHelper.CleanUpFiles(Name, "AutoMapperProfiles", _models.Select(m => m.Id).ToArray());
            _projectHelper.CleanUpFiles(Name, "Areas/Admin/Models", _models.Select(m => m.Id).ToArray());
            _projectHelper.CleanUpFiles(Name, "Areas/Admin/AutoMapperProfiles", _models.Select(m => m.Id).ToArray());
        }
        #endregion

        #region controllers
        public Task<string> GenerateControllers()
        {
            foreach (var model in _models)
            {
                GenerateController(model);
                GenerateAdminController(model);
            }

            GenerateHomeAdminController();

            CleanUpControllers();

            return Task.FromResult($"{_models.Count} controller(s) generated");
        }

        private void GenerateController(ProjectDataModelDto model)
        {
            var camelModelName = TextHelper.Camelize(model.Name);
            var sb = new StringBuilder();
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using AutoMapper;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Services;");
            sb.AppendLine($"using {Name}.Models;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Controllers");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {model.Name}Controller : Controller");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly I{model.Name}Service _{camelModelName}Service;");
            sb.AppendLine("        private readonly IMapper _mapper;");
            sb.AppendLine();
            sb.AppendLine($"        public {model.Name}Controller(I{model.Name}Service {camelModelName}Service, IMapper mapper)");
            sb.AppendLine("        {");
            sb.AppendLine($"            _{camelModelName}Service = {camelModelName}Service;");
            sb.AppendLine($"            _mapper = mapper;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<IActionResult> Index()");
            sb.AppendLine("        {");
            sb.AppendLine($"            var data = await _{camelModelName}Service.Get{TextHelper.Pluralize(model.Name)}();");
            sb.AppendLine($"            var models = _mapper.Map<List<{model.Name}ViewModel>>(data);");
            sb.AppendLine("            return View(models);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"Controllers/{model.Name}Controller.cs", sb.ToString(), modelId: model.Id);
        }

        private void GenerateAdminController(ProjectDataModelDto model)
        {
            var camelModelName = TextHelper.Camelize(model.Name);
            var sb = new StringBuilder();
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using AutoMapper;");
            sb.AppendLine("using Microsoft.AspNetCore.Authorization;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine($"using {Name}.Areas.Admin.Models;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Entities;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Services;");
            sb.AppendLine($"using {Name}.Identity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Areas.Admin.Controllers");
            sb.AppendLine("{");
            sb.AppendLine("    [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]");
            sb.AppendLine("    [Area(\"Admin\")]");
            sb.AppendLine($"    public class {model.Name}Controller : Controller");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly I{model.Name}Service _{camelModelName}Service;");
            sb.AppendLine("        private readonly IMapper _mapper;");
            sb.AppendLine();
            sb.AppendLine($"        public {model.Name}Controller(I{model.Name}Service {camelModelName}Service, IMapper mapper)");
            sb.AppendLine("        {");
            sb.AppendLine($"            _{camelModelName}Service = {camelModelName}Service;");
            sb.AppendLine($"            _mapper = mapper;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<IActionResult> Index()");
            sb.AppendLine("        {");
            sb.AppendLine($"            var data = await _{camelModelName}Service.Get{TextHelper.Pluralize(model.Name)}();");
            sb.AppendLine($"            var models = _mapper.Map<List<{model.Name}ViewModel>>(data);");
            sb.AppendLine("            return View(models);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public IActionResult Create()");
            sb.AppendLine("        {");
            sb.AppendLine($"            return View(new {model.Name}ViewModel());");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [ValidateAntiForgeryToken]");
            sb.AppendLine($"        public async Task<IActionResult> Create(int id, {model.Name}ViewModel model)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var entity = _mapper.Map<{model.Name}>(model);");
            sb.AppendLine($"                await _{camelModelName}Service.Create{model.Name}(entity);");
            sb.AppendLine("                return RedirectToAction(nameof(Index));");
            sb.AppendLine("            }");
            sb.AppendLine("            catch");
            sb.AppendLine("            {");
            sb.AppendLine("                return View(model);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<IActionResult> Edit(int id)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var data = await _{camelModelName}Service.Get{model.Name}ById(id);");
            sb.AppendLine($"            var model = _mapper.Map<{model.Name}ViewModel>(data);");
            sb.AppendLine("            return View(model);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [ValidateAntiForgeryToken]");
            sb.AppendLine($"        public async Task<IActionResult> Edit(int id, {model.Name}ViewModel model)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var entity = _mapper.Map<{model.Name}>(model);");
            sb.AppendLine($"                await _{camelModelName}Service.Update{model.Name}(entity);");
            sb.AppendLine("                return RedirectToAction(nameof(Index));");
            sb.AppendLine("            }");
            sb.AppendLine("            catch");
            sb.AppendLine("            {");
            sb.AppendLine("                return View(model);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public IActionResult Delete(int id)");
            sb.AppendLine("        {");
            sb.AppendLine("            ViewData[\"Id\"] = id;");
            sb.AppendLine("            return View();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [ValidateAntiForgeryToken]");
            sb.AppendLine($"        public async Task<IActionResult> Delete(int id, {model.Name}ViewModel model)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                await _{camelModelName}Service.Delete{model.Name}(id);");
            sb.AppendLine("                return RedirectToAction(nameof(Index));");
            sb.AppendLine("            }");
            sb.AppendLine("            catch");
            sb.AppendLine("            {");
            sb.AppendLine("                ViewData[\"Id\"] = id;");
            sb.AppendLine("                return View();");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Controllers/{model.Name}Controller.cs", sb.ToString(), modelId: model.Id);
        }

        private void GenerateHomeAdminController()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.AspNetCore.Authorization;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine($"using {Name}.Identity;");
            sb.AppendLine("");
            sb.AppendLine($"namespace {Name}.Areas.Admin.Controllers");
            sb.AppendLine("{");
            sb.AppendLine("    [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]");
            sb.AppendLine("    [Area(\"Admin\")]");
            sb.AppendLine("    public class HomeController : Controller");
            sb.AppendLine("    {");
            sb.AppendLine("        public IActionResult Index()");
            sb.AppendLine("        {");
            sb.AppendLine("            return View();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            _projectHelper.AddFileToProject(Name, "Areas/Admin/Controllers/HomeController.cs", sb.ToString());
        }

        private void CleanUpControllers()
        {
            _projectHelper.CleanUpFiles(Name, "Controllers", _models.Select(m => m.Id).ToArray());
            _projectHelper.CleanUpFiles(Name, "Areas/Admin/Controllers", _models.Select(m => m.Id).ToArray());
        }
        #endregion

        #region views
        public Task<string> GenerateViews()
        {
            foreach (var model in _models)
            {
                GenerateIndexView(model);

                GenerateAdminIndexView(model);
                GenerateAdminCreateView(model);
                GenerateAdminEditView(model);
                GenerateAdminDeleteView(model);
            }

            UpdateMenuAndFooter();
            GenerateLoginPartial();
            GenerateAdminLayout();
            GenerateAdminViewImports();
            GenerateAdminViewStart();
            GenerateAdminLoginPartial();
            GenerateAdminIndexView();

            CleanUpViews();

            return Task.FromResult($"{_models.Count} view(s) generated");
        }

        private void GenerateIndexView(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@model List<{Name}.Models.{model.Name}ViewModel>");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"View {model.Label}\";");
            sb.AppendLine("}");
            sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
            sb.AppendLine("<div>&nbsp;</div>");
            sb.AppendLine("<table class=\"table\">");
            sb.AppendLine("    <tr>");

            foreach (var property in model.Properties)
                if (string.IsNullOrEmpty(property.RelatedProjectDataModelName))
                    sb.AppendLine($"        <th>{property.Label}</th>");
            
            sb.AppendLine("    </tr>");
            sb.AppendLine("@foreach (var item in Model)");
            sb.AppendLine("{");
            sb.AppendLine("    <tr>");

            foreach (var property in model.Properties)
                if (string.IsNullOrEmpty(property.RelatedProjectDataModelName))
                    sb.AppendLine($"        <td>@item.{property.Name}</td>");

            sb.AppendLine("    </tr>");
            sb.AppendLine("}");
            sb.AppendLine("</table>");

            _projectHelper.AddFileToProject(Name, $"Views/{model.Name}/Index.cshtml", sb.ToString(), modelId: model.Id);
        }

        private void GenerateAdminIndexView(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@model List<{Name}.Areas.Admin.Models.{model.Name}ViewModel>");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"View {model.Label}\";");
            sb.AppendLine("}");
            sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
            sb.AppendLine("<div>");
            sb.AppendLine($"    <a asp-area=\"Admin\" asp-controller=\"{model.Name}\" asp-action=\"Create\" class=\"btn btn-default\">Create</a>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div>&nbsp;</div>");
            sb.AppendLine("<table class=\"table\">");
            sb.AppendLine("    <tr>");

            foreach (var property in model.Properties)
                if (string.IsNullOrEmpty(property.RelatedProjectDataModelName))
                    sb.AppendLine($"        <th>{property.Label}</th>");

            sb.AppendLine($"        <th></th>");

            sb.AppendLine("    </tr>");
            sb.AppendLine("@foreach (var item in Model)");
            sb.AppendLine("{");
            sb.AppendLine("    <tr>");

            foreach (var property in model.Properties)
                if (string.IsNullOrEmpty(property.RelatedProjectDataModelName))
                    sb.AppendLine($"        <td>@item.{property.Name}</td>");

            sb.AppendLine("        <td>");
            sb.AppendLine("            <a asp-action=\"Edit\" asp-route-id=\"@item.Id\">Edit</a> |");
            sb.AppendLine("            <a asp-action=\"Delete\" asp-route-id=\"@item.Id\">Delete</a>");
            sb.AppendLine("        </td>");

            sb.AppendLine("    </tr>");
            sb.AppendLine("}");
            sb.AppendLine("</table>");

            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Views/{model.Name}/Index.cshtml", sb.ToString(), modelId: model.Id);
        }

        private void GenerateAdminCreateView(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@model {Name}.Areas.Admin.Models.{model.Name}ViewModel");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"Create {model.Label}\";");
            sb.AppendLine("}");
            sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
            sb.AppendLine("<form method=\"post\">");
            sb.AppendLine(GenerateHtmlForm(model));
            sb.AppendLine("     <input type=\"submit\" value=\"Save\" class=\"btn btn-primary\" />");
            sb.AppendLine("</form>");
            sb.AppendLine("<div>");
            sb.AppendLine("    <a asp-action=\"Index\">Back to List</a>");
            sb.AppendLine("</div>");
            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Views/{model.Name}/Create.cshtml", sb.ToString(), modelId: model.Id);
        }

        private void GenerateAdminEditView(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@model {Name}.Areas.Admin.Models.{model.Name}ViewModel");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"Edit {model.Label}\";");
            sb.AppendLine("}");
            sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
            sb.AppendLine("<form method=\"post\">");
            sb.AppendLine(GenerateHtmlForm(model));
            sb.AppendLine("     <input type=\"submit\" value=\"Save\" class=\"btn btn-primary\" />");
            sb.AppendLine("</form>");
            sb.AppendLine("<div>");
            sb.AppendLine("    <a asp-action=\"Index\">Back to List</a>");
            sb.AppendLine("</div>");
            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Views/{model.Name}/Edit.cshtml", sb.ToString(), modelId: model.Id);
        }

        private void GenerateAdminDeleteView(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@model {Name}.Areas.Admin.Models.{model.Name}ViewModel");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"Edit {model.Label}\";");
            sb.AppendLine("}");
            sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
            sb.AppendLine("<form method=\"post\" asp-route-id=\"@ViewData[\"Id\"]\">");
            sb.AppendLine($"    <h3>Confirm delete {model.Label}?</h3>");
            sb.AppendLine("     <input type=\"submit\" value=\"Delete\" class=\"btn btn-danger\" />");
            sb.AppendLine("</form>");
            sb.AppendLine("<div>");
            sb.AppendLine("    <a asp-action=\"Index\">Back to List</a>");
            sb.AppendLine("</div>");
            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Views/{model.Name}/Delete.cshtml", sb.ToString(), modelId: model.Id);
        }

        private void UpdateMenuAndFooter()
        {
            string line = null;
            bool isNavBlock = false;
            bool isFooterBlock = false;
            var layoutFile = Path.Combine(_projectHelper.GetProjectFolder(Name), "Views/Shared/_Layout.cshtml");
            var updatedContent = new StringBuilder();
            using (var reader = new StreamReader(layoutFile))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var trimmedLine = line.Trim();

                    if (trimmedLine.StartsWith("<ul"))
                    {
                        isNavBlock = true;
                        updatedContent.AppendLine(line);
                    }
                    else if (trimmedLine == "</ul>")
                    {
                        isNavBlock = false;
                        updatedContent.AppendLine("                    <li class=\"nav-item\"><a class=\"nav-link\" asp-area=\"\" asp-controller=\"Home\" asp-action=\"Index\">Home</a></li>");
                        foreach (var model in _models)
                            updatedContent.AppendLine($"                    <li class=\"nav-item\"><a class=\"nav-link\" asp-area=\"\" asp-controller=\"{model.Name}\" asp-action=\"Index\">{model.Label}</a></li>");

                        updatedContent.AppendLine(line);

                        updatedContent.AppendLine("<partial name=\"_LoginPartial\" />");
                    }
                    else if (trimmedLine == "<partial name=\"_LoginPartial\" />")
                    {
                        // skip this line as it should have been readded in the nav section
                    }
                    else if (trimmedLine.StartsWith("<footer"))
                    {
                        isFooterBlock = true;
                        updatedContent.AppendLine(line);
                    }
                    else if (trimmedLine == "</footer>")
                    {
                        isFooterBlock = false;
                        updatedContent.AppendLine("<div class=\"container\">");
                        updatedContent.AppendLine($"<p class=\"float-left\">&copy; {DateTime.Today.Year} - {_projectName}</p>");
                        updatedContent.AppendLine("<p class=\"float-right\">Generated with ❤ by <a href=\"https://opencatapult.net/\" target=\"_blank\">OpenCatapult</a></p>");
                        updatedContent.AppendLine($"</div>");
                        updatedContent.AppendLine(line);
                    }
                    else if (trimmedLine.StartsWith("<nav"))
                    {
                        updatedContent.AppendLine("<nav class=\"navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-dark border-bottom box-shadow mb-3\">");
                    }
                    else if (trimmedLine.StartsWith("<div class=\"navbar-collapse collapse"))
                    {
                        updatedContent.AppendLine("<div class=\"navbar-collapse collapse\">");
                    }
                    else
                    {
                        if (!isNavBlock && !isFooterBlock)
                            updatedContent.AppendLine(line);
                    }
                }
            }

            using (var writer = new StreamWriter(layoutFile))
            {
                writer.Write(updatedContent.ToString());
            }
        }

        private void GenerateLoginPartial()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@if (User.Identity.Name != null)");
            sb.AppendLine("{");
            sb.AppendLine("    <form asp-area=\"Identity\" asp-page=\"/Account/Logout\" asp-route-returnUrl=\"@Url.Page(\"/Index\", new { area = \"\" })\" method=\"post\" id=\"logoutForm\" class=\"navbar-right\">");
            sb.AppendLine("        <ul class=\"navbar-nav ml-auto\">");
            sb.AppendLine("            <li class=\"nav-item\">");
            sb.AppendLine("                <a class=\"nav-link\" asp-area=\"Identity\" asp-page=\"/Account/Manage/Index\" title=\"Manage\">Hello @User.Identity.Name!</a>");
            sb.AppendLine("            </li>");
            sb.AppendLine("            <li class=\"nav-item\">");
            sb.AppendLine("                <a class=\"nav-link\" asp-area=\"Admin\" asp-controller=\"Home\" title=\"Admin\">Admin</a>");
            sb.AppendLine("            </li>");
            sb.AppendLine("            <li class=\"nav-item\">");
            sb.AppendLine("                <button type=\"submit\" class=\"btn btn-link navbar-btn navbar-link nav-link\">Logout</button>");
            sb.AppendLine("            </li>");
            sb.AppendLine("        </ul>");
            sb.AppendLine("    </form>");
            sb.AppendLine("}");
            sb.AppendLine("else");
            sb.AppendLine("{");
            sb.AppendLine("    <ul class=\"navbar-nav ml-auto\">");
            sb.AppendLine("        <li class=\"nav-item\"><a class=\"nav-link\" asp-area=\"Identity\" asp-page=\"/Account/Register\">Register</a></li>");
            sb.AppendLine("        <li class=\"nav-item\"><a class=\"nav-link\" asp-area=\"Identity\" asp-page=\"/Account/Login\">Login</a></li>");
            sb.AppendLine("    </ul>");
            sb.AppendLine("}");
            _projectHelper.AddFileToProject(Name, $"Views/Shared/_LoginPartial.cshtml", sb.ToString());
        }

        private void GenerateAdminLayout()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"utf-8\" />");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
            sb.AppendLine($"    <title>@ViewData[\"Title\"] - {Name}</title>");
            sb.AppendLine("");
            sb.AppendLine("    <environment include=\"Development\">");
            sb.AppendLine("        <link rel=\"stylesheet\" href=\"~/lib/bootstrap/dist/css/bootstrap.css\" />");
            sb.AppendLine("    </environment>");
            sb.AppendLine("    <environment exclude=\"Development\">");
            sb.AppendLine("        <link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css\"");
            sb.AppendLine("              asp-fallback-href=\"~/lib/bootstrap/dist/css/bootstrap.min.css\"");
            sb.AppendLine("              asp-fallback-test-class=\"sr-only\" asp-fallback-test-property=\"position\" asp-fallback-test-value=\"absolute\"");
            sb.AppendLine("              crossorigin=\"anonymous\"");
            sb.AppendLine("              integrity=\"sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T\"/>");
            sb.AppendLine("    </environment>");
            sb.AppendLine("    <link rel=\"stylesheet\" href=\"~/css/site.css\" />");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <header>");
            sb.AppendLine("<nav class=\"navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-dark border-bottom box-shadow mb-3\">");
            sb.AppendLine("            <div class=\"container\">");
            sb.AppendLine($"                <a class=\"navbar-brand\" asp-area=\"\" asp-controller=\"Home\" asp-action=\"Index\">{Name}</a>");
            sb.AppendLine("                <button class=\"navbar-toggler\" type=\"button\" data-toggle=\"collapse\" data-target=\".navbar-collapse\" aria-controls=\"navbarSupportedContent\"");
            sb.AppendLine("                        aria-expanded=\"false\" aria-label=\"Toggle navigation\">");
            sb.AppendLine("                    <span class=\"navbar-toggler-icon\"></span>");
            sb.AppendLine("                </button>");
            sb.AppendLine("<div class=\"navbar-collapse collapse\">");
            sb.AppendLine("                    <ul class=\"navbar-nav flex-grow-1\">");
            sb.AppendLine("                    <li class=\"nav-item\"><a class=\"nav-link\" asp-area=\"Admin\" asp-controller=\"Home\" asp-action=\"Index\">Dashboard</a></li>");

            foreach (var model in _models)
                sb.AppendLine($"                    <li class=\"nav-item\"><a class=\"nav-link\" asp-area=\"Admin\" asp-controller=\"{model.Name}\" asp-action=\"Index\">{model.Label}</a></li>");

            sb.AppendLine("                    </ul>");
            sb.AppendLine("<partial name=\"_LoginPartial\" />");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </nav>");
            sb.AppendLine("    </header>");
            sb.AppendLine("    <div class=\"container\">");
            sb.AppendLine("        <partial name=\"_CookieConsentPartial\" />");
            sb.AppendLine("        <main role=\"main\" class=\"pb-3\">");
            sb.AppendLine("            @RenderBody()");
            sb.AppendLine("        </main>");
            sb.AppendLine("    </div>");
            sb.AppendLine("");
            sb.AppendLine("    <footer class=\"border-top footer text-muted\">");
            sb.AppendLine("<div class=\"container\">");
            sb.AppendLine($"            <p class=\"pull-left\">&copy; {DateTime.Today.Year} - {Name}</p>");
            sb.AppendLine("<p class=\"float-right\">Generated with ❤ by <a href=\"https://opencatapult.net/\" target=\"_blank\">OpenCatapult</a></p>");
            sb.AppendLine("</div>");
            sb.AppendLine("    </footer>");
            sb.AppendLine("");
            sb.AppendLine("    <environment include=\"Development\">");
            sb.AppendLine("        <script src=\"~/lib/jquery/dist/jquery.js\"></script>");
            sb.AppendLine("        <script src=\"~/lib/bootstrap/dist/js/bootstrap.bundle.js\"></script>");
            sb.AppendLine("    </environment>");
            sb.AppendLine("    <environment exclude=\"Development\">");
            sb.AppendLine("        <script src=\"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js\"");
            sb.AppendLine("                asp-fallback-src=\"~/lib/jquery/dist/jquery.min.js\"");
            sb.AppendLine("                asp-fallback-test=\"window.jQuery\"");
            sb.AppendLine("                crossorigin=\"anonymous\"");
            sb.AppendLine("                integrity=\"sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=\">");
            sb.AppendLine("        </script>");
            sb.AppendLine("        <script src=\"https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js\"");
            sb.AppendLine("                asp-fallback-src=\"~/lib/bootstrap/dist/js/bootstrap.bundle.min.js\"");
            sb.AppendLine("                asp-fallback-test=\"window.jQuery && window.jQuery.fn && window.jQuery.fn.modal\"");
            sb.AppendLine("                crossorigin=\"anonymous\"");
            sb.AppendLine("                integrity=\"sha384-xrRywqdh3PHs8keKZN+8zzc5TX0GRTLCcmivcbNJWm2rs5C8PRhcEn3czEjhAO9o\">");
            sb.AppendLine("        </script>");
            sb.AppendLine("    </environment>");
            sb.AppendLine("    <script src=\"~/js/site.js\" asp-append-version=\"true\"></script>");
            sb.AppendLine("");
            sb.AppendLine("    @RenderSection(\"Scripts\", required: false)");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            sb.AppendLine("");

            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Views/Shared/_Layout.cshtml", sb.ToString(), true);
        }

        private void GenerateAdminViewImports()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@namespace {Name}.Areas.Admin");
            sb.AppendLine("@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers");
            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Views/_ViewImports.cshtml", sb.ToString());
        }

        private void GenerateAdminViewStart()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    Layout = \"_Layout\";");
            sb.AppendLine("}");
            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Views/_ViewStart.cshtml", sb.ToString());
        }

        private void GenerateAdminLoginPartial()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@if (User.Identity.Name != null)");
            sb.AppendLine("{");
            sb.AppendLine("    <form asp-area=\"Identity\" asp-page=\"/Account/Logout\" asp-route-returnUrl=\"@Url.Page(\"/Index\", new { area = \"\" })\" method=\"post\" id=\"logoutForm\" class=\"navbar-right\">");
            sb.AppendLine("        <ul class=\"navbar-nav ml-auto\">");
            sb.AppendLine("            <li class=\"nav-item\">");
            sb.AppendLine("                <a class=\"nav-link\" asp-area=\"Identity\" asp-page=\"/Account/Manage/Index\" title=\"Manage\">Hello @User.Identity.Name!</a>");
            sb.AppendLine("            </li>");
            sb.AppendLine("            <li class=\"nav-item\">");
            sb.AppendLine("                <button type=\"submit\" class=\"btn btn-link navbar-btn navbar-link nav-link\">Logout</button>");
            sb.AppendLine("            </li>");
            sb.AppendLine("        </ul>");
            sb.AppendLine("    </form>");
            sb.AppendLine("}");
            sb.AppendLine("else");
            sb.AppendLine("{");
            sb.AppendLine("    <ul class=\"navbar-nav ml-auto\">");
            sb.AppendLine("        <li class=\"nav-item\"><a class=\"nav-link\" asp-area=\"Identity\" asp-page=\"/Account/Register\">Register</a></li>");
            sb.AppendLine("        <li class=\"nav-item\"><a class=\"nav-link\" asp-area=\"Identity\" asp-page=\"/Account/Login\">Login</a></li>");
            sb.AppendLine("    </ul>");
            sb.AppendLine("}");
            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Views/Shared/_LoginPartial.cshtml", sb.ToString());
        }

        private string GenerateHtmlForm(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            foreach (var property in model.Properties)
            {
                var propertyName = property.Name;
                if (property.RelationalType == PropertyRelationalType.OneToOne)
                    propertyName += "Id";
                else if (property.RelationalType == PropertyRelationalType.OneToMany)
                    propertyName += "Ids";

                sb.AppendLine("    <div class=\"form-group row\">");
                sb.AppendLine($"        <label asp-for=\"{propertyName}\" class=\"col-form-label col-md-2\"></label>");
                sb.AppendLine("        <div class=\"col-md-10\">");
                switch (property.ControlType)
                {
                    case PropertyControlType.InputText:
                        sb.AppendLine($"            <input asp-for=\"{propertyName}\" type=\"text\" class=\"form-control\" />");
                        break;
                    case PropertyControlType.InputNumber:
                        sb.AppendLine($"            <input asp-for=\"{propertyName}\" type=\"number\" class=\"form-control\" />");
                        break;
                    case PropertyControlType.Calendar:
                        sb.AppendLine($"            <input asp-for=\"{propertyName}\" type=\"date\" class=\"form-control\" />");
                        break;
                    case PropertyControlType.InputFile:
                    case PropertyControlType.Image:
                        sb.AppendLine($"            <input asp-for=\"{propertyName}\" type=\"file\" class=\"form-control\" />");
                        break;
                    case PropertyControlType.Textarea:
                        sb.AppendLine($"            <textarea asp-for=\"{propertyName}\" class=\"form-control\"></textarea>");
                        break;
                    case PropertyControlType.Checkbox:
                        sb.AppendLine($"            <input type=\"checkbox\" asp-for=\"{propertyName}\" class = \"form-control\" />");
                        break;
                }
                sb.AppendLine($"            <span asp-validation-for=\"{propertyName}\" class=\"text-danger\"></span>");
                sb.AppendLine("        </div>");
                sb.AppendLine("    </div>");
            }

            return sb.ToString();
        }

        private void GenerateAdminIndexView()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    ViewData[\"Title\"] = \"Admin Page\";");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("<div class=\"jumbotron\">");
            sb.AppendLine("    <h1>Admin Page</h1>");
            sb.AppendLine("    <h3>This is the Dashboard page of your Admin</h3>");
            sb.AppendLine("</div>");
            sb.AppendLine("");
            sb.AppendLine("<p class=\"text-center\">");
            sb.AppendLine("    <img class=\"logo\" src=\"/images/logo.png\" />");
            sb.AppendLine("</p>");
            sb.AppendLine("");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("    <div class=\"col-lg-12 text-center\">");
            sb.AppendLine("        <h1>Insert dashboard content here</h1>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");
            _projectHelper.AddFileToProject(Name, $"Areas/Admin/Views/Home/Index.cshtml", sb.ToString());
        }

        private void CleanUpViews()
        {
            _projectHelper.CleanUpFolders(Name, "Views", _models.Select(m => m.Id).ToArray());
            _projectHelper.CleanUpFolders(Name, "Areas/Admin/Views", _models.Select(m => m.Id).ToArray());
        }
        #endregion

        #region startup
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
            
            sb.AppendLine($"            services.AddTransient<IUserService, UserService>();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"ServiceInjection.cs", sb.ToString(), true);

            return Task.FromResult("ServiceInjection generated");
        }

        public Task<string> GenerateStartupClass()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using AutoMapper;");
            sb.AppendLine("using Microsoft.AspNetCore.Builder;");
            sb.AppendLine("using Microsoft.AspNetCore.Hosting;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using Microsoft.Extensions.Configuration;");
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine($"using {Name}.Areas.Identity.Services;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Constants;");
            sb.AppendLine($"using {Name}.Identity;");
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
            sb.AppendLine("            services.AddAppIdentity();");
            sb.AppendLine();
            sb.AppendLine("            services.AddAuthorization(options =>");
            sb.AppendLine("            {");
            sb.AppendLine("                options.AddPolicy(AuthorizePolicy.UserRoleAdminAccess, policy => policy.RequireRole(UserRole.Administrator));");
            sb.AppendLine("                options.AddPolicy(AuthorizePolicy.UserRoleGuestAccess, policy => policy.RequireRole(UserRole.Administrator, UserRole.Guest));");
            sb.AppendLine("            });");
            sb.AppendLine();
            sb.AppendLine("            services.AddEmail(Configuration);");
            sb.AppendLine();
            sb.AppendLine("            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);");
            sb.AppendLine("            services.AddAutoMapper();");
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
            sb.AppendLine("            app.UseAuthentication();");
            sb.AppendLine();
            sb.AppendLine("            app.UseMvc(routes =>");
            sb.AppendLine("            {");
            sb.AppendLine("                routes.MapRoute(");
            sb.AppendLine("                    name: \"areas\",");
            sb.AppendLine("                    template: \"{area:exists}/{controller=Home}/{action=Index}/{id?}\");");
            sb.AppendLine();
            sb.AppendLine("                routes.MapRoute(");
            sb.AppendLine("                    name: \"default\",");
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
        #endregion

        #region Identity
        public Task<string> AddApplicationIdentity()
        {
            AddAuthorizePolicy();
            AddLoginPage();
            GenerateIdentityViewImports();
            GenerateIdentityViewStart();
            AddSmtpSettingToAppSetting();
            GenerateEmailSender();
            GenerateSmtpSetting();
            GenerateEmailInjection();

            return Task.FromResult("Application identity generated");
        }

        private void AddSmtpSettingToAppSetting()
        {
            string line = null;
            var appSettingFile = Path.Combine(_projectHelper.GetProjectFolder(Name), "appsettings.json");
            var appsetting = File.ReadAllText(appSettingFile);
            
            if (!appsetting.Contains("\"SmtpSetting\""))
            {
                var updatedContent = new StringBuilder();
                using (var reader = new StreamReader(appSettingFile))
                {
                    int lineNo = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNo++;
                        if (lineNo == 1)
                        {
                            updatedContent.AppendLine(line);
                            updatedContent.AppendLine("  \"SmtpSetting\": {");
                            updatedContent.AppendLine($"    \"Server\": \"localhost\",");
                            updatedContent.AppendLine($"    \"Port\": 0,");
                            updatedContent.AppendLine($"    \"Username\": \"username\",");
                            updatedContent.AppendLine($"    \"Password\": \"password\",");
                            updatedContent.AppendLine($"    \"SenderEmail\": \"localhost\"");
                            updatedContent.AppendLine("  },");
                        }
                        else
                        {
                            updatedContent.AppendLine(line);
                        }
                    }
                }
                using (var writer = new StreamWriter(appSettingFile))
                {
                    writer.Write(updatedContent.ToString());
                }
            }
        }

        private void AddAuthorizePolicy()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"namespace {Name}.Identity");
            sb.AppendLine("{");
            sb.AppendLine("    public static class AuthorizePolicy");
            sb.AppendLine("    {");
            sb.AppendLine("        public const string UserRoleAdminAccess = \"UserRoleAdminAccess\";");
            sb.AppendLine("        public const string UserRoleGuestAccess = \"UserRoleGuestAccess\";");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            _projectHelper.AddFileToProject(Name, $"Identity/AuthorizePolicy.cs", sb.ToString());
        }

        private void AddLoginPage()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@page");
            sb.AppendLine("@model LoginModel");
            sb.AppendLine("");
            sb.AppendLine("@{");
            sb.AppendLine("    ViewData[\"Title\"] = \"Log in\";");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("    <div class=\"col-md-4\">");
            sb.AppendLine("        <section>");
            sb.AppendLine("            <form method=\"post\">");
            sb.AppendLine("                <h4>Use a local account to log in.</h4>");
            sb.AppendLine("                <hr />");
            sb.AppendLine("                <div asp-validation-summary=\"All\" class=\"text-danger\"></div>");
            sb.AppendLine("                <div class=\"form-group\">");
            sb.AppendLine("                    <label asp-for=\"Input.Email\"></label>");
            sb.AppendLine("                    <input asp-for=\"Input.Email\" class=\"form-control\" />");
            sb.AppendLine("                    <span asp-validation-for=\"Input.Email\" class=\"text-danger\"></span>");
            sb.AppendLine("                </div>");
            sb.AppendLine("                <div class=\"form-group\">");
            sb.AppendLine("                    <label asp-for=\"Input.Password\"></label>");
            sb.AppendLine("                    <input asp-for=\"Input.Password\" class=\"form-control\" />");
            sb.AppendLine("                    <span asp-validation-for=\"Input.Password\" class=\"text-danger\"></span>");
            sb.AppendLine("                </div>");
            sb.AppendLine("                <div class=\"form-group\">");
            sb.AppendLine("                    <div class=\"checkbox\">");
            sb.AppendLine("                        <label asp-for=\"Input.RememberMe\">");
            sb.AppendLine("                            <input asp-for=\"Input.RememberMe\" />");
            sb.AppendLine("                            @Html.DisplayNameFor(m => m.Input.RememberMe)");
            sb.AppendLine("                        </label>");
            sb.AppendLine("                    </div>");
            sb.AppendLine("                </div>");
            sb.AppendLine("                <div class=\"form-group\">");
            sb.AppendLine("                    <button type=\"submit\" class=\"btn btn-default\">Log in</button>");
            sb.AppendLine("                </div>");
            sb.AppendLine("                <div class=\"form-group\">");
            sb.AppendLine("                    <p>");
            sb.AppendLine("                        <a asp-page=\"./ForgotPassword\">Forgot your password?</a>");
            sb.AppendLine("                    </p>");
            sb.AppendLine("                    <p>");
            sb.AppendLine("                        <a asp-page=\"./Register\" asp-route-returnUrl=\"@Model.ReturnUrl\">Register as a new user</a>");
            sb.AppendLine("                    </p>");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </form>");
            sb.AppendLine("        </section>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");
            sb.AppendLine("");
            sb.AppendLine("@section Scripts {");
            sb.AppendLine("    <partial name=\"_ValidationScriptsPartial\" />");
            sb.AppendLine("}");
            _projectHelper.AddFileToProject(Name, $"Areas/Identity/Pages/Account/Login.cshtml", sb.ToString());
            
            sb = new StringBuilder();
            sb.AppendLine("using Microsoft.AspNetCore.Authentication;");
            sb.AppendLine("using Microsoft.AspNetCore.Authorization;");
            sb.AppendLine("using Microsoft.AspNetCore.Identity;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc.RazorPages;");
            sb.AppendLine("using Microsoft.Extensions.Logging;");
            sb.AppendLine($"using {_projectName}.{CoreProjectGenerator.CoreProject}.Services;");
            sb.AppendLine($"using {_projectName}.{DataProjectGenerator.DataProject}.Identity;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Areas.Identity.Pages.Account");
            sb.AppendLine("{");
            sb.AppendLine("    [AllowAnonymous]");
            sb.AppendLine("    public class LoginModel : PageModel");
            sb.AppendLine("    {");
            sb.AppendLine("        private readonly IUserService _userService;");
            sb.AppendLine("        private readonly ILogger<LoginModel> _logger;");
            sb.AppendLine();
            sb.AppendLine("        public LoginModel(IUserService userService, ILogger<LoginModel> logger)");
            sb.AppendLine("        {");
            sb.AppendLine("            _userService = userService;");
            sb.AppendLine("            _logger = logger;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [BindProperty]");
            sb.AppendLine("        public InputModel Input { get; set; }");
            sb.AppendLine();
            sb.AppendLine("        public string ReturnUrl { get; set; }");
            sb.AppendLine();
            sb.AppendLine("        [TempData]");
            sb.AppendLine("        public string ErrorMessage { get; set; }");
            sb.AppendLine();
            sb.AppendLine("        public class InputModel");
            sb.AppendLine("        {");
            sb.AppendLine("            [Required]");
            sb.AppendLine("            [EmailAddress]");
            sb.AppendLine("            public string Email { get; set; }");
            sb.AppendLine();
            sb.AppendLine("            [Required]");
            sb.AppendLine("            [DataType(DataType.Password)]");
            sb.AppendLine("            public string Password { get; set; }");
            sb.AppendLine();
            sb.AppendLine("            [Display(Name = \"Remember me?\")]");
            sb.AppendLine("            public bool RememberMe { get; set; }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task OnGetAsync(string returnUrl = null)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (!string.IsNullOrEmpty(ErrorMessage))");
            sb.AppendLine("            {");
            sb.AppendLine("                ModelState.AddModelError(string.Empty, ErrorMessage);");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            returnUrl = returnUrl ?? Url.Content(\"~/\");");
            sb.AppendLine();
            sb.AppendLine("            // Clear the existing external cookie to ensure a clean login process");
            sb.AppendLine("            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);");
            sb.AppendLine();
            sb.AppendLine("            ReturnUrl = returnUrl;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<IActionResult> OnPostAsync(string returnUrl = null)");
            sb.AppendLine("        {");
            sb.AppendLine("            returnUrl = returnUrl ?? Url.Content(\"~/\");");
            sb.AppendLine();
            sb.AppendLine("            if (ModelState.IsValid)");
            sb.AppendLine("            {");
            sb.AppendLine("                // This doesn't count login failures towards account lockout");
            sb.AppendLine("                // To enable password failures to trigger account lockout, set lockoutOnFailure: true");
            sb.AppendLine("                var result = await _userService.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);");
            sb.AppendLine("                if (result.Succeeded)");
            sb.AppendLine("                {");
            sb.AppendLine("                    _logger.LogInformation(\"User logged in.\");");
            sb.AppendLine("                    return LocalRedirect(returnUrl);");
            sb.AppendLine("                }");
            sb.AppendLine("                if (result.RequiresTwoFactor)");
            sb.AppendLine("                {");
            sb.AppendLine("                    return RedirectToPage(\"./LoginWith2fa\", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });");
            sb.AppendLine("                }");
            sb.AppendLine("                if (result.IsLockedOut)");
            sb.AppendLine("                {");
            sb.AppendLine("                    _logger.LogWarning(\"User account locked out.\");");
            sb.AppendLine("                    return RedirectToPage(\"./Lockout\");");
            sb.AppendLine("                }");
            sb.AppendLine("                else");
            sb.AppendLine("                {");
            sb.AppendLine("                    ModelState.AddModelError(string.Empty, \"Invalid login attempt.\");");
            sb.AppendLine("                    return Page();");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            // If we got this far, something failed, redisplay form");
            sb.AppendLine("            return Page();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();
            _projectHelper.AddFileToProject(Name, $"Areas/Identity/Pages/Account/Login.cshtml.cs", sb.ToString());
        }

        private void GenerateIdentityViewImports()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@namespace {Name}.Areas.Identity.Pages");
            sb.AppendLine("@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers");
            _projectHelper.AddFileToProject(Name, $"Areas/Identity/Pages/_ViewImports.cshtml", sb.ToString());
        }

        private void GenerateIdentityViewStart()
        {
            var sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine($"    if (User.IsInRole({_projectName}.{CoreProjectGenerator.CoreProject}.Constants.UserRole.Administrator))");
            sb.AppendLine("    {");
            sb.AppendLine("        Layout = \"/Areas/Admin/Views/Shared/_Layout.cshtml\";");
            sb.AppendLine("    }");
            sb.AppendLine("    else");
            sb.AppendLine("    {");
            sb.AppendLine("        Layout = \"/Views/Shared/_Layout.cshtml\";");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            _projectHelper.AddFileToProject(Name, $"Areas/Identity/Pages/_ViewStart.cshtml", sb.ToString());
        }

        private void GenerateEmailSender()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using MailKit.Net.Smtp;");
            sb.AppendLine("using MailKit.Security;");
            sb.AppendLine("using Microsoft.AspNetCore.Identity.UI.Services;");
            sb.AppendLine("using Microsoft.Extensions.Options;");
            sb.AppendLine("using MimeKit;");
            sb.AppendLine("using MimeKit.Text;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Areas.Identity.Services");
            sb.AppendLine("{");
            sb.AppendLine("    public class EmailSender : IEmailSender");
            sb.AppendLine("    {");
            sb.AppendLine("        public EmailSender(IOptions<SmtpSetting> optionsAccessor)");
            sb.AppendLine("        {");
            sb.AppendLine("            _smtpSetting = optionsAccessor.Value;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private readonly SmtpSetting _smtpSetting;");
            sb.AppendLine();
            sb.AppendLine("        public async Task SendEmailAsync(string email, string subject, string message)");
            sb.AppendLine("        {");
            sb.AppendLine("            var mail = new MimeMessage();");
            sb.AppendLine("            mail.From.Add(new MailboxAddress(_smtpSetting.SenderEmail));");
            sb.AppendLine("            mail.To.Add(new MailboxAddress(email));");
            sb.AppendLine();
            sb.AppendLine("            mail.Subject = subject;");
            sb.AppendLine();
            sb.AppendLine("            mail.Body = new TextPart(TextFormat.Html)");
            sb.AppendLine("            {");
            sb.AppendLine("                Text = message");
            sb.AppendLine("            };");
            sb.AppendLine();
            sb.AppendLine("            using (var client = new SmtpClient())");
            sb.AppendLine("            {");
            sb.AppendLine("                await client.ConnectAsync(_smtpSetting.Server, _smtpSetting.Port, SecureSocketOptions.Auto);");
            sb.AppendLine("                await client.AuthenticateAsync(_smtpSetting.Username, _smtpSetting.Password);");
            sb.AppendLine();
            sb.AppendLine("                await client.SendAsync(mail);");
            sb.AppendLine("                await client.DisconnectAsync(true);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();
            _projectHelper.AddFileToProject(Name, $"Areas/Identity/Services/EmailSender.cs", sb.ToString());
        }

        private void GenerateSmtpSetting()
        {
            var sb = new StringBuilder(); sb.AppendLine($"namespace {Name}.Areas.Identity.Services");
            sb.AppendLine("{");
            sb.AppendLine("    public class SmtpSetting");
            sb.AppendLine("    {");
            sb.AppendLine("        public string Server { get; set; }");
            sb.AppendLine();
            sb.AppendLine("        public int Port { get; set; }");
            sb.AppendLine();
            sb.AppendLine("        public string Username { get; set; }");
            sb.AppendLine();
            sb.AppendLine("        public string Password { get; set; }");
            sb.AppendLine();
            sb.AppendLine("        public string SenderEmail { get; set; }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();
            _projectHelper.AddFileToProject(Name, $"Areas/Identity/Services/SmtpSetting.cs", sb.ToString());
        }

        private void GenerateEmailInjection()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.AspNetCore.Identity.UI.Services;");
            sb.AppendLine("using Microsoft.Extensions.Configuration;");
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine();
            sb.AppendLine($"namespace {Name}.Areas.Identity.Services");
            sb.AppendLine("{");
            sb.AppendLine("    public static class EmailInjection");
            sb.AppendLine("    {");
            sb.AppendLine("        public static void AddEmail(this IServiceCollection services, IConfiguration configuration, string sectionName = \"SmtpSetting\")");
            sb.AppendLine("        {");
            sb.AppendLine("            services.AddTransient<IEmailSender, EmailSender>();");
            sb.AppendLine();
            sb.AppendLine("            var section = configuration.GetSection(sectionName);");
            sb.AppendLine("            services.Configure<SmtpSetting>(section);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            _projectHelper.AddFileToProject(Name, $"Areas/Identity/Services/EmailInjection.cs", sb.ToString());
        }
        #endregion //Identity
    }
}
