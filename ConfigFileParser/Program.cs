﻿// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Reflection;
using ConfigFileParser;
using ConfigFileParser.Components;
using ConfigFileParser.Configs;

//CosturaUtility.Initialize();
Config conf = new Config();
conf.Args = args;
Config.Singleton.Debug = args.Any(x => x == "--debug");
try
{
if (args.Any(x => x == "--updateNew"))
{
    Console.WriteLine($"Restarting after rename");
    string curFile = "";
    foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
    {
        var split = file.Replace("\\", "/");
        string fileName = split.Split('/')[^1];
        string fileEnd = fileName.Contains(".") ?  "." + fileName.Split('.')[^1] : "";
        if (file.Contains("BetterConfigs") && file.Contains("-new"))
        {
            File.Copy(file, split.Replace($"/{fileName}", "") + "MGHBetterConfigs" + fileEnd, true);
        }

        curFile = split.Replace($"/{fileName}", "");
        break;
    }

    for (int i = 0; i < args.Length; i++)
    {
        string arg = args[i];
        if (arg == "--updateNew")
        {
            args[i] = "--updateRestart";
            break;
        }
    }
    
    Process.Start(curFile, args);
    Environment.Exit(0);
}

    if (args.Any(x => x == "--updateRestart"))
    {
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
}
catch (Exception e)
{
    if (Config.Singleton.Debug)
    {
        Console.WriteLine(e);
    }
}

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