// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace EntityFrameworkCore
{
    public interface IDatabaseCommand
    {
        /// <summary>
        /// Run the dotnet ef database update command
        /// </summary>
        /// <param name="dataProject">The --project option</param>
        /// <param name="startupProject">The --startup-project option</param>
        /// <param name="configuration">The --configuration option</param>
        /// <returns></returns>
        Task<string> Update(string dataProject, string startupProject, string configuration = "Debug");
    }
}
