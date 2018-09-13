// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Polyrific.Catapult.Engine.Commands;

namespace Polyrific.Catapult.Engine
{
    public static class CommandRegistration
    {
        public static void RegisterCommands(this CommandLineApplication app)
        {
            app.Command<StartCommand>("start", _ => { });
            app.Command<CheckCommand>("check", _ => { });
            app.Command<ConfigCommand>("config", _ => { });
        }
    }
}