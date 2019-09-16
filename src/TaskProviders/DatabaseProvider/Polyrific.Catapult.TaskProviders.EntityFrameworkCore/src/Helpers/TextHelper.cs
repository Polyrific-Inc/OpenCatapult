using Humanizer;

namespace Polyrific.Catapult.TaskProviders.EntityFrameworkCore.Helpers
{
    public static class TextHelper
    {
        public static string GetNormalizedName(string text)
        {
            return text.Replace("-", "_").Pascalize();
        }
    }
}
