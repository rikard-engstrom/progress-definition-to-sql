using System;

namespace ProgressDefinitionToSql
{
    static class HelpText
    {
        internal static void Print()
        {
            Console.WriteLine("dotnet ProgressDefinitionToSql -i [input file] -o [output file]");
            Console.WriteLine("Supply multiple -i if you want to merge multiple definitions");
            Console.ReadLine();
        }
    }
}
