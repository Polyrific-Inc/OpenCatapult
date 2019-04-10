// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class TagFilterSpecification : BaseSpecification<Tag>
    {
        /// <summary>
        /// Array of tag names
        /// </summary>
        public string[] TagNames { get; set; }

        /// <summary>
        /// Filter the tags by tag names
        /// </summary>
        /// <param name="tagNames">Array of tag names</param>
        public TagFilterSpecification(string[] tagNames)
            : base(m => tagNames.Contains(m.Name))
        {
            TagNames = tagNames;
        }
    }
}
