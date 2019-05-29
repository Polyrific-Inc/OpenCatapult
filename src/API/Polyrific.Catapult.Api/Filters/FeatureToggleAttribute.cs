// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Polyrific.Catapult.Api.Filters
{
    public class FeatureToggle : ActionFilterAttribute
    {
        public string FeatureToggleKey { get; set; }

        public FeatureToggle(string featureToggleKey)
        {
            FeatureToggleKey = featureToggleKey;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var enabled = configuration.GetValue<bool>(FeatureToggleKey);

            if (enabled)
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new BadRequestObjectResult($"The application setting {FeatureToggleKey} is not enabled");
            }
        }
    }
}
