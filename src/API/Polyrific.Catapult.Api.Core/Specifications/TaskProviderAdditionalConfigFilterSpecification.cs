// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class TaskProviderAdditionalConfigFilterSpecification : BaseSpecification<TaskProviderAdditionalConfig>
    {
        /// <summary>
        /// Filter Provider additional configs by task provider id
        /// </summary>
        /// <param name="taskProviderId"></param>
        public TaskProviderAdditionalConfigFilterSpecification(int taskProviderId) 
            : base(m => m.TaskProviderId == taskProviderId)
        {
        }

        /// <summary>
        /// Filter Provider additional configs by task provider name
        /// </summary>
        /// <param name="taskProviderName"></param>
        public TaskProviderAdditionalConfigFilterSpecification(string taskProviderName)
            : base(m => m.TaskProvider.Name == taskProviderName)
        {
        }
    }
}
