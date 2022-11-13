using System;
using System.IO;
using System.Text;
using ObfuscarMappingParser.Engine;

namespace ObfuscarMappingParser
{
    class CmdLine
    {

        internal const string HELP_STRING = @"Usage:
ObfuscarMappingParser.exe
    This help
ObfuscarMappingParser.exe Mapping.xml [backtrace.txt]
    Open mapping, read stacktrace from standart input or 'backtrace.txt' file and write processed stacktrace to the standart output";

        public static void Main(string[] args)
        {
            string mapping_filename;
            string backtrace_filename = null;

            if (args.Length == 0)
            {
                mapping_filename = null;
                WriteHelp();
                return;
            }

            mapping_filename = args[0];

            if (args.Length > 1)
                backtrace_filename = args[1];

            String backtrace;

            if (backtrace_filename == null)
            {
                string s;
                StringBuilder sb = new StringBuilder();

                while (!string.IsNullOrEmpty(s = Console.ReadLine()))
                    sb.AppendLine(s);
                backtrace = sb.ToString();
            } else {
                backtrace = File.ReadAllText(backtrace_filename);
            }

            ParserConfigs.Instance = new DefConfig();

            Mapping mapping = new Mapping(mapping_filename);

            Console.WriteLine(mapping.ProcessCrashlogText(backtrace.ToString()));
        }

        private static void WriteHelp()
        {
            Console.WriteLine(HELP_STRING);
        }

    }

    class DefConfig : IParserConfigs
    {
        bool IParserConfigs.SimplifyNullable => true;

        bool IParserConfigs.SimplifyRef => true;

        bool IParserConfigs.SimplifySystemNames => true;
    }

}
