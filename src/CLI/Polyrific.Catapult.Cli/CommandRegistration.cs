// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Polyrific.Catapult.Cli.Commands;

namespace Polyrific.Catapult.Cli
{
    public static class CommandRegistration
    {
        public static void RegisterCommands(this CommandLineApplication app)
        {
            app.Command<AccountCommand>("account", _ => { });
            app.Command<JobCommand>("job", _ => { });
            app.Command<LoginCommand>("login", _ => { });
            app.Command<LogoutCommand>("logout", _ => { });
            app.Command<MemberCommand>("member", _ => { });
            app.Command<ModelCommand>("model", _ => { });
            app.Command<ProjectCommand>("project", _ => { });
            app.Command<PropertyCommand>("property", _ => { });
            app.Command<QueueCommand>("queue", _ => { });
            app.Command<TaskCommand>("task", _ => { });
            app.Command<EngineCommand>("engine", _ => { });
            app.Command<ServiceCommand>("service", _ => { });
            app.Command<ConfigCommand>("config", _ => { });
            app.Command<PluginCommand>("plugin", _ => { });
        }
    }
}
