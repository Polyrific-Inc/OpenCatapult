// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Engine.Core.JobTasks;

namespace Polyrific.Catapult.Engine.Core
{
    /// <summary>
    /// Wrapper service to hold job task instances
    /// </summary>
    public class JobTaskService
    {
        public JobTaskService(IBuildTask buildTask, 
            ICloneTask cloneTask,
            IDeployTask deployTask, 
            IDeployDbTask deployDbTask,
            IGenerateTask generateTask,
            IMergeTask mergeTask,
            IPublishArtifactTask publishArtifactTask,
            IPushTask pushTask,
            ITestTask testTask,
            IDeleteRepositoryTask deleteRepositoryTask,
            IDeleteHostingTask deleteHostingTask)
        {
            BuildTask = buildTask;
            CloneTask = cloneTask;
            DeployTask = deployTask;
            DeployDbTask = deployDbTask;
            GenerateTask = generateTask;
            MergeTask = mergeTask;
            PublishArtifactTask = publishArtifactTask;
            PushTask = pushTask;
            TestTask = testTask;
            DeleteRepositoryTask = deleteRepositoryTask;
            DeleteHostingTask = deleteHostingTask;
        }

        /// <summary>
        /// Instance of <see cref="IBuildTask"/>
        /// </summary>
        public IBuildTask BuildTask { get; }

        /// <summary>
        /// Instance of <see cref="ICloneTask"/>
        /// </summary>
        public ICloneTask CloneTask { get; }

        /// <summary>
        /// Instance of <see cref="IDeployTask"/>
        /// </summary>
        public IDeployTask DeployTask { get; }

        /// <summary>
        /// Instance of <see cref="DeployDbTask"/>
        /// </summary>
        public IDeployDbTask DeployDbTask { get; }

        /// <summary>
        /// Instance of <see cref="IGenerateTask"/>
        /// </summary>
        public IGenerateTask GenerateTask { get; }

        /// <summary>
        /// Instance of <see cref="IMergeTask"/>
        /// </summary>
        public IMergeTask MergeTask { get; }

        /// <summary>
        /// Instance of <see cref="IPublishArtifactTask"/>
        /// </summary>
        public IPublishArtifactTask PublishArtifactTask { get; }

        /// <summary>
        /// Instance of <see cref="IPushTask"/>
        /// </summary>
        public IPushTask PushTask { get; }

        /// <summary>
        /// Instance of <see cref="ITestTask"/>
        /// </summary>
        public ITestTask TestTask { get; }

        /// <summary>
        /// Instance of <see cref="IDeleteRepositoryTask"/>
        /// </summary>
        public IDeleteRepositoryTask DeleteRepositoryTask { get; }

        /// <summary>
        /// Instance of <see cref="IDeleteHostingTask"/>
        /// </summary>
        public IDeleteHostingTask DeleteHostingTask { get; }
    }
}
