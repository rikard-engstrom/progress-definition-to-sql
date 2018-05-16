using NUnit.Framework;
using ProgressDefinitionToSql.Services;
using System.Linq;

namespace ProgressDefinitionToSql.Tests.Services
{
    [TestFixture]
    public sealed class ParseDefinitionsTest
    {
        [Test]
        public void FieldWithMultiLineDescriptionWithQuotes()
        {
            var lines = new[] 
            {
                        "ADD TABLE \"TheTable\"",
                        "  AREA \"tableArea\"",
                        "  DESCRIPTION \"The description of the table\"",
                        "  DUMP-NAME \"thetable\"",
                        string.Empty,
                        "ADD FIELD \"TheField\" OF \"TheTable\" AS character ",
                        "  DESCRIPTION \"First line of field description",
                        "Second line of \"\"field\"\" description\"",
                        "  FORMAT \"x(8)\"",
                        "  INITIAL \"\"",
                        "  POSITION 8",
                        "  MAX-WIDTH 16",
                        "  ORDER 80",
                        "  MANDATORY"
            };

            var result = new ParseDefinitions()
                                .Parse(lines)
                                .ToArray();

            var expectedDescription = string.Join("\r\n", "First line of field description", "Second line of \"field\" description");
            Assert.That(result.Single().Fields.Single().Description, Is.EqualTo(expectedDescription));
        }
    }
}
