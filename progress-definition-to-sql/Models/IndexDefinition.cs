using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgressDefinitionToSql.Models
{
    public class IndexDefinition : IDefinition
    {
        public string IndexName { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsUnique { get; set; }
        public List<FieldIndex> Fields { get; set; } = new List<FieldIndex>();
        public string TableName { get; set; }

        internal static IDefinition Parse(string[] section)
        {
            var firstLine = section[0].Split('"', StringSplitOptions.RemoveEmptyEntries);
            var index = new IndexDefinition
            {
                IndexName = firstLine[1],
                IsUnique = section.Any(x => x.Trim() == "UNIQUE"),
                IsPrimary = section.Any(x => x.Trim() == "PRIMARY"),
                TableName = firstLine[3]
            };

            index.Fields.AddRange(section.Where(x => x.StartsWith("  INDEX-FIELD \"")).Select(FieldIndex.Parse));
            return index;
        }
    }
}
