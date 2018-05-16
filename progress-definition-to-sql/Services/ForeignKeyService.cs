using ProgressDefinitionToSql.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProgressDefinitionToSql.Services
{
    static class ForeignKeyService
    {
        /// <summary>
        /// Guess foreign keys based on same column name
        /// </summary>
        public static IEnumerable<string> SuggestForeignKeys(IEnumerable<TableDefinition> tables)
        {
            var primaryKeys = tables
                .Where(table => table.Indecis.Any(i => i.IsPrimary && i.IsUnique))
                .Select(table =>
                {
                    var primaryKey = table.Indecis.SingleOrDefault(i => i.IsPrimary && i.IsUnique);
                    return new
                    {
                        fields = string.Concat(",", string.Join(",", primaryKey?.Fields.Select(f => f.FieldName).OrderBy(f => f)).ToLowerInvariant(), ","),
                        primaryKey,
                        table
                    };
                })
                .OrderBy(x => x.fields)
                .GroupBy(x => x.fields)
                .Where(grp => grp.Count() == 1)
                .Select(grp => grp.Single())
                .ToArray();

            foreach (var table in tables)
            {
                var columns = string.Concat(",", string.Join(",", table.Fields.Select(x => x.FieldName.ToLower())), ",");
                var foreignKeys = primaryKeys
                                    .Where(key => key.table.TableName != table.TableName)
                                    .Where(key => columns.Contains(key.fields));

                foreach (var foreignKey in foreignKeys)
                {
                    var fields = string.Join(",", foreignKey.primaryKey.Fields.Select(x => $"[{x.FieldName}]"));
                    var name = fields.Replace(",", "_").Replace("[", string.Empty).Replace("]", string.Empty);
                    yield return $"ALTER TABLE [{table.TableName}] ADD CONSTRAINT [FK_{name}_{table.TableName}] FOREIGN KEY ({fields}) REFERENCES [{foreignKey.table.TableName}] ({fields})\r\ngo";
                }
            }
        }
    }
}
