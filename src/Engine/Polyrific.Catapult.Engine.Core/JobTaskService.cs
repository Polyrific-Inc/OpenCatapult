// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Engine.Core.JobTasks;

namespace Polyrific.Catapult.Engine.Core
{
    public class JobTaskService
    {
        public JobTaskService(IBuildTask buildTask, IDeployTask deployTask, IGenerateTask generateTask, IPushTask pushTask)
        {
            BuildTask = buildTask;
            DeployTask = deployTask;
            GenerateTask = generateTask;
            PushTask = pushTask;
        }

        public IBuildTask BuildTask { get; }
        public IDeployTask DeployTask { get; }
        public IGenerateTask GenerateTask { get; }
        public IPushTask PushTask { get; }
    }
}