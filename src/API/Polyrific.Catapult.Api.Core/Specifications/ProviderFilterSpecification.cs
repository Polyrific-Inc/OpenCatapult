// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class ProviderFilterSpecification : BaseSpecification<Provider>
    {
        /// <summary>
        /// Name of the provider
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Type of the provider
        /// </summary>
        public string ProviderType { get; set; }

        /// <summary>
        /// Id of the provider
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// Filter providers by Name and Type
        /// </summary>
        /// <param name="name">Name of the provider (set null if you don't want to search by Name)</param>
        /// <param name="type">Type of the provider (set null if you don't want to search by Type)</param>
        public ProviderFilterSpecification(string name, string type) 
            : base(m => (name == null || m.Name == name) && (type == null || m.Type == type))
        {
            ProviderName = name;
            ProviderType = type;
        }

        /// <summary>
        /// Filter providers by id
        /// </summary>
        /// <param name="id">Id of the provider</param>
        public ProviderFilterSpecification(int id)
            : base(m => m.Id == id)
        {
            ProviderId = id;
        }
    }
}
