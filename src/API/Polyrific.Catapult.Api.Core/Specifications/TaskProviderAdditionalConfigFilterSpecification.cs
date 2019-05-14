// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class TaskProviderAdditionalConfigFilterSpecification : BaseSpecification<TaskProviderAdditionalConfig>
    {
        /// <summary>
        /// Filter Provider additional configs by task provider id
        /// </summary>
        /// <param name="taskProviderId">Id of the task provider</param>
        /// <param name="isSecret">is additional config secret?</param>
        public TaskProviderAdditionalConfigFilterSpecification(int taskProviderId, bool? isSecret = null) 
            : base(m => m.TaskProviderId == taskProviderId && (isSecret == null || m.IsSecret == isSecret))
        {
        }

        /// <summary>
        /// Filter Provider additional configs by task provider name
        /// </summary>
        /// <param name="taskProviderName">Name of the task provider</param>
        public TaskProviderAdditionalConfigFilterSpecification(string taskProviderName)
            : base(m => m.TaskProvider.Name == taskProviderName)
        {
        }
    }
}
