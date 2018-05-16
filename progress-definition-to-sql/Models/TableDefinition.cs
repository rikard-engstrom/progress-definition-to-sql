using ProgressDefinitionToSql.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgressDefinitionToSql.Models
{
    public class TableDefinition : IDefinition
    {
        public string TableName { get; set; }
        public string Description { get; set; }

        public List<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();
        public List<IndexDefinition> Indecis { get; set; } = new List<IndexDefinition>();

        public string ToSql()
        {
            return $"CREATE TABLE [{TableName}] ({FieldsSql()}{PrimaryKey()})\r\ngo\r\n{CreateIndecis()}\r\n{DescriptionsSql()}";
        }

        private string CreateIndecis()
        {
            return string.Join(
                            "\r\n",
                            Indecis.Where(x => !x.IsPrimary)
                                            .Select(x => new { x.IndexName, x.IsUnique, columns = string.Join(",", x.Fields.Select(f => $"[{f.FieldName}]")) })
                                            .Select(x => $"CREATE {(x.IsUnique ? "UNIQUE " : null)}INDEX [{x.IndexName}] ON {TableName} ({x.columns})\r\ngo"));
        }

        private string DescriptionsSql()
        {
            return string.Concat(MSDescription(Description?.Trim()), "\r\n", string.Join("\r\n", Fields.Select(x => MSDescription(x.Description?.Trim(), x.FieldName)).Where(x => x != null)));
        }

        private string MSDescription(string description, string fieldName = null)
        {
            if (description == null)
            {
                return null;
            }

            description = description.Replace("'", "''");

            if (fieldName == null)
            {
                return $"sp_addextendedproperty @name=N'MS_Description', @value=N'{description}' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'{TableName}'\r\ngo";
            }
            else
            {
                return $"sp_addextendedproperty @name=N'MS_Description', @value=N'{description}' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'{TableName}', @level2type=N'COLUMN', @level2name=N'{fieldName}'\r\ngo";
            }
        }

        private string FieldsSql()
        {
            return "\r\n\t" + string.Join(",\r\n\t", Fields.Select(f => f.ToSql()));
        }

        private string PrimaryKey()
        {
            if (!Indecis.Any(x => x.IsPrimary))
            {
                return null;
            }

            return string.Concat(",\r\n\tPRIMARY KEY(", string.Join(",", Indecis.Single(x => x.IsPrimary).Fields.Select(x => $"[{x.FieldName}]")), ")\r\n");
        }

        public static IDefinition Parse(string[] section)
        {
            return new TableDefinition
            {
                TableName = section[0].Split('"', StringSplitOptions.RemoveEmptyEntries).Last(),
                Description = section.ParseDescription()
            };
        }
    }
}
