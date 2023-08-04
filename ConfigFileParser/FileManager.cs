// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         FileManager.cs
//    Author:           Redforce04#4091
//    Revision Date:    07/25/2023 10:50 PM
//    Created Date:     07/25/2023 10:50 PM
// -----------------------------------------

using System;
using System.IO;
using System.Threading;
using ConfigFileParser.Configs;

namespace ConfigFileParser;

public class FileManager
{
    public static FileManager Singleton;

    public static bool RunInitializerScript = false;
    public FileManager()
    {
        Singleton = this;
        CheckForInitialFile();
    }

    private void CheckForInitialFile()
    {
        if (!File.Exists(Config.Singleton.InputFileLoc))
        {
            Console.WriteLine("No pre-existing configuration files have been found.");
            Thread.Sleep(3000);
            RunInitializerScript = true;
        }

    }
}