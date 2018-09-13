// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class FilterTypeNotFoundException : Exception
    {
        public string FilterType { get; set; }

        public FilterTypeNotFoundException(string filterType)
            : base($"Filter type \"{filterType}\" was not found.")
        {
            FilterType = filterType;
        }
    }
}