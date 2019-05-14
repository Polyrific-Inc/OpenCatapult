// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class ExternalServiceFilterSpecification : BaseSpecification<ExternalService>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string[] Names { get; set; }

        public ExternalServiceFilterSpecification(int id, string name)
            : base(m => (id == 0 || m.Id == id) && (name == null || m.Name == name))
        {
            Id = id;
            Name = name;
        }

        public ExternalServiceFilterSpecification(int userId)
            : base(m => m.UserId == userId || m.IsGlobal)
        {
            UserId = userId;
        }
    }
}
