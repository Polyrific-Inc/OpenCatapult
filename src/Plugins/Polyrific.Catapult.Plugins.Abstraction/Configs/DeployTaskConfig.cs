// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class DeployTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Id of the Azure subscription
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Name of the resource group
        /// </summary>
        public string ResourceGroupName { get; set; }

        /// <summary>
        /// Name of the App Service
        /// </summary>
        public string AppServiceName { get; set; }

        /// <summary>
        /// Name of the deployment slot. If empty, it will deploy to production slot.
        /// </summary>
        public string DeploymentSlot { get; set; }
    }
}
