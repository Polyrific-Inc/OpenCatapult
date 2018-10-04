// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli
{
    public class CatapultHelpTextGenerator : DefaultHelpTextGenerator
    {
        private readonly IExternalServiceTypeService _externalServiceTypeService;

        public CatapultHelpTextGenerator(IExternalServiceTypeService externalServiceTypeService)
        {
            _externalServiceTypeService = externalServiceTypeService;
        }

        protected override void GenerateFooter(CommandLineApplication application, TextWriter output)
        {
            base.GenerateFooter(application, output);

            if (application.Name == "add" && application.Parent.Name == "service")
            {
                output.WriteLine("Types of the external service:");
                try
                {
                    var serviceTypes = _externalServiceTypeService.GetExternalServiceTypes(true).Result;
                    foreach (var serviceType in serviceTypes)
                    {
                        output.WriteLine($"  - {serviceType.Name}");
                        if (serviceType.ExternalServiceProperties != null && serviceType.ExternalServiceProperties.Count > 0)
                        {
                            output.WriteLine("    Properties:");
                            foreach (var property in serviceType.ExternalServiceProperties)
                                output.WriteLine($"      - {property.Name} {(property.IsRequired ? "(required)" : "")}");
                        }
                    }
                }
                catch
                {
                    output.WriteLine("Failed retrieving external service types. Please try to login into application");
                }
            }
        }
    }
}
