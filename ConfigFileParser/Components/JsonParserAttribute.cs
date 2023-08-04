// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         JsonParserAttribute.cs
//    Author:           Redforce04#4091
//    Revision Date:    07/26/2023 1:23 AM
//    Created Date:     07/26/2023 1:23 AM
// -----------------------------------------

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Formats.Tar;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using ConfigFileParser.Configs;
using Microsoft.VisualBasic.FileIO;

namespace ConfigFileParser.Components;

/* Steps: 
    1. Print Out Config Info
    2. Listen for commands.
    3. check if command
    4. respond to command
    5. add to list/dict or dont
    5. prep next response
*/



[AttributeUsage(AttributeTargets.All)]
public class JsonParserAttribute : CustomParserAttribute
{
    public override object? Parse(out bool skip, ref TextInfo info, Type type)
    {
        
        bool showInvalidWarning = false;
        PreviousAction previousAction = PreviousAction.None;
        object obj = type.GetConstructor(Type.EmptyTypes)!.Invoke(new object[] { });
        skip = false;
        bool firstTry = true;
        bool isList = type.IsGenericList();
        bool isDict = type.IsGenericDict();
        if (!isDict && !isList)
        {
            CustomTextParser.Singleton.PrintLine("<Warn>Warning: Custom list parser is intended for dictionaries / lists. " +
                                                 "\nIt is being used to parse an object that is not a dictionary / list. " +
                                                 "\nThis config will be skipped, and the default will be used.");
            skip = true;
            return null;
        }

        if (isDict)
        {
            var instructions = Instruction.Split('\n');
            instructions[0] =
                "<Secondary>For each dictionary entry, separate the key and value with an <White>'='<Secondary>. Press enter to submit that entry";
            info.Instruction = "";
            foreach (string x in instructions)
            {
                info.Instruction += $"\n{x}";
            }
        }
        string? lastEntry = "";
        int failedTries = 0;
        while (failedTries < 3)
        {
            if (previousAction == PreviousAction.Skip || previousAction == PreviousAction.Finish)
            {
                break;
            }

            if (previousAction == PreviousAction.List)
            {
                if (!type.GenericTypeArguments[0].IsEnum)
                {
                    info.ErrorString += ("<Warn>Error: Cannot list the possible entries because the object is not an enum.\n");
                }
                else
                {
                    string[] enumNames = Enum.GetNames(type.GenericTypeArguments[0]);
                    if (enumNames.Length == 0)
                    {
                        info.CustomStrings.Add($"<Primary>Possible Option: <Secondary>[<SecondaryAccent>None<Secondary>]");
                    }
                    else
                    {

                        string options = ",";
                        foreach (string Enum in Enum.GetNames(type.GenericTypeArguments[0]))
                        {
                            options += $", {Enum}";
                        }

                        info.CustomStrings.Add($"<Primary>Possible Options: <Secondary>[<SecondaryAccent>{options.Replace(",, ", "")}<Secondary>]");
                    }

                }
            }
            string currentListItems = "<Secondary>,";
            if (isList)
            {
                foreach (object listObj in ((IList)obj))
                {
                    currentListItems += $", {listObj}";
                }
            }
            else if (isDict)
            {
                foreach (DictionaryEntry dictObj in ((IDictionary)obj))
                {
                    currentListItems += $", <Accent>{dictObj.Key}<Secondary> [<SecondaryAccent>{dictObj.Value}<Secondary>]";
                }
            }

            if (((ICollection)obj).Count == 0)
            {
                currentListItems = currentListItems.Remove(currentListItems.Length - 1, 1) + "<Accent>No Items";
            }
            currentListItems = currentListItems.Replace(",, ", "");
            
            info.CustomStrings.Add($"<Primary>Current values: <Secondary>[<Accent>{currentListItems}<Secondary>]");
            

            if (showInvalidWarning)
            {
                showInvalidWarning = false;
                if (Config.Singleton.Debug)
                {
                    Console.WriteLine("Last action skipping warning.");
                }
                info.ErrorString += $"<Warn>Could not process the last entry. Skipping in {3 - failedTries} more failed {(failedTries > 1 ? "try" : "tries")}. [Try <Warn>{failedTries} <Secondary>/ 3]\n";
            }
            else
            {
                failedTries = 0;
            }
            
            CustomTextParser.Singleton.PrintCustomInput(info);
            var input = Console.ReadLine();
            if (input is null or "")
            {
                if (firstTry)
                {
                    showInvalidWarning = false;
                    previousAction = PreviousAction.Skip;
                    skip = true;
                    break;
                }
                else
                {
                    info.ErrorString += $"<Warn>Type <White>'finish'<Warn> or <White>'skip'<Warn> when done. Blank entries only skip on the first input.\n";
                }
            }
            if (firstTry)
            {
                firstTry = false;
            }
            lastEntry = input;

            bool finished = false;
            switch (input.Split(" ")[0].ToLower())
            {
                case "skip":
                    previousAction = PreviousAction.Skip;
                    CustomTextParser.Singleton.Print("\n<Primary> Skipping config. Default config will be used.");
                    Thread.Sleep(2000);
                    break;
                case "finish":
                    previousAction = PreviousAction.Finish;
                    CustomTextParser.Singleton.Print("\n<Primary>Config finished. Moving on to next config.");
                    Thread.Sleep(2000);
                    continue;
                case "list":
                    previousAction = PreviousAction.List;
                    continue;
                case "remove":
                    showInvalidWarning = true;
                    previousAction = PreviousAction.Remove;
                    string itemToRemove = "";
                    bool initial = true;
                    foreach (string arg in input.Split(' '))
                    {
                        if (!initial)
                        {
                            itemToRemove += $"{arg} ";
                        }
                        else
                            initial = false;
                    }
                    object? argValue = ProcessArg(itemToRemove, type.GenericTypeArguments[0]);
                    if (argValue is null)
                    {
                        failedTries++;
                        info.ErrorString += $"<Warn>Could not remove the item due to parsing. Could not parse item <White>'{itemToRemove}'\n";
                        continue;
                    }

                    if (isDict)
                    {
                        if (((IDictionary)obj).Contains(argValue))
                        {
                            ((IDictionary)obj).Remove(argValue);
                        }
                        else
                        {
                            failedTries++;
                            info.ErrorString += $"<Warn>Item <White>'{argValue}'<Warn> was not found in the dictionary.\n";
                            continue;
                        }
                    }
                    else if (isList)
                    {
                        if (((IList)obj).Contains(argValue))
                        {
                            ((IList)obj).Remove(argValue);
                        }
                        else
                        {
                            failedTries++;
                            info.ErrorString += $"<Warn>Item <White>'{argValue}'<Warn> was not found in the list.\n";
                            continue;
                        }
                    }

                    showInvalidWarning = false;
                    break;
                default:
                    showInvalidWarning = true;
                    previousAction = PreviousAction.Add;
                    string itemToAdd = "";
                    foreach (string arg in input.Split(' '))
                    {
                        itemToAdd += $"{arg} ";
                    }

                    string? arg2 = null;
                    var split = itemToAdd.Split('=');
                    if (isDict && split.Length > 1)
                    {
                        itemToAdd = split[0];
                        arg2 = split[1];
                    }
                    else if (isDict && split.Length <= 1)
                    {
                        info.ErrorString +=
                            "<Warn>You entered an object, without it's value. \n" +
                            "<Warn>Dictionaries require an object and value separated by an <White>'='<Warn>.\n" +
                            "<Warn>Ex: <White>'C4=0.5' <Warn> or <White> 'Trap=1.5'<Warn>.\n";
                        failedTries++;
                        continue;
                    }
                    
                    object? arg1Value = ProcessArg(itemToAdd, type.GenericTypeArguments[0]);
                    object? arg2Value = null;
                    if (arg2 is not null)
                    {
                        arg2Value = ProcessArg(arg2, type.GenericTypeArguments[1]);
                        if (arg2Value is null)
                        {
                            showInvalidWarning = true;
                            char starter = type.GenericTypeArguments[1].Name[0];
                            info.ErrorString += $"<Warn>Could not parse value <White>'{arg2}'<Warn>. Ensure it is a{(starter is 'a' or 'e' or 'i' or 'o' or 'u' ? "n" : "" )} {type.GenericTypeArguments[1]}.\n";
                            failedTries++;
                            continue;
                        }
                    }
                    if (arg1Value is null || (isDict && arg2Value is null))
                    {
                        showInvalidWarning = true;
                        char starter = type.GenericTypeArguments[0].Name[0];
                        info.ErrorString += $"<Warn>Could not parse key <White>'{itemToAdd}'<Warn>. Ensure it is a{(starter is 'a' or 'e' or 'i' or 'o' or 'u' ? "n" : "" )} {type.GenericTypeArguments[0]}.\n";
                        failedTries++;
                        continue;
                    }


                    if (isDict)
                    {
                        if (((IDictionary)obj).Contains(arg1Value))
                        {
                            ((IDictionary)obj)[arg1Value] = arg2Value;
                        }
                        else
                        {
                            ((IDictionary)obj).Add(arg1Value, arg2Value);
                        }
                    }
                    else
                    {
                        if (!((IList)obj).Contains(arg1Value))
                        {
                            ((IList)obj).Add(arg1Value);
                        }
                    }

                    showInvalidWarning = false;
                    break;
            }

        }

        if (previousAction == PreviousAction.Skip)
        {
            skip = true;
            return null;
        }
        
        return obj;
    }

    enum PreviousAction
    {
        None,
        Add,
        Remove,
        List,
        Skip,
        Finish
    }

    public object? ProcessArg(string arg, Type type)
    {
        try
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, arg, true);
            }

            if (type.GetInterface(nameof(IConvertible)) is not null)
            {
                return Convert.ChangeType(arg, type);
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject(arg, type);
        }
        catch (Exception e)
        {
            if (Config.Singleton.Debug)
            {
                Console.WriteLine(e);
            }
        }

        return null;
    }

    /*
    public object? Parse2(out bool skip, ref TextInfo info, Type type)
    {
        
        object obj = type.GetConstructor(Type.EmptyTypes)!.Invoke(new object[]{});
        bool isList = type.IsGenericList();
        if (!isList)
        {
            info.Instruction += "\nSeparate the key and value with a <White>'='<Secondary>. Ex:'<Accent>C4=1.0<Secondary>'.";
        }
        skip = false;
        int tries = 0;
        bool accepted = false;
        string? previousInput = "";
        string enums = "";
        while (!accepted && tries < 3)
        {
            string txt = "<Secondary>Current Items: ";
            if (type.IsGenericList())
            {
                foreach (object entry in ((IList)obj))
                {
                    txt += $" {entry}";
                }
            }
            else
            {
                foreach (DictionaryEntry entry in ((IDictionary)obj))
                {
                    txt += $" {entry.Key}";
                }
                
            }
            
            if (info.CustomStrings.Count < 1)
            {
                info.CustomStrings.Add(txt);
            }
            else
            {
                info.CustomStrings[0] = txt;
            }
            tries++;
            if (enums != "")
            {
                info.CustomStrings.Add(enums);
            }
            if (tries > 1 && enums != "")
            {
                string errorText = $"<Warn>Value \'{previousInput}\' is invalid for this field. Please re-enter the selected value. <Secondary>[Try <Warn>{tries} <Secondary>/ 3]";
                info.ErrorString = errorText;
            }

            enums = "";
            
            info.CurrentTry = tries;
            CustomTextParser.Singleton.PrintCustomInput(info);
            string? input = Console.ReadLine();
            if (input is null || input == "")
            {
                Console.WriteLine("Skipping config.");
                skip = true;
                return null;
            }

            switch (input.ToLower().Split(" ")[0])
            {
                case "list":
                    tries--;
                    Type type1 = type.GenericTypeArguments[0];
                    if (type1.IsEnum)
                    {
                        string totalEnums = "";
                        foreach (string Enum in Enum.GetNames(type1))
                        {
                            totalEnums += $", {Enum}";
                        }
                        enums = ("<Secondary>Available Options: \n," + totalEnums).Replace(",, ", "");

                    }

                    continue;
                    case "remove":
                        var args = input.Split(' ');
                        if (args.Length < 2)
                        {
                            previousInput = "";
                        }

                        try
                        {
                            object? result = null;
                            if (type.GenericTypeArguments[0].IsEnum)
                            {
                                result = typeof(Enum).GetMethod(nameof(Enum.Parse))!
                                    .MakeGenericMethod(type.GenericTypeArguments[0]).Invoke(null, new object[]{ args[1], true });
                                
                            }
                            else
                            {
                                result = Convert.ChangeType(args[1], type.GenericTypeArguments[0]);
                            }

                            if (result is null)
                            {
                                continue;
                            }
                            if (type.IsGenericList())
                            {
                                if(((IList)obj).Contains(result))
                                    ((IList)obj).Remove(result);
                            }
                            else if (type.IsGenericDict())
                            {
                                if(((IDictionary)obj).Contains(result))
                                    ((IDictionary)obj).Remove(result!);
                            }
                        }
                        catch (Exception e)
                        {
                            CustomTextParser.Singleton.Print("\nCould not find value, or parse enum.");
                            if(Config.Singleton.Debug) Console.WriteLine(e);
                        }
                        
                        break;
                    case "finish":
                        return obj;
                    default:
                        args = input.Split(' ');
                        previousInput = input;
                        try
                        {
                            string inpt1 = "";
                            object? value = null;
                            bool dict = type.IsGenericDict();
                            if (dict)
                            {
                                var splt = input.Split("=");
                                inpt1 = splt[0];
                                // splt[1];
                                value = Convert.ChangeType(splt[1], type.GenericTypeArguments[1]);

                            }
                            object? result = null;
                            object? value2 = null;
                            if (type.GenericTypeArguments[0].IsEnum)
                            {
                                result = typeof(Enum).GetMethod(nameof(Enum.Parse))!
                                    .MakeGenericMethod(type.GenericTypeArguments[0]).Invoke(null, new object[]{ dict ? inpt1 : input, true });
                                
                            }
                            else
                            {
                                result = Convert.ChangeType(dict ? inpt1 : input , type.GenericTypeArguments[0]);
                            }
                            
                            
                            if (result is null)
                            {
                                continue;
                            }
                            if (type.IsGenericList())
                            {
                                if(!((IList)obj).Contains(result))
                                    ((IList)obj).Add(result);
                            }
                            else if (type.IsGenericDict())
                            {
                                if(!((IDictionary)obj).Contains(result))
                                    ((IDictionary)obj).Add(result, value);
                            }
                        }
                        catch (Exception e)
                        {
                            CustomTextParser.Singleton.Print("\nCould not find value, or parse enum.");
                            if(Config.Singleton.Debug) Console.WriteLine(e);
                        }
                        break;
            }

            previousInput = input;

            
            //var json = Newtonsoft.Json.JsonConvert.DeserializeObject(input);

            try
            {
            }
            catch (Exception e)
            {
                if (Config.Singleton.Debug)
                {
                    Console.WriteLine(e);
                }
            }
        }
        return obj;
    }
    */
    public static string Instruction = "For each list entry, type the value then press enter to submit that entry." +
                                       "\n<Secondary>Unwanted entries can be removed with: <White>'remove [option]'<Secondary> ex: '<Accent>remove C4<Secondary>' " +
                                       "\nOther Options: <White>'list'<Secondary> - lists options. <White>'finish'<Secondary> - move to the next config." +
                                       "\n               <White>'skip'<Secondary> - use the default value, and move to the next config.";
}
