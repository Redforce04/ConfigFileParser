// See https://aka.ms/new-console-template for more information

using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using ConfigFileParser;
using ConfigFileParser.Components;
using ConfigFileParser.Configs;
using Newtonsoft.Json.Converters;

//CosturaUtility.Initialize();
var unusedStringEnumConverter = new StringEnumConverter();
Config conf = new Config();
conf.Args = args;
Config.Singleton.Debug = args.Any(x => x == "--debug");
Config.Singleton.InteractiveStartup = args.Any(x => x == "--interactive-startup");
if (args.Any(x => x == "--attach-debugger"))
{
    Config.Singleton.AttachDebugger = true;
    Console.WriteLine("Launching Debugger");
    Debugger.Launch();
}

var unused4 = new CustomTextParser();

CustomTextParser.Singleton.PrintLine("Starting MGH BetterConfigs.");

// string? i = CustomTextParser.Singleton.PrintCoundown("Starting MGH BetterConfigs", 2);
List<string> argsList = args.ToList();
if (Config.Singleton.InteractiveStartup)
{
    CustomTextParser.Singleton.Print("Interactive argument mode is active. Please input additional arguments, or type <White>'skip'<Primary> to skip this step.");
    string? inpt = Console.ReadLine();
    if (inpt is not null && inpt != "")
    {
        if (inpt.ToLower() == "skip")
        {
            CustomTextParser.Singleton.Print("Skipping custom arguments.");
            goto SkipArgs;
        }
        Regex reg = new Regex("\"(.*?)\"|([\\S]*)");

        foreach (Match match in reg.Matches(inpt))
        {
            string val = match.Groups.Count == 0 ? match.Value : match.Groups[^1].Value;
            if (val == "")
            {
                continue;
            }

            argsList.Add(val);
        }
    }
}
SkipArgs:
// CustomTextParser.Singleton.Print("Input arguments added.");
SleepManager.Sleep(2000);
var unused = new ArgumentParser(argsList.ToArray());
if (!ArgumentParser.Singleton.ParsedSuccessfully)
{
    CustomTextParser.Singleton.PrintLine("<Warn>Failed to parse arguments successfully.");
    Console.Read();
    return;
}

if(conf.Debug) Console.WriteLine($"Input Parser: {conf.InputParserType}, Output Parser: {conf.OutputParserType}");
var unused5 = new ConfigUpdater();
var unused2 = new FileManager();
var unused3 = new ConfigParser();
if (FileManager.RunInitializerScript)
{
    ConfigParser.Singleton.RunInitializationScript();
}
else
{
    ConfigParser.Singleton.RegenerateOutput();
}
CustomTextParser.Singleton.PrintLine("Thank you for using MGH Better Configs by Redforce04!. Please report any errors to redforce04 on discord.");




string Help = "Command Usage: ConfigFileParser [source file] [destination file]" +
              "\n" + "-i*, --input-parsing*: source file parsing type. Supported types: ini, json" +
              "\n" + "-o*, --output-parsing*: destination file parsing type. Supported types ini, json" +
              "\n" + "* - This feature may not currently be fully supported.";