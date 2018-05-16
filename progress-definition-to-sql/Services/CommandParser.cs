using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProgressDefinitionToSql.Services
{
    class CommandParser
    {
        private readonly int argsCount;

        public string[] InputFiles { get; }
        public string OutputFile { get; }

        public CommandParser(string[] args)
        {
            argsCount = args.Length;

            var arguments = GetKeyValuePairs(args);
            InputFiles = arguments.Where(x => x.Key.Equals("-i", StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Value).ToArray();
            OutputFile = arguments.Single(x => x.Key.Equals("-o", StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        public void ValidateArguments()
        {
            if (argsCount % 2 != 0)
            {
                throw new Exception("Invalid number of arguments.");
            }

            if (!InputFiles.Any())
            {
                throw new Exception("No input file specified.");
            }

            if (string.IsNullOrWhiteSpace(OutputFile))
            {
                throw new Exception("No output file specified.");
            }

            foreach (var file in InputFiles)
            {
                if (!File.Exists(file))
                {
                    throw new Exception($"File [{file}] does not exist.");
                }
            }
        }

        private IEnumerable<KeyValuePair<string, string>> GetKeyValuePairs(string[] args)
        {
            for (int i = 0; i < args.Length - 1; i += 2)
            {
                yield return new KeyValuePair<string, string>(args[i], args[i + 1]);
            }
        }
    }
}
