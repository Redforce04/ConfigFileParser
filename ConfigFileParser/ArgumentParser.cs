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
                    
                    if(Config.Singleton.Debug) SleepManager.Sleep(2000);
                    return;
                }
                result = rootDirectory;
            }

            result = "";
            if(Config.Singleton.Debug) SleepManager.Sleep(2000);
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
                string? nextArg = (i + 1 == args.Length) ? null : args[i + 1];

                // if (conf.Debug) Console.WriteLine($"Argument: {argument}");

                switch (argument.ToLower())
                {
                    case "--debug":
                        Config.Singleton.Debug = true;
                        break;
                    case "--development-mode":
                        Config.Singleton.DevelopmentMode = true;
                        break;
                    case "--attach-debugger":
                        if (!Config.Singleton.AttachDebugger)
                        {
                            Debugger.Launch();
                        }
                        Config.Singleton.AttachDebugger = true;
                        break;
                    case "--interactive-startup":
                        Config.Singleton.InteractiveStartup = true;
                        break;
                    case "-r" or "--force-reconfigure":
                        FileManager.RunInitializerScript = true;
                        break;
                    case "--no-auto-updater":
                        Config.Singleton.AutoUpdate = false;
                        break;
                    case "-c" or "--use-cached-config":
                        Config.Singleton.UseCache = true;
                        break;
                    case "--github-api-key":
                        if (nextArg != "")
                        {
                            Config.Singleton.GithubApiKey = nextArg;
                            skipNext = true;
                        }

                        break;
                    case "--color-test":
                        Console.WriteLine("Testing Console Colors: ");
                        List<ConsoleColor> rainbow = new List<ConsoleColor>()
                        {
                            ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Gray,
                            ConsoleColor.White, ConsoleColor.DarkRed, ConsoleColor.Red, 
                            ConsoleColor.Yellow, ConsoleColor.DarkYellow, ConsoleColor.Green,
                            ConsoleColor.DarkGreen, ConsoleColor.Cyan, ConsoleColor.DarkCyan,
                            ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.Magenta,
                            ConsoleColor.DarkMagenta,
                        };
                        List<ConsoleColor> colorsCategorized = new List<ConsoleColor>()
                        {
                            ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Gray,
                            ConsoleColor.White, ConsoleColor.Red, ConsoleColor.Yellow,
                            ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Blue, 
                            ConsoleColor.Magenta, ConsoleColor.DarkRed, ConsoleColor.DarkYellow,
                            ConsoleColor.DarkGreen, ConsoleColor.DarkCyan, ConsoleColor.DarkBlue,
                            ConsoleColor.DarkMagenta,
                        };
                        List<ConsoleColor> brightness = new List<ConsoleColor>()
                        {
                            ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.DarkRed,
                            ConsoleColor.DarkYellow, ConsoleColor.DarkGreen, ConsoleColor.DarkCyan, 
                            ConsoleColor.DarkBlue, ConsoleColor.DarkMagenta, ConsoleColor.Gray,
                            ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green,
                            ConsoleColor.Cyan, ConsoleColor.Blue, ConsoleColor.Magenta,
                            ConsoleColor.White,
                        };
                        List<ConsoleColor> activeList = brightness;
                        if (nextArg != null)
                        {
                            switch (nextArg)
                            {
                                case "rainbow":
                                    activeList = rainbow;
                                    break;
                                case "categorized":
                                    activeList = colorsCategorized;
                                    break;
                                case "brightness":
                                    activeList = brightness;
                                    break;
                                case "all":
                                    activeList = Enum.GetValues<ConsoleColor>().ToList();
                                    break;
                            }
                        }
                        foreach (ConsoleColor color in activeList)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"[");
                            Console.ForegroundColor = color;
                            Console.Write($"[{color} - color]");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"]\n");
                        }

                        Console.WriteLine("Ending App in 5 seconds.");
                        Thread.Sleep(5000);
                        Environment.Exit(0);
                        break;
                    case "-q" or "--quiet":
                        Config.Singleton.Silent = true;
                        break;
                    case "-qq" or "--quietquiet":
                        Config.Singleton.Silent = true;
                        Config.Singleton.SuperSilent = true;
                        break;
                    case "--theme":
                        if (nextArg != null)
                        {
                            switch (nextArg)
                            {
                                case "pterodactyl":
                                    Config.ExportedColorScheme = ColorSchemes.Pterodactyl;
                                    break;
                                case "linux":
                                    Config.ExportedColorScheme = ColorSchemes.Linux;
                                    break;
                                case "windows":
                                    Config.ExportedColorScheme = ColorSchemes.Windows;
                                    break;
                            }

                            skipNext = true;
                        }

                        break;
                    case "-i" or "--input-parsing":
                        if (nextArg == null)
                        {
                            CustomTextParser.Singleton.PrintLine($"<Warn>You must specify a valid parsing method after -i or --input-parsing");
                            ParsedSuccessfully = false;
                            return;
                        }

                        Config.Singleton.AttemptedInputParser = nextArg;
                        Config.Singleton.InputParserType = Enum.Parse<ParserType>(nextArg, true);
                        if (Config.Singleton.Debug)
                            Console.WriteLine($"Input Parsing: {Config.Singleton.InputParserType}");
                        skipNext = true;
                        break;
                    case "-o" or "--output-parsing":
                        if (nextArg == null)
                        {
                            CustomTextParser.Singleton.PrintLine($"<Warn>You must specify a valid parsing method after -o or --output-parsing");
                            ParsedSuccessfully = false;
                            return;
                        }

                        Config.Singleton.AttemptedOutputParser = nextArg;
                        Config.Singleton.OutputParserType = Enum.Parse<ParserType>(nextArg, true);
                        if (Config.Singleton.Debug)
                            Console.WriteLine($"Output Parsing: {Config.Singleton.InputParserType}");
                        skipNext = true;
                        break;
                    default:
                        if (Config.Singleton.InputFileLoc == "")
                        {
                            Config.Singleton.InputFileLoc = argument;
                            if (Config.Singleton.Debug)
                                Console.WriteLine($"Input Loc: {Config.Singleton.InputFileLoc}");
                            if (Config.Singleton.InputParserType != ParserType.None)
                            {
                                break;
                            }

                            string[] subArray = Config.Singleton.InputFileLoc.Split(".");
                            Config.Singleton.AttemptedInputParser = subArray[subArray.Length - 1];
                            Config.Singleton.InputParserType =
                                Enum.Parse<ParserType>(Config.Singleton.AttemptedInputParser, true);

                        }
                        else
                        {

                            Config.Singleton.OutputFileLoc = argument;
                            if (Config.Singleton.Debug)
                                Console.WriteLine($"Output Loc: {Config.Singleton.OutputFileLoc}");
                            if (Config.Singleton.OutputParserType != ParserType.None)
                            {
                                break;
                            }

                            string[] subArray = Config.Singleton.OutputFileLoc.Split(".");
                            Config.Singleton.AttemptedOutputParser = subArray[subArray.Length - 1];

                            Config.Singleton.OutputParserType =
                                Enum.Parse<ParserType>(Config.Singleton.AttemptedOutputParser, true);
                        }

                        break;
                    case "--updateNew":
                        try
                        {
                            CustomTextParser.Singleton.PrintLine($"<Warn>Restarting after rename");
                            string srcFile = "";
                            string destFile = "";
                            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
                            {
                                var split = file.Replace("\\", "/");
                                string fileName = split.Split('/')[^1];
                                string fileEnd = fileName.Contains(".") ? "." + fileName.Split('.')[^1] : "";
                                if (file.Contains("BetterConfigs") && file.Contains("-new") &&
                                    !file.EndsWith(".dll") &&
                                    !file.EndsWith(".pdb") && !file.EndsWith(".deps.json") &&
                                    !file.EndsWith(".runtimeconfig.json"))
                                {
                                    srcFile = file;
                                    destFile = split.Replace($"/{fileName}", "") + "/MGHBetterConfigs" + fileEnd;
                                    break;
                                }
                            }

                            if (Config.Singleton.Debug)
                            {
                                Console.WriteLine($"Found Source File: {srcFile}, replacing dest file: {destFile}");
                            }

                            File.Delete(destFile);
                            File.Copy(srcFile, destFile, true);

                            List<string> curArgs = args.ToList();
                            curArgs.Remove($"--updateNew");
                            curArgs.Add("--updateRestart");
                            Process.Start(destFile, curArgs);
                            Environment.Exit(0);

                            return;
                        }
                        catch (Exception e)
                        {
                            if (Config.Singleton.Debug)
                            {
                                Console.WriteLine(e);
                            }

                            Environment.Exit(0);
                            return;
                        }
                    case "--updateRestart":
                        try
                        {
                            CustomTextParser.Singleton.PrintLine("Update installed successfully.");
                            Config.Singleton.AutoUpdate = false;
                            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
                            {
                                string fileName = file.Replace("\\", "/").Split('/')[^1];
                                if (fileName.Contains("-new") && fileName.Contains("BetterConfigs"))
                                {
                                    File.Delete(file);
                                    break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            if (Config.Singleton.Debug)
                            {
                                Console.WriteLine($"Caught exception: {e}");
                            }
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
