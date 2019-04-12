// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class ProviderAdditionalConfigFilterSpecification : BaseSpecification<ProviderAdditionalConfig>
    {
        /// <summary>
        /// Filter Provider additional configs by provider id
        /// </summary>
        /// <param name="providerId"></param>
        public ProviderAdditionalConfigFilterSpecification(int providerId) 
            : base(m => m.ProviderId == providerId)
        {
        }

        /// <summary>
        /// Filter Provider additional configs by provider name
        /// </summary>
        /// <param name="providerName"></param>
        public ProviderAdditionalConfigFilterSpecification(string providerName)
            : base(m => m.Provider.Name == providerName)
        {
        }
    }
}
