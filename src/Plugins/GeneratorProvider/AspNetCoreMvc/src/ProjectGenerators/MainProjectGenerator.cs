// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMvc.Helpers;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace AspNetCoreMvc.ProjectGenerators
{
    internal class MainProjectGenerator
    {
        private string _projectName;
        private readonly ProjectHelper _projectHelper;
        private readonly List<ProjectDataModelDto> _models;
        private readonly string _connectionString;

        private string Name => $"{_projectName}";

        public MainProjectGenerator(string projectName, ProjectHelper projectHelper, List<ProjectDataModelDto> models, string connectionString)
        {
            _projectName = projectName;
            _projectHelper = projectHelper;
            _models = models;
            _connectionString = connectionString;
        }

        public async Task<string> Initialize()
        {
            var mainProjectReferences = new string[]
            {
                _projectHelper.GetProjectFullPath($"{_projectName}.{CoreProjectGenerator.CoreProject}"),
                _projectHelper.GetProjectFullPath($"{_projectName}.{InfrastructureProjectGenerator.InfrastructureProject}")
            };
            var mainProjectPackages = new (string, string)[]
            {
                ("AutoMapper", "7.0.1"),
                ("AutoMapper.Extensions.Microsoft.DependencyInjection", "5.0.1")
            };

            var message = await _projectHelper.CreateProject($"{_projectName}", "mvc", mainProjectReferences, mainProjectPackages);
            AddConnectionString();
            AddLogo();
            ModifyHomePage();

            return message;
        }

        public async Task<string> UpdateMigrationScript()
        {
            var args = $"ef migrations add {DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}_CatapultUpdate -s {_projectHelper.GetProjectFullPath(Name)} -p {_projectHelper.GetProjectFullPath($"{_projectName}.{DataProjectGenerator.DataProject}")}";
            return await CommandHelper.RunDotnet(args);
        }

        private void AddConnectionString()
        {
            string line = null;
            var appSettingFile = Path.Combine(_projectHelper.GetProjectFolder(Name), "appsettings.json");
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
                        updatedContent.AppendLine("  \"ConnectionStrings\": {");
                        updatedContent.AppendLine($"    \"DefaultConnection\": \"{_connectionString}\"");
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

        private void AddLogo()
        {
            var logoFile = Path.Combine(AppContext.BaseDirectory, "Resources/Images/logo.png");
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

            foreach (var model in _models)
            {
                GenerateViewModel(model);
                GenerateAutoMapperProfile(model);
            }                

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

            _projectHelper.AddFileToProject(Name, $"Models/{model.Name}ViewModel.cs", sb.ToString(), true);
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

            _projectHelper.AddFileToProject(Name, $"AutoMapperProfiles/{model.Name}AutoMapperProfile.cs", sb.ToString(), true);
        }
        #endregion

        #region controllers
        public Task<string> GenerateControllers()
        {
            foreach (var model in _models)
                GenerateController(model);

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

            _projectHelper.AddFileToProject(Name, $"Controllers/{model.Name}Controller.cs", sb.ToString());
        }
        #endregion

        #region views
        public Task<string> GenerateViews()
        {
            foreach (var model in _models)
            {
                GenerateIndexView(model);
                GenerateCreateView(model);
                GenerateEditView(model);
                GenerateDeleteView(model);
            }

            UpdateMenuAndFooter();

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
            sb.AppendLine("<div>");
            sb.AppendLine($"    <a asp-area=\"\" asp-controller=\"{model.Name}\" asp-action=\"Create\" class=\"btn btn-default\">Create</a>");
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
            sb.AppendLine("            @Html.ActionLink(\"Edit\", \"Edit\", new { id = item.Id }) |");
            sb.AppendLine("            @Html.ActionLink(\"Delete\", \"Delete\", new { id = item.Id }) |");
            sb.AppendLine("        </td>");

            sb.AppendLine("    </tr>");
            sb.AppendLine("}");
            sb.AppendLine("</table>");

            _projectHelper.AddFileToProject(Name, $"Views/{model.Name}/Index.cshtml", sb.ToString());
        }

        private void GenerateCreateView(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@model {Name}.Models.{model.Name}ViewModel");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"Create {model.Label}\";");
            sb.AppendLine("}");
            sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
            sb.AppendLine("@using (Html.BeginForm())");
            sb.AppendLine("{");
            sb.AppendLine("    @Html.AntiForgeryToken()");
            sb.AppendLine(GenerateHtmlForm(model));
            sb.AppendLine("     <input type=\"submit\" value=\"Save\" class=\"btn btn-primary\" />");
            sb.AppendLine("}");
            sb.Append("<div>");
            sb.Append("    @Html.ActionLink(\"Back to List\", \"Index\")");
            sb.Append("</div>");
            _projectHelper.AddFileToProject(Name, $"Views/{model.Name}/Create.cshtml", sb.ToString());
        }

        private void GenerateEditView(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@model {Name}.Models.{model.Name}ViewModel");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"Edit {model.Label}\";");
            sb.AppendLine("}");
            sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
            sb.AppendLine("@using (Html.BeginForm())");
            sb.AppendLine("{");
            sb.AppendLine("    @Html.AntiForgeryToken()");
            sb.AppendLine(GenerateHtmlForm(model));
            sb.AppendLine("     <input type=\"submit\" value=\"Save\" class=\"btn btn-primary\" />");
            sb.AppendLine("}");
            sb.Append("<div>");
            sb.Append("    @Html.ActionLink(\"Back to List\", \"Index\")");
            sb.Append("</div>");
            _projectHelper.AddFileToProject(Name, $"Views/{model.Name}/Edit.cshtml", sb.ToString());
        }

        private void GenerateDeleteView(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"@model {Name}.Models.{model.Name}ViewModel");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"Edit {model.Label}\";");
            sb.AppendLine("}");
            sb.AppendLine("<h2>@ViewData[\"Title\"]</h2>");
            sb.AppendLine("@using (Html.BeginForm(routeValues: new { id = ViewData[\"Id\"] }))");
            sb.AppendLine("{");
            sb.AppendLine("    @Html.AntiForgeryToken()");
            sb.AppendLine($"    <h3>Confirm delete {model.Label}?</h3>");
            sb.AppendLine("     <input type=\"submit\" value=\"Delete\" class=\"btn btn-danger\" />");
            sb.AppendLine("");
            sb.AppendLine("}");
            sb.Append("<div>");
            sb.Append("    @Html.ActionLink(\"Back to List\", \"Index\")");
            sb.Append("</div>");
            _projectHelper.AddFileToProject(Name, $"Views/{model.Name}/Delete.cshtml", sb.ToString());
        }

        private void UpdateMenuAndFooter()
        {
            string line = null;
            bool isNavBlock = false;
            bool isFooterBlock = false;
            var layoutFile = Path.Combine(_projectHelper.GetProjectFolder(Name), "Views\\Shared\\_Layout.cshtml");
            var updatedContent = new StringBuilder();
            using (var reader = new StreamReader(layoutFile))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    switch (line.Trim())
                    {
                        case "<ul class=\"nav navbar-nav\">":
                            isNavBlock = true;
                            updatedContent.AppendLine(line);
                            break;
                        case "</ul>":
                            isNavBlock = false;
                            updatedContent.AppendLine("                    <li><a asp-area=\"\" asp-controller=\"Home\" asp-action=\"Index\">Home</a></li>");
                            foreach (var model in _models)
                                updatedContent.AppendLine($"                    <li><a asp-area=\"\" asp-controller=\"{model.Name}\" asp-action=\"Index\">{model.Label}</a></li>");

                            updatedContent.AppendLine(line);
                            break;
                        case "<footer>":
                            isFooterBlock = true;
                            updatedContent.AppendLine(line);
                            break;
                        case "</footer>":
                            isFooterBlock = false;
                            updatedContent.AppendLine($"<p class=\"pull-left\">&copy; {DateTime.Today.Year} - {_projectName}</p>");
                            updatedContent.AppendLine($"<p class=\"pull-right\">Generated with ❤ by <a href=\"https://opencatapult.net/\" target=\"_blank\">OpenCatapult</a></p>");
                            updatedContent.AppendLine(line);
                            break;
                        default:
                            if (!isNavBlock && !isFooterBlock)
                                updatedContent.AppendLine(line);
                            break;
                    }
                }
            }

            using (var writer = new StreamWriter(layoutFile))
            {
                writer.Write(updatedContent.ToString());
            }
        }

        private string GenerateHtmlForm(ProjectDataModelDto model)
        {
            var sb = new StringBuilder();
            foreach (var property in model.Properties)
            {
                var propertyName = string.IsNullOrEmpty(property.RelatedProjectDataModelName) ? property.Name : $"{property.Name}Id";
                sb.AppendLine("    <div class=\"form-group row\">");
                sb.AppendLine($"        @Html.LabelFor(model => model.{propertyName}, htmlAttributes: new {{ @class = \"col-form-label col-md-2\" }})");
                sb.AppendLine("        <div class=\"col-md-10\">");
                switch (property.ControlType)
                {
                    case PropertyControlType.InputText:
                    case PropertyControlType.InputNumber:
                    case PropertyControlType.Calendar:
                        sb.AppendLine($"            @Html.EditorFor(model => model.{propertyName}, new {{ htmlAttributes = new {{ @class = \"form-control\" }} }})");
                        break;
                    case PropertyControlType.InputFile:
                    case PropertyControlType.Image:
                        sb.AppendLine($"            @Html.EditorFor(model => model.{propertyName}, new {{ type = \"file\" htmlAttributes = new {{ @class = \"form-control\" }} }})");
                        break;
                    case PropertyControlType.Textarea:
                        sb.AppendLine($"            @Html.EditorFor(model => model.{propertyName}, new {{ htmlAttributes = new {{ @class = \"form-control\" }} }})");
                        break;
                    case PropertyControlType.Checkbox:
                        sb.AppendLine($"            @Html.CheckBoxFor(model => model.{propertyName}, new {{ htmlAttributes = new {{ @class = \"form-control\" }} }})");
                        break;
                }
                sb.AppendLine($"            @Html.ValidationMessageFor(model => model.{propertyName}, \"\", new {{ @class = \"text-danger\" }})");
                sb.AppendLine("        </div>");
                sb.AppendLine("    </div>");
            }

            return sb.ToString();
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
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            _projectHelper.AddFileToProject(Name, $"ServiceInjection.cs", sb.ToString());

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
            sb.AppendLine();
            sb.AppendLine("            app.UseMvc(routes =>");
            sb.AppendLine("            {");
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
    }
}
