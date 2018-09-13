// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System;
using System.Linq;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class JobCounterFilterSpecification : BaseSpecification<JobCounter>
    {
        public DateTime Date { get; set; }

        public JobCounterFilterSpecification(DateTime date)
            : base(m => m.Date.Year == date.Year && m.Date.Month == date.Month && date.Day == date.Day)
        {
            Date = date;
        }
    }
}
