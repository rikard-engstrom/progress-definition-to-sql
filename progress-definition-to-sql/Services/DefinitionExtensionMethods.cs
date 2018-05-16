using System.Text.RegularExpressions;

namespace ProgressDefinitionToSql.Services
{
    static class DefinitionExtensionMethods
    {
        private static Regex descriptionRegex = new Regex("DESCRIPTION\\s\"((?:[^\"]|\"\")+)\"", RegexOptions.Compiled);

        internal static string ParseDescription(this string[] section)
        {
            var match = descriptionRegex.Match(string.Join("\r\n", section));
            var description = match.Success ? match.Groups[1].ToString() : null;
            return description?.Replace("\"\"", "\"");
        }
    }
}
