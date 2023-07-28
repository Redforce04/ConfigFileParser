// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         ConfigParser.cs
//    Author:           Redforce04#4091
//    Revision Date:    07/25/2023 11:33 PM
//    Created Date:     07/25/2023 11:33 PM
// -----------------------------------------

using System.Collections;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using ConfigFileParser.Components;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConfigFileParser.Configs;

public class ConfigParser
{
    public static ConfigParser Singleton;
    public ConfigParser()
    {
        Singleton = this;
    }

    public void RunInitializationScript()
    {
        MGHConfig conf = new MGHConfig();
        SerializableConfig serializableConfig = new SerializableConfig();

        var fields = conf.GetType().GetRuntimeFields();
        TextInfo info = new TextInfo();
        info.TotalConfigNum = fields.Count();
        int itemCount = 0;
        foreach (var item in fields)
        {
            itemCount++;
            info.CurrentConfigNum = itemCount;
            string baseType = item.FieldType.Name.Replace("`1", "").Replace("`2", "");
            string subType1 = item.FieldType.GenericTypeArguments.Count() >= 1
                ? item.FieldType.GenericTypeArguments[0].Name
                : "";
            string subType2 = item.FieldType.GenericTypeArguments.Count() >= 2
                ? item.FieldType.GenericTypeArguments[1].Name
                : "";


            info.ConfigType = baseType + (subType1 == "" ? "" : "[" + subType1) +
                              (subType2 == "" ? "" : $", {subType2}") + (subType1 != "" ? "]" : "");
            NameAttribute? name = item.GetCustomAttribute<NameAttribute>();
            DescriptionAttribute? description = item.GetCustomAttribute<DescriptionAttribute>();
            DefaultValueAttribute? defaultValue = item.GetCustomAttribute<DefaultValueAttribute>();
            ConfigNameAttribute? fullName = item.GetCustomAttribute<ConfigNameAttribute>();
            if (description is not null)
            {
                info.Description = description.Description;
            }

            if (name is not null)
            {
                info.ConfigName = name.Name;
            }

            if (fullName is not null)
            {
                info.ConfigFullName = fullName.ConfigName;
            }
            else
            {
                info.ConfigFullName = info.ConfigName;
            }

            if (defaultValue is not null)
            {
                info.DefaultValue = defaultValue.DefaultValue;
            }
            else
            {
                info.DefaultValue = (item.GetValue(conf) is null) ? "N/A" : item.GetValue(conf)!.ToString()!;
                // if(Config.Singleton.Debug) Console.WriteLine("");
            }

            ConfigItem confItem = new ConfigItem()
            {
                Description = info.Description,
                DefaultValue = info.DefaultValue,
                Name = info.ConfigName,
                ConfigName = info.ConfigFullName,
                Type = info.ConfigType,
                InternalFieldName = item.Name,
                Value = null
            };
            var jsonParser = item.GetCustomAttribute<JsonParserAttribute>();
            if (jsonParser is not null)
            {
                if(Config.Singleton.Debug) Console.WriteLine($"Json Parsing");
                info.Instruction = JsonParserAttribute.Instruction;
                var result = jsonParser.Parse(out bool skip, ref info, item.FieldType);
                if (!skip)
                {
                    item.SetValue(conf, Convert.ChangeType(result, item.FieldType));
                }

                confItem.Value = item.GetValue(conf);
                serializableConfig.Items.Add(confItem);

                continue;
            }

            var parser = item.GetCustomAttribute<CustomParserAttribute>();
            if (parser is null)
            {
                continue;
            }
            if(Config.Singleton.Debug) Console.WriteLine($"Normal Parsing");

            info.Instruction = CustomParserAttribute.Instruction;

            var parserResult = parser.Parse(out bool skipParser, ref info, item.FieldType);
            if (!skipParser)
            {
                item.SetValue(conf, Convert.ChangeType(parserResult, item.FieldType));
            }

            confItem.Value = item.GetValue(conf);
            serializableConfig.Items.Add(confItem);
        }

        GenerateInputFile(serializableConfig);
        GenerateOutputFile(conf);
        info = new TextInfo();
        info.Description = $"Previewing Generated {Config.Singleton.OutputParserType} file.";

        CustomTextParser.Singleton.PrintConfigSummary(info);

        
    }
    public void GenerateInputFile(SerializableConfig config)
    {
        string result = "";
        switch (Config.Singleton.InputParserType)
        {
            case ParserType.Json:
                result = Newtonsoft.Json.JsonConvert.SerializeObject(config);
                break;
            default:
                CustomTextParser.Singleton.PrintLine($"<Warn>Input parser {Config.Singleton.InputParserType} is not currently supported.");
                Thread.Sleep(4000);
                break;
        }

        try
        {
            File.WriteAllText(Config.Singleton.InputFileLoc, result, Encoding.Default);
        }
        catch (AccessViolationException)
        {
            CustomTextParser.Singleton.PrintLine(
                $"<Warn>Could not write configs to file '{Config.Singleton.InputFileLoc}'. (Access Violation Exception)" +
                $"\nEnsure you have permissions to write here, and that no programs are open that may be using this file.");
            Thread.Sleep(6000);
        }
        catch (UnauthorizedAccessException)
        {
            CustomTextParser.Singleton.PrintLine(
                $"<Warn>Could not write configs to file '{Config.Singleton.InputFileLoc}'. (Unauthorized Access Exception)" +
                $"\nEnsure you have permissions to write here, and that no programs are open that may be using this file.");
            Thread.Sleep(6000);
        }
        catch (Exception e)
        {
            CustomTextParser.Singleton.PrintLine($"<Warn>Could not write configs to file '{Config.Singleton.InputFileLoc}'. (Exception)");
            if (Config.Singleton.Debug) Console.WriteLine($"{e}");
            Thread.Sleep(5000);
        }
        
    }

    public void RegenerateOutput()
    {
        string input = "";
        try
        {
            input = File.ReadAllText(Config.Singleton.InputFileLoc);
        }
        catch (AccessViolationException)
        {
            CustomTextParser.Singleton.PrintLine(
                $"<Warn>Could not read configs from file '{Config.Singleton.InputFileLoc}'. (Access Violation Exception)" +
                $"\nEnsure you have permissions to read here, and that no programs are open that may be using this file.");
            Thread.Sleep(6000);
        }
        catch (UnauthorizedAccessException)
        {
            CustomTextParser.Singleton.PrintLine(
                $"<Warn>Could not read configs from file '{Config.Singleton.InputFileLoc}'. (Unauthorized Access Exception)" +
                $"\nEnsure you have permissions to read here, and that no programs are open that may be using this file.");
            Thread.Sleep(6000);
        }
        catch (Exception e)
        {
            CustomTextParser.Singleton.PrintLine(
                $"<Warn>Could not read configs from file '{Config.Singleton.InputFileLoc}'. (Exception)");
            Thread.Sleep(4000);
            if (Config.Singleton.Debug) Console.WriteLine($"{e}");
        }

        SerializableConfig? config = null;
        switch (Config.Singleton.InputParserType)
        {
            case ParserType.Json:

                try
                {
                    config = JsonConvert.DeserializeObject<SerializableConfig>(input);
                }
                catch (Exception e)
                {
                    CustomTextParser.Singleton.PrintLine("<Warn>Input file is corrupted or invalid.");
                    Thread.Sleep(3000);
                    if (Config.Singleton.Debug) Console.WriteLine($"{e}");
                }

                break;
            default:
                CustomTextParser.Singleton.PrintLine(
                    "<Warn>The input parsing method selected is not currently supported.");
                Thread.Sleep(4000);
                break;
        }

        if (config is null)
        {
            CustomTextParser.Singleton.PrintLine(
                "<Warn>Input file is corrupted or invalid. Cannot parse the input. + " +
                "\n<Secondary>Would you like to delete the old input config file? \n <Warn>Yes <Secondary>or No?");
            string? readLine = Console.ReadLine();
            if (readLine is null || readLine == "" || readLine.ToLower() != "yes" || readLine.ToLower() == "y")
            {
                return;
            }

            try
            {
                CustomTextParser.Singleton.PrintLine("<Primary>Deleting old config.");
                Thread.Sleep(3000);
                File.Delete(Config.Singleton.InputFileLoc);
            }
            catch (Exception e)
            {
                CustomTextParser.Singleton.PrintLine(
                    "<Warn>Could not delete the input file. Ensure you have permissions to write to this file, and ensure it is not in use.");
                if (Config.Singleton.Debug) Console.WriteLine(e);
            }

            return;
        }

        MGHConfig conf = new MGHConfig();
        var fields = conf.GetType().GetFields();
        List<string> ImplementedFields = new List<string>();
        List<string> OldFields = new List<string>();
        List<string> NewFields = new List<string>();

        foreach (var confItem in config.Items)
        {
            ImplementedFields.Add(confItem.InternalFieldName);
            var field = fields.FirstOrDefault(x => x.Name == confItem.InternalFieldName);

            if (field is null)
            {
                OldFields.Add(confItem.InternalFieldName);
                continue;
            }


            object? newValue = null;
            if (confItem.Value is JContainer)
            {
                newValue = ((JContainer)confItem.Value).ToObject(field.FieldType);
            }
            else
            {
                newValue = Convert.ChangeType(confItem.Value, field.FieldType);
            }
            
            
            try
            {
                field.SetValue(conf, newValue);
            }
            catch (Exception e)
            {
                if (Config.Singleton.Debug)
                {
                    Console.WriteLine($"{e}");
                }
            }
        }

        foreach (FieldInfo field in fields)
        {
            if (!ImplementedFields.Contains(field.Name))
            {
                NewFields.Add(field.Name);
            }
        }

        string info =
            $"<Warn>" +
            $"{(NewFields.Count > 0 ? $"{NewFields.Count} new config option{(NewFields.Count == 1 ? "" : "s")} {(NewFields.Count == 1 ? "has" : "have")} been added" : "")} " +
            $"{(NewFields.Count > 0 && OldFields.Count > 0 ? "and" : "to")}" +
            $" {(OldFields.Count > 0 ? $"{OldFields.Count} config option{(NewFields.Count == 1 ? "" : "s")} {(NewFields.Count == 1 ? "has" : "have")} been removed from " : "")}" +
            $" MGH since you last configured mgh with this tool.\n";
        
        if (NewFields.Count > 0)
        {
            string newConfs = "";
            foreach (string newConf in NewFields)
            {
                newConfs += $"<DarkGreen>+{newConf}<Secondary>, ";
            }
            //info += $"<Warn>{NewFields.Count} new config option{(NewFields.Count == 1 ? "" : "s")} {(NewFields.Count == 1 ? "has" : "have")} been added to MGH since you last configured mgh with this tool.\n";
            info += $"<Primary>New Configs: <Secondary>[{newConfs},<Secondary>]\n".Replace(", ,", "");
        }

        if (OldFields.Count > 0)
        {
            string oldConfs = "";
            foreach (string oldConf in OldFields)
            {
                oldConfs += $"<DarkRed>-{oldConf}<Secondary>, ";
            }
            //info += $"<Warn>{OldFields.Count} config option{(NewFields.Count == 1 ? "" : "s")} {(NewFields.Count == 1 ? "has" : "have")} been removed from MGH since you last configured mgh with this tool.\n";
            info += $"<Primary>Old Configs: <Secondary>[{oldConfs},<Secondary>]\n".Replace(", ,", "");

        }

        
        foreach (string oldConf in OldFields)
        {
            
        }
        if (OldFields.Count == 0 && NewFields.Count == 0)
        {
            CustomTextParser.Singleton.PrintLine("<Primary>All configs up to date.");
            Thread.Sleep(2000);
        }
        else
        {
            
            //info += $"<Primary>Would you like to {(OldFields.Count == 0 ? "" : $"remove old configs {(NewFields.Count == 0 ? "" : "and ")}")}{(OldFields.Count == 0 ? "" : "configure new configs")}? (Yes / No)";
            info += "<Primary>Would you like to reconfigure server configs, and overwrite old configs? (Yes / No)";
            CustomTextParser.Singleton.PrintLine(info);
            string? lineread = Console.ReadLine();
            if (lineread is null || lineread == "" || lineread.ToLower() == "no" || lineread.ToLower() == "n")
            {
                // skip   
            }
            else
            {
                RunInitializationScript();
                return;
                /*TextInfo txtinfo = new TextInfo();
                if (OldFields.Count > 0)
                {
                    CustomTextParser.Singleton.PrintLine("Not Implemented");
                    Console.Read();
                }

                if (NewFields.Count > 0)
                {
                    CustomTextParser.Singleton.PrintLine("Not Implemented.");
                    Console.Read();
                }*/

            }

        }

        GenerateOutputFile(conf);
    }
    private void GenerateOutputFile(MGHConfig conf)
    {
        
        switch (Config.Singleton.OutputParserType)
        {
            case (ParserType.Ini):
                WriteMGHIni(conf);
                break;
            default:
                CustomTextParser.Singleton.PrintLine($"<Warn>This output parser is not implemented.");
                break;
        }
    }

    private void WriteMGHIni(MGHConfig conf)
    {
        CustomTextParser.Singleton.PrintLine("Outputting the config options that will be written to the output file.");
        string output = "";
        var fields = conf.GetType().GetFields();
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<ConfigNameAttribute>() is null)
            {
                continue;
            }
            string name = field.GetCustomAttribute<ConfigNameAttribute>()!.ConfigName;
            if (field.FieldType.IsGenericList())
            {
                var list = field.GetValue(conf);
                if (list is null)
                {
                    continue;
                }
                if (((IList)list).Count == 0)
                {
                    continue;
                }

                Type type = field.FieldType.GenericTypeArguments[0];
                string listEntries = "";
                foreach (object item in (IList)list)
                {
                    // Console.WriteLine("List entry");
                    object key = Convert.ChangeType(item, type);
                    listEntries += $",{key}";
                }
                output += ($"{name}=," + listEntries).Replace(",,", "") + "\n";
                // Console.WriteLine("List");
            }
            else if (field.FieldType.IsGenericDict())
            {
                string entries = "";
                Type type1 = field.FieldType.GenericTypeArguments[0];
                Type type2 = field.FieldType.GenericTypeArguments[1];
                var dict = field.GetValue(conf);
                if (dict is null)
                {
                    continue;
                }
                if (((IDictionary)dict).Count == 0)
                {
                    continue;
                }
                foreach (DictionaryEntry kvp in (IDictionary)dict)
                {
                    // Console.WriteLine("Dict entry");
                    var key = Convert.ChangeType(kvp.Key, type1);
                    var value = Convert.ChangeType(kvp.Value, type2);
                    entries += $",({key},{value})";
                }
                output += ($"{name}=(," + entries + ")").Replace(",,", "") + "\n";

                // Console.WriteLine("Dict");
            }
            else
            {
                var value = field.GetValue(conf);
                output += $"{name}={value}\n";
                //Console.WriteLine(field.FieldType.Name);  
            }
        }
        
        Console.WriteLine(output);
        Thread.Sleep(5000);
        CustomTextParser.Singleton.PrintLine($"Writing Output File.");
        //Console.Read();
        Thread.Sleep(3000);
        try
        {
            File.WriteAllText(Config.Singleton.OutputFileLoc, output);
        }
        catch (Exception e)
        {
            CustomTextParser.Singleton.PrintLine(
                "<Warn>Could not delete the input file. Ensure you have permissions to write to this file, and ensure it is not in use.");
            if(Config.Singleton.Debug) Console.WriteLine(e);
        }
    }

}
public static class Extensions
{
    public static bool IsGenericList(this Type o)
    {
        return (o.IsGenericType && (o.GetGenericTypeDefinition() == typeof(List<>)));
    }
    public static bool IsGenericDict(this Type o)
    {
        return (o.IsGenericType && (o.GetGenericTypeDefinition() == typeof(Dictionary<,>)));
    }
}


/*
MGH BetterConfigs
Hunter Movement Speed Multiplier (Integer) - Config [3 / 19]
-- Type value or leave blank for default value, then press enter to submit. --
What should the speed multiplier be for ghosts? (default: 1.0)  
*/