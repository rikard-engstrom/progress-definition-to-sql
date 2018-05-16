using System;

namespace ProgressDefinitionToSql.Models
{
    public class FieldIndex
    {
        public string FieldName { get; set; }
        public bool Ascending { get; set; }

        internal static FieldIndex Parse(string definition)
        {
            var parts = definition.Split('"', StringSplitOptions.RemoveEmptyEntries);
            return new FieldIndex
            {
                FieldName = parts[1],
                Ascending = parts[2].Trim().Equals("ASCENDING")
            };
        }
    }
}
