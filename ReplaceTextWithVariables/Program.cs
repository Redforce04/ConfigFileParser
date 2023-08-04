// See https://aka.ms/new-console-template for more information
// dotnet publish -r win-x64 -c Release

using System.Collections;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;

public static class Program
{
    public static void Main(string[] args)
    {
        bool ReplaceEnvironmentalVariables = true;
        Dictionary<string, string> replacementTerms = new Dictionary<string, string>();
        string fileLoc = "";
        List<string> EnvironmentalVariables = new List<string>();

        if (args.Length == 0)
        {
            Console.Error.WriteLine("No file specified. Could not replace variables.");
            return;
        }
        
        for (int i = 0; i < args.Length; i++)
        {
            string argument = args[i];
            //Console.WriteLine($"Argument: {argument}");
            if (argument.StartsWith('-'))
            {
                List<string> nextArguments = new List<string>();
                switch (argument)
                {
                    case "-d" or "--datetime":
                        if (args.TryProcessNextArguments(i, 1, out nextArguments))
                        {
                            i += 1;
                            replacementTerms.Add(nextArguments[0], DateTime.UtcNow.ToString("s"));
                        }
                        continue;
                    case "-v" or "--var":
                        //Console.WriteLine($"var");
                        if (args.TryProcessNextArguments(i, 2, out nextArguments))
                        {
                            replacementTerms.Add(nextArguments[0], nextArguments[1]);
                            i += 2;
                            Console.WriteLine($"var: {nextArguments[0]}, {nextArguments[1]}");
                        }
                        continue;

                    case "-ne" or "--no-environmental":
                        //Console.WriteLine($"no env");
                        ReplaceEnvironmentalVariables = false;
                        continue;

                    case "-e" or "--environmental":
                        //Console.WriteLine($"env");
                        if (args.TryProcessNextArguments(i, 1, out nextArguments))
                        {
                            EnvironmentalVariables.Add(nextArguments[0]);
                            i += 1;
                            //Console.WriteLine($"env: {nextArguments[0]}");
                        }
                        continue;
                    
                    case "-cmd" or "--cmd-variable":
                        if (args.TryProcessNextArguments(i, 2, out nextArguments))
                        {
                            i += 2;
                            string procName = nextArguments[0].Split(' ')[0];
                            string procArgs = nextArguments[0].Replace($"{procName} ", "");
                            //procArgs = "";
                            //Console.WriteLine($"starting process {procName}");
                            Process proc = new Process() { StartInfo = new ProcessStartInfo(procName,procArgs)
                            {
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                CreateNoWindow = true
                            }};
                            proc.Start();
                            string output = "";
                            while (!proc.StandardOutput.EndOfStream)
                            {
                                output += proc.StandardOutput.ReadLine();
                            }
                            proc.WaitForExit();
                            //Console.WriteLine($"{output}");
                            replacementTerms.Add($"{nextArguments[1]}", output);
                        }
                        continue;

                    default:
                        Console.Error.WriteLine($"Unknown variable \"{argument}\"");
                        continue;

                }
            }
            else if (File.Exists(args[i]))
            {
                fileLoc = args[i];
                continue;
            }
            else
            {
                Console.Error.WriteLine($"Unknown Argument \"{argument}\"");
                return;
            }
        }

        if (fileLoc == "")
        {
            Console.Error.WriteLine($"File location must be specified.");
            return;
        }
        Dictionary<string, string> varsToReplace = new Dictionary<string, string>();

        if (ReplaceEnvironmentalVariables)
        {
            foreach (DictionaryEntry envVar in Environment.GetEnvironmentVariables())
            {
                //Console.WriteLine($"{(string)envVar.Key}, {(string)envVar.Value!}");
                varsToReplace.Add((string)envVar.Key,(string)envVar.Value!);
            }
        }

        string text = File.ReadAllText(fileLoc);

        foreach (var var in replacementTerms)
        {
            if (text.Contains(var.Key))
                varsToReplace.Add(var.Key, var.Value);
        }

        foreach (var x in varsToReplace)
        {
            text = text.Replace(x.Key, x.Value);
        }

        //Console.WriteLine($"Replacing {varsToReplace.Count + replacementTerms.Count} variables");
        File.WriteAllText(fileLoc, text);
        Console.Write(text);
    }

    private static bool TryProcessNextArguments(this string[] arguments, int currentTerm, int amountToSearch, out List<string> nextArguments)
    {
        nextArguments = new List<string>();
        try
        {
            if (arguments.Length < currentTerm + amountToSearch)
            {
                Console.Error.WriteLine($"Incorrect argument {arguments[currentTerm]}. Proper Usage: \n{ProperUsage(arguments[currentTerm])}");
                return false;
            }
            for (int i = currentTerm + 1; i <= currentTerm + amountToSearch; i++)
            {
                //Console.WriteLine($"{arguments[i]}");
                nextArguments.Add(arguments[i]);
            }
            return true;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Could not process next arguments for var {arguments[currentTerm]}. Error: {e}");
            return false;
        }
    }

    private static string ProperUsage(string var)
    {
        switch (var)
        {
            case "-v" or "--var":
                return "(--var / -v) \"string to replace\" \"replacement value\" - replaces a string with a value";
            
            case "-ne" or "--no-environmental":
                return "(--no-environemental / -ne) - prevents environmental variables from being swapped. (Use -e to add specific variables)";
            
            case "-e" or "--environmental":
                return "(--environmental / -e) \"environmental variable name\" - replaces an environmental variable.";
            case "-cmd" or "--cmd-variable":
                return "(--cmd-variable / -cmd) \"command to execute\" \"variable name\" - replaces an variable with the output of a command.";
            
        }

        return $"Could not find variable {var}";
    }
}