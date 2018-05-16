using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProgressDefinitionToSql.Services
{
    static class FileUtility
    {
        internal static string[] ReadAllLines(string[] files)
        {
            var lines = new List<string>();

            foreach (var file in files)
            {
                lines.AddRange(ReadAllLines(file));
                lines.Add(string.Empty);
            }

            return lines.ToArray();
        }

        private static string[] ReadAllLines(string file)
        {
            if (!File.Exists(file))
            {
                throw new Exception($"Could not find input file \"{file}\"");
            }

            try
            {
                return File.ReadAllLines(file, Encoding.GetEncoding("iso8859-1"));
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not read from input file \"{file}\"\r\nError: {ex.Message}");
            }
        }
    }
}
