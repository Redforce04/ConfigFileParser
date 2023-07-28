// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         CustomParserAttribute.cs
//    Author:           Redforce04#4091
//    Revision Date:    07/26/2023 1:01 AM
//    Created Date:     07/26/2023 1:01 AM
// -----------------------------------------

using System.Diagnostics.CodeAnalysis;
using ConfigFileParser.Configs;

namespace ConfigFileParser.Components;

[AttributeUsage(AttributeTargets.All)]
public class CustomParserAttribute : Attribute
{
    public virtual object? Parse (out bool skip, ref TextInfo textInfo, Type type)
    {
        skip = false;
        int tries = 0;
        bool accepted = false;
        string? previousInput = "";
        while (!accepted && tries < 3)
        {
            tries++;
            if (tries > 1)
            {
                string errorText = $"<Warn>Value \'{previousInput}\' is invalid for this field. Please re-enter the selected value. <Secondary>[Try <Warn>{tries} <Secondary>/ 3]";
                textInfo.ErrorString = errorText;
            }
            
            textInfo.CurrentTry = tries;
            CustomTextParser.Singleton.PrintCustomInput(textInfo);
            string? input = Console.ReadLine();
            if (input is null || input == "")
            {
                Console.WriteLine("Skipping config.");
                break;
            }
            previousInput = input;

            try
            {

                return Convert.ChangeType(input, type);
            }
            catch (FormatException)
            {
                if (tries >= 3)
                {
                    textInfo.ErrorString = "<Warn>Could not determine the desired value. The default config will be used for this value." +
                                           "\n<Warn>If you believe this is an error, please contact redforce04 on discord. " +
                                           "\n== <Secondary>Press enter to continue. ==";
                    CustomTextParser.Singleton.PrintCustomInput(textInfo);
                    Console.Read();
                }
                continue;
            }
            catch (Exception e)
            {
                if(Config.Singleton.Debug) Console.WriteLine($"{e}");
                if (tries >= 3)
                {
                    textInfo.ErrorString = "<Warn>Could not determine the desired value. The default config will be used for this value." +
                                           "\nIf you believe this is an error, please contact redforce04 on discord. " +
                                           "\n== <Secondary>Press enter to continue. ==";
                    CustomTextParser.Singleton.PrintCustomInput(textInfo);
                    Console.Read();
                }
                continue;
            }

            accepted = true;
        }
        skip = true;
        return null;
    }

    public static string Instruction = "Type value or leave blank for default value, then press enter to submit.";
}
/*
 MGH BetterConfigs
 Hunter Movement Speed Multiplier (Integer) - Config [3 / 19]
 -- Type value or leave blank for default value, then press enter to submit. --
 What should the speed multiplier be for ghosts? (default: 1.0)  
 */