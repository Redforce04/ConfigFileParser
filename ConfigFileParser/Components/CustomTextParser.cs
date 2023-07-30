// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         CustomTextParser.cs
//    Author:           Redforce04#4091
//    Revision Date:    07/26/2023 2:18 AM
//    Created Date:     07/26/2023 2:18 AM
// -----------------------------------------

using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using ConfigFileParser.Configs;

namespace ConfigFileParser.Components;

public class CustomTextParser
{
    public static CustomTextParser Singleton;
    private static string Billboard = @"<Accent>  __  __  _____ _    _   ____       _   _             _____             __ _           " + "\n" + @" |  \/  |/ ____| |  | | |  _ \     | | | |           / ____|           / _(_)           " + "\n" + @" | \  / | |  __| |__| | | |_) | ___| |_| |_ ___ _ __| |     ___  _ __ | |_ _  __ _ ___  " + "\n" + @" | |\/| | | |_ |  __  | |  _ < / _ | __| __/ _ | '__| |    / _ \| '_ \|  _| |/ _` / __| " + "\n" + @" | |  | | |__| | |  | | | |_) |  __| |_| ||  __| |  | |___| (_) | | | | | | | (_| \__ \ " + "\n" + @" |_|  |_|\_____|_|  |_| |____/ \___|\__|\__\___|_|   \_____\___/|_| |_|_| |_|\__, |___/ " + "\n" + @"                                                                              __/ |     " + "\n" + @"                                                                             |___/      ";
    private static string Banner = @"<SecondaryAccent>MGH BetterConfigs - by Redforce04"; 

    public CustomTextParser()
    {
        Singleton = this;
    }

    public void PrintLine(string line)
    {
        Console.Clear();
        if (!Config.Singleton.Silent)
        {
            Print(Billboard);
            Print(Banner.PadLeft((Billboard.Split("\n")[0].Length + Banner.Length) / 2) + "\n");
        }

        Print(line);
    }
    public void PrintCustomInput(TextInfo info)
    {
        // 88
        // 88 - text.count /2

        Console.Clear();
        if (!Config.Singleton.Silent)
        {
            Print(Billboard);
            Print(Banner.PadLeft((Billboard.Split("\n")[0].Length + Banner.Length) / 2) + "\n");
        }

        Print($"<Primary>Current Config: <Accent>{info.ConfigName} <Primary>({info.ConfigType}) - Config [<Accent>{info.CurrentConfigNum} <Primary>/ {info.TotalConfigNum}]");
        Print($"<Primary>{info.Description} (default: <Accent>{info.DefaultValue}<Primary>)");

        foreach (string instructionLine in info.Instruction.Split('\n'))
        {
            if(instructionLine != "")
                Print($"<Secondary>-- {instructionLine} --");
        }
        foreach (string errorLine in info.ErrorString.Split('\n'))
        {
            Print($"{errorLine}");
        }
        info.ErrorString = "";
        foreach(string str in info.CustomStrings)
        {
            Print(str);
        }

        info.CustomStrings = new List<string>();
    }

    public void PrintConfigSummary(TextInfo info)
    {
        Console.Clear();
        if (!Config.Singleton.Silent)
        {
            Print(Billboard);
            Print(Banner.PadLeft((Billboard.Split("\n")[0].Length + Banner.Length) / 2) + "\n");
        }

        Print($"{info.Description}");
        /*foreach (string instructionLine in info.Instruction.Split('\n'))
        {
            Print($"<Secondary>-- {instructionLine} --");
        }*/

        info.ErrorString = "";
        foreach(string str in info.CustomStrings)
        {
            Print(str);
        }
    }
    internal void Print(string text)
    {
        string newText = text;
        List<KeyValuePair<int, ConsoleColor>> Lines = new List<KeyValuePair<int, ConsoleColor>>();
        Lines.Add(new KeyValuePair<int, ConsoleColor>(0, ConsoleColor.White));
        string colors = "|";
        foreach (ConsoleColor en in Enum.GetValuesAsUnderlyingType<ConsoleColor>())
        {
            colors += $"|{en.ToString()}";
        }

        colors = colors.Replace("||", "");
        colors += "|Primary|Accent|SecondaryAccent|Secondary|Warn|Error";
        var regex = new Regex(@"<(" + colors + @".*?)>");
        var matches = regex.Matches(text);
        int displacement = 0;
        if (matches.Count < 1)
        {
            Console.WriteLine(text);
            return;
        }
        foreach (Match match in matches)
        {
            if (match.Groups.Count < 2)
            {
                //if(Config.Singleton.Debug) Console.WriteLine($"No group found.");
                continue;
            }
            string val = newText.Substring(0, match.Index - displacement);

            ConsoleColor color = ActiveColorScheme["Primary"];
            try
            {
                color = Enum.Parse<ConsoleColor>(match.Groups[1].Value, true);
            }
            catch (ArgumentException)
            {
                if (ConsoleColorScheme.ContainsKey(match.Groups[1].Value))
                {
                    color = ConsoleColorScheme[match.Groups[1].Value];
                }
                else
                {
                    continue;
                }
            }
            catch (Exception e)
            {
                if(Config.Singleton.Debug) Console.WriteLine($"{e}");
                continue;
            }

            newText = newText.Remove(match.Index-displacement, match.Length);
            //newText = text.Substring(match.Index + match.Length, (newText.Length - (match.Index + match.Length)));
            Lines.Add(new KeyValuePair<int, ConsoleColor>(match.Index - displacement, color));
            displacement += match.Length;
            //Console.WriteLine($"{color}");
        }

        for (int i = 0; i < Lines.Count; i++)
        {
            KeyValuePair<int, ConsoleColor> pair = Lines[i];
            int length = 0;
            if (i + 1 < Lines.Count)
            {
                length = Lines[i + 1].Key - pair.Key;
            }
            else
            {
                length = newText.Length - pair.Key;
            }

            string line = newText.Substring(pair.Key, length);
            Console.ForegroundColor = pair.Value;
            Console.Write(line);
        }
        Console.WriteLine();
        Console.ForegroundColor = ActiveColorScheme["Primary"];;
    }

    public static Dictionary<string, ConsoleColor> ActiveColorScheme => Config.ExportedColorScheme switch
    {
        ColorSchemes.Console => ConsoleColorScheme,
        ColorSchemes.Linux => LinuxColorScheme,
        ColorSchemes.Pterodactyl => PterodactylColorScheme,
        _ => ConsoleColorScheme
    };

    public static Dictionary<string, ConsoleColor> ConsoleColorScheme = new Dictionary<string, ConsoleColor>()
    {
        { "Primary", ConsoleColor.Gray },
        { "Secondary", ConsoleColor.DarkGray },
        { "Accent", ConsoleColor.DarkBlue },
        { "SecondaryAccent", ConsoleColor.DarkMagenta },
        { "Warn", ConsoleColor.Red },
        { "Error", ConsoleColor.Red },
    };
    public static Dictionary<string, ConsoleColor> PterodactylColorScheme = new Dictionary<string, ConsoleColor>()
    {
        { "Primary", ConsoleColor.White },
        { "Secondary", ConsoleColor.Gray },
        { "SecondaryAccent", ConsoleColor.DarkRed },
        { "Accent", ConsoleColor.Cyan },
        { "Warn", ConsoleColor.DarkRed },
        { "Error", ConsoleColor.Red },
    };
    public static Dictionary<string, ConsoleColor> LinuxColorScheme = new Dictionary<string, ConsoleColor>()
    {
        { "Primary", ConsoleColor.White },
        { "Secondary", ConsoleColor.Gray },
        { "SecondaryAccent", ConsoleColor.DarkRed },
        { "Accent", ConsoleColor.Cyan },
        { "Warn", ConsoleColor.DarkRed },
        { "Error", ConsoleColor.Red },
    };
}

public class TextInfo
{
    public string ConfigName = "";
    public string ConfigFullName = "";
    public string ConfigType = "";
    public int CurrentConfigNum = 0;
    public int TotalConfigNum = 0;
    public int CurrentTry = 0;
    public string Instruction = "";
    public string Description = "";
    public string DefaultValue = "";
    public string ErrorString = "";
    public List<string> CustomStrings = new List<string>();
}

/*
 MGH BetterConfigs
 Hunter Movement Speed Multiplier (Integer) - Config [3 / 19]
 -- Type value or leave blank for default value, then press enter to submit. --
 What should the speed multiplier be for ghosts? (default: 1.0)  
 */