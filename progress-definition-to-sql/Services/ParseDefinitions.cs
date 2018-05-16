using ProgressDefinitionToSql.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProgressDefinitionToSql.Services
{
    public class ParseDefinitions
    {
        public IEnumerable<TableDefinition> Parse(string[] definitionLines)
        {
            var tables = SplitOnEmptyLine(definitionLines)
                                .Select(ParseSection)
                                .Where(x => x != null)
                                .GroupBy(x => x.TableName)
                                .Select(grp =>
                                {
                                    var table = grp.Single(x => x is TableDefinition) as TableDefinition;
                                    table.Fields.AddRange(grp.Where(x => x is FieldDefinition).Cast<FieldDefinition>());
                                    table.Indecis.AddRange(grp.Where(x => x is IndexDefinition).Cast<IndexDefinition>());
                                    return table;
                                })
                                .ToArray();

            return tables;
        }

        private IDefinition ParseSection(string[] section)
        {
            if (section[0].StartsWith("ADD TABLE \""))
            {
                return TableDefinition.Parse(section);
            }
            else if (section[0].StartsWith("ADD FIELD \""))
            {
                return FieldDefinition.Parse(section);
            }
            else if (section[0].StartsWith("ADD INDEX \""))
            {
                return IndexDefinition.Parse(section);
            }

            return null;
        }

        private string[][] SplitOnEmptyLine(string[] definitionLines)
        {
            int section = 0;

            return definitionLines.Select(line =>
            {
                if (line == string.Empty)
                {
                    section++;
                }

                return new { section, line };
            })
            .Where(x => x.line != string.Empty)
            .GroupBy(x => x.section)
            .Select(x => x.Select(l => l.line).ToArray())
            .ToArray();
        }
    }
}
