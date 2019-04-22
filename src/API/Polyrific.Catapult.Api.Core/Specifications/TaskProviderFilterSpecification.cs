// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class TaskProviderFilterSpecification : BaseSpecification<TaskProvider>
    {
        /// <summary>
        /// Name of the task provider
        /// </summary>
        public string TaskProviderName { get; set; }

        /// <summary>
        /// Type of the task provider
        /// </summary>
        public string TaskProviderType { get; set; }

        /// <summary>
        /// Id of the task provider
        /// </summary>
        public int TaskProviderId { get; set; }

        /// <summary>
        /// Filter providers by Name and Type
        /// </summary>
        /// <param name="name">Name of the task provider (set null if you don't want to search by Name)</param>
        /// <param name="type">Type of the task provider (set null if you don't want to search by Type)</param>
        public TaskProviderFilterSpecification(string name, string type) 
            : base(m => (name == null || m.Name == name) && (type == null || m.Type == type))
        {
            TaskProviderName = name;
            TaskProviderType = type;
        }

        /// <summary>
        /// Filter providers by id
        /// </summary>
        /// <param name="id">Id of the task provider</param>
        public TaskProviderFilterSpecification(int id)
            : base(m => m.Id == id)
        {
            TaskProviderId = id;
        }
    }
}
