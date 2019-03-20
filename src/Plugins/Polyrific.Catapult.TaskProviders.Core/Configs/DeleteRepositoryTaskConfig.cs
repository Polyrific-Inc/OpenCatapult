namespace Polyrific.Catapult.TaskProviders.Core.Configs
{
    public class DeleteRepositoryTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Remote repository
        /// </summary>
        public string Repository { get; set; }
    }
}
