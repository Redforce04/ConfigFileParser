// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using ConfigFileParser;
using ConfigFileParser.Components;
using ConfigFileParser.Configs;

//CosturaUtility.Initialize();
Config conf = new Config();

Config.Singleton.Debug = args.Any(x => x == "--debug");
var unused4 = new CustomTextParser();
CustomTextParser.Singleton.PrintLine("Starting MGH BetterConfigs.");
Thread.Sleep(2000);
var unused = new ArgumentParser(args);
if (!ArgumentParser.Singleton.ParsedSuccessfully)
{
    Console.WriteLine("Failed to parse arguments successfully.");
    Console.Read();
    return;
}

if(conf.Debug) Console.WriteLine($"Input Parser: {conf.InputParserType}, Output Parser: {conf.OutputParserType}");
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