// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class MultipleDeletionJobException : Exception
    {
        public MultipleDeletionJobException()
            : base("A deletion job definition is already exist. A project should only contain one deletion job definition.")
        {
        }
    }
}
