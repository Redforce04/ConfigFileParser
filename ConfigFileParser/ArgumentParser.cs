using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConfigFileParser.Components;
using ConfigFileParser.Configs;

namespace ConfigFileParser
{
    internal class ArgumentParser
    {
        public static ArgumentParser Singleton;
        public bool ParsedSuccessfully = false;
        internal ArgumentParser(string[] args)
        {
            Singleton = this;
            try
            {
                processArgs(args);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"You must specify a valid method of parsing. '{(Config.Singleton.AttemptedOutputParser == "" ? Config.Singleton.AttemptedInputParser : Config.Singleton.AttemptedOutputParser)}' is not a valid parsing method.");
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "An error has occured while parsing arguments. Please contact redforce04 on discord.");
                if (Config.Singleton.Debug)
                {
                    Console.WriteLine($"{e}");
                }
            }

            if (Config.Singleton.InputFileLoc == "")
            {
                FindDirectory(out string result);
                if (result != "")
                {
                    Config.Singleton.InputFileLoc = result + "Config.json";
                    if (Config.Singleton.InputParserType == ParserType.None)
                        Config.Singleton.InputParserType = ParserType.Json;
                }
            }

            if (Config.Singleton.OutputFileLoc == "")
            {
                FindDirectory(out string result);
                if (result != "")
                {
                    Config.Singleton.OutputFileLoc = result + "CustomRules.ini";
                    if (Config.Singleton.OutputParserType == ParserType.None)
                        Config.Singleton.OutputParserType = ParserType.Ini;
                }
            }
            verifyResults();
        }
        private void FindDirectory(out string result)
        {
            if (Config.Singleton.Debug)
            {
                Debugger.Launch();
            }
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string rootDirectory = "";
            var localDirectoryFiles = Directory.GetFiles(baseDirectory);
            // if(Config.Singleton.Debug) Console.WriteLine($"Searching Local Directory: {baseDirectory}");
                // if(Config.Singleton.Debug) Console.WriteLine($"Files: {localDirectoryFiles.Aggregate((x,y) => x += $" {y}")}");

            if(localDirectoryFiles.Contains(baseDirectory + "MidnightGhostHuntServer.exe"))
            {
                result = baseDirectory;
                return;
            }

            foreach (string path in Directory.GetDirectories(baseDirectory))
            {
                var downwardFiles = Directory.GetFiles(path);
                // if(Config.Singleton.Debug) Console.WriteLine($"Searching Upward Directory: {path}");

                // if(Config.Singleton.Debug) Console.WriteLine($"Files: {downwardFiles.Aggregate((x,y) => x += $" {y}")}");

                if (downwardFiles.Contains(path + "/MidnightGhostHuntServer.exe"))
                {
                    result = path;
                    return;
                }
            }

            if (rootDirectory == "")
            {
                string upwardDirectory = "";
                var roots = baseDirectory.Replace("\\", "/").Split("/");
                for (int i = 0; i < roots.Length - 1; i++)
                {
                    upwardDirectory += $"{roots[i]}/";
                }

                // if(Config.Singleton.Debug) Console.WriteLine($"Searching Upward Directory: {upwardDirectory}");
                var upwardDirectoryFiles = Directory.GetFiles(upwardDirectory);
                // if(Config.Singleton.Debug) Console.WriteLine($"Files: {upwardDirectoryFiles.Aggregate((x,y) => x += $" {y}")}");
                if (Directory.Exists(upwardDirectory) &&
                    upwardDirectoryFiles.Contains(upwardDirectory+"MidnightGhostHuntServer.exe"))
                {
                    result = upwardDirectory;
                    return;
                }

                if (rootDirectory == "")
                {
                    CustomTextParser.Singleton.PrintLine($"<Warn>Could not find MidnightGhostHuntServer.exe automatically. Please manually specify the location.");
                    result = "";
                    
                    if(Config.Singleton.Debug) Thread.Sleep(6000);
                    return;
                }
                result = rootDirectory;
            }

            result = "";
            if(Config.Singleton.Debug) Thread.Sleep(6000);
        }
        private void processArgs(string[] args)
        {
            bool skipNext = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (skipNext)
                {
                    if (Config.Singleton.Debug) Console.WriteLine($"Skipping argument.");
                    skipNext = false;
                    continue;
                }
                string argument = args[i];
                string? nextArg = (i + 1 == args.Length ) ? null : args[i + 1];

                // if (conf.Debug) Console.WriteLine($"Argument: {argument}");
                
                    switch (argument.ToLower())
                    {
                        case "--debug":
                            break;
                        case "-r" or "--force-reconfigure":
                            FileManager.RunInitializerScript = true;
                            break;
                        case "--no-auto-updater":
                            Config.Singleton.AutoUpdate = false;
                            break;
                        case "-c" or "--use-cached-config":
                            Config.Singleton.UseCache = false;
                            break;
                        
                        case "-q" or "--quiet":
                            Config.Singleton.Silent = true;
                            break;
                        case "-qq" or "--quietquiet":
                            Config.Singleton.Silent = true;
                            Config.Singleton.SuperSilent = true;
                            break;
                        case "-i" or "--input-parsing":
                            if (nextArg == null)
                            {
                                Console.WriteLine($"You must specify a valid parsing method after -i or --input-parsing");
                                ParsedSuccessfully = false;
                                return;
                            }
                            Config.Singleton.AttemptedInputParser = nextArg;
                            Config.Singleton.InputParserType = Enum.Parse<ParserType>(nextArg, true);
                            if (Config.Singleton.Debug) Console.WriteLine($"Input Parsing: {Config.Singleton.InputParserType}");
                            skipNext = true;
                            break;
                        case "-o" or "--output-parsing":
                            if (nextArg == null)
                            {
                                Console.WriteLine($"You must specify a valid parsing method after -o or --output-parsing");
                                ParsedSuccessfully = false;
                                return;
                            }
                            Config.Singleton.AttemptedOutputParser = nextArg;
                            Config.Singleton.OutputParserType = Enum.Parse<ParserType>(nextArg, true);
                            if (Config.Singleton.Debug) Console.WriteLine($"Output Parsing: {Config.Singleton.InputParserType}");
                            skipNext = true;
                            break;
                        default:
                            if (Config.Singleton.InputFileLoc == "")
                            {
                                Config.Singleton.InputFileLoc = argument;
                                if (Config.Singleton.Debug) Console.WriteLine($"Input Loc: {Config.Singleton.InputFileLoc}");
                                if (Config.Singleton.InputParserType != ParserType.None)
                                {
                                    break;
                                }

                                string[] subArray = Config.Singleton.InputFileLoc.Split(".");
                                Config.Singleton.AttemptedInputParser = subArray[subArray.Length - 1];
                                Config.Singleton.InputParserType = Enum.Parse<ParserType>(Config.Singleton.AttemptedInputParser, true);

                            }
                            else
                            {

                                Config.Singleton.OutputFileLoc = argument;
                                if (Config.Singleton.Debug) Console.WriteLine($"Output Loc: {Config.Singleton.OutputFileLoc}");
                                if (Config.Singleton.OutputParserType != ParserType.None)
                                {
                                    break;
                                }

                                string[] subArray = Config.Singleton.OutputFileLoc.Split(".");
                                Config.Singleton.AttemptedOutputParser = subArray[subArray.Length - 1];

                                Config.Singleton.OutputParserType = Enum.Parse<ParserType>(Config.Singleton.AttemptedOutputParser, true);
                            }

                            break;
                    }


                }
        }

        private void verifyResults()
        {
            if (Config.Singleton.InputFileLoc.Length == 0)
            {
                Console.WriteLine($"Error: Could not find input file location '{Config.Singleton.InputFileLoc}'");
                return;
            }

            if (Config.Singleton.OutputFileLoc.Length == 0)
            {
                Console.WriteLine($"Error: Could not find output file location '{Config.Singleton.OutputFileLoc}'");
                return;
            }

            if (Config.Singleton.InputParserType == ParserType.None)
            {
                Console.WriteLine($"Error: Could not determine the type of parser to use for the input file. Please manually specify the parser with the -i or --input-parsing argument.");
                return;
            }
            if (Config.Singleton.OutputParserType == ParserType.None)
            {
                Console.WriteLine($"Error: Could not determine the type of parser to use for the output file. Please manually specify the parser with the -o or --output-parsing argument.");
                return;
            }

            ParsedSuccessfully = true;
        }
    }
}
