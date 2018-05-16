using ProgressDefinitionToSql.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProgressDefinitionToSql.Models
{
    public class FieldDefinition : IDefinition
    {
        private static Regex fieldRegex = new Regex("ADD FIELD\\s\"(.+?)\"\\sOF\\s\"(.+?)\"\\sAS\\s(.+)", RegexOptions.Compiled);

        public string FieldName { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public string FieldType { get; set; }
        public string FieldFormat { get; set; }

        public string ToSql()
        {
            return $"[{FieldName}] {GetSqlType()}";
        }

        private string GetSqlType()
        {
            if (FieldType.Equals("int64", StringComparison.InvariantCultureIgnoreCase))
            {
                return "integer";
            }
            else if (FieldType.Equals("logical", StringComparison.InvariantCultureIgnoreCase))
            {
                return "bit";
            }
            else if (FieldType.Equals("raw", StringComparison.InvariantCultureIgnoreCase))
            {
                return "binary";
            }
            else if (FieldType.Equals("clob", StringComparison.InvariantCultureIgnoreCase))
            {
                return "text";
            }
            else if (FieldType.Equals("blob", StringComparison.InvariantCultureIgnoreCase))
            {
                return "binary";
            }

            return FieldType;
        }

        public static IDefinition Parse(string[] section)
        {
            var firstLine = fieldRegex.Match(section[0]);

            return new FieldDefinition
            {
                FieldName = firstLine.Groups[1].ToString(),
                FieldType = firstLine.Groups[3].ToString().Trim(),
                FieldFormat = section.Single(x => x.StartsWith("  FORMAT ")).Substring(8).Trim('"', ' '),
                TableName = firstLine.Groups[2].ToString(),
                Description = section.ParseDescription()
            };
        }
    }
}