// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Polyrific.Catapult.Shared.Dto.Constants;

namespace Polyrific.Catapult.Engine.Core.Utils
{
    public class JobTaskConfigUtil
    {
        /// <summary>
        /// Get names of configuration items in a job task config
        /// </summary>
        /// <param name="taskName"></param>
        /// <returns></returns>
        public static string[] GetTaskConfigNames(string taskName)
        {
            string[] names = { };
            BaseJobTaskConfig config = null;

            switch (taskName)
            {
                case JobTaskDefinitionType.Build:
                    config = new BuildTaskConfig();
                    break;
                case JobTaskDefinitionType.Clone:
                    config = new CloneTaskConfig();
                    break;
                case JobTaskDefinitionType.Deploy:
                    config = new DeployTaskConfig();
                    break;
                case JobTaskDefinitionType.DeployDb:
                    config = new DeployDbTaskConfig();
                    break;
                case JobTaskDefinitionType.Generate:
                    config = new GenerateTaskConfig();
                    break;
                case JobTaskDefinitionType.Merge:
                    config = new MergeTaskConfig();
                    break;
                case JobTaskDefinitionType.PublishArtifact:
                    config = new PublishArtifactTaskConfig();
                    break;
                case JobTaskDefinitionType.Push:
                    config = new PushTaskConfig();
                    break;
                case JobTaskDefinitionType.Test:
                    config = new TestTaskConfig();
                    break;
            }

            if (config != null)
            {
                names = config.GetType().GetProperties().Select(p => p.Name).ToArray();
            }

            return names;
        }
    }
}
