using ProgressDefinitionToSql.Services;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ProgressDefinitionToSql
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var commandParser = new CommandParser(args);
                commandParser.ValidateArguments();

                var definitionLines = FileUtility.ReadAllLines(commandParser.InputFiles);
                Run(definitionLines, commandParser.OutputFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                HelpText.Print();
            }
        }

        private static void Run(string[] definitionLines, string output)
        {
            var tables = new ParseDefinitions().Parse(definitionLines);
            var sql = string.Join("\r\n\r\n", tables.Select(x => x.ToSql()));
            var fk = ForeignKeyService.SuggestForeignKeys(tables);

            File.WriteAllText(output, sql, Encoding.UTF8);
            File.AppendAllLines(output, new[] { "\r\n-- Foreign keys --\r\n" }, Encoding.UTF8);
            File.AppendAllLines(output, fk, Encoding.UTF8);
        }
    }
}
