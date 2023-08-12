// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         SleepManager.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/11/2023 1:22 PM
//    Created Date:     08/11/2023 1:22 PM
// -----------------------------------------

using ConfigFileParser.Configs;

namespace ConfigFileParser;

public class SleepManager
{
    public static void Sleep(int milliseconds)
    {
        if (!Config.Singleton.SuperSilent)
        {
            Thread.Sleep(milliseconds);
        } 
    }
}