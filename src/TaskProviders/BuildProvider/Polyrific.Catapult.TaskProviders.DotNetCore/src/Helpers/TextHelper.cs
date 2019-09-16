using Humanizer;

namespace Polyrific.Catapult.TaskProviders.DotNetCore.Helpers
{
    public static class TextHelper
    {
        public static string GetNormalizedName(string text)
        {
            return text.Replace("-", "_").Pascalize();
        }
    }
}
