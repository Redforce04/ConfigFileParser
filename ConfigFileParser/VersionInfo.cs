// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         VersionInfo.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/02/2023 6:41 PM
//    Created Date:     08/02/2023 6:41 PM
// -----------------------------------------
/* 
    This file is apart of the building system. During building the following will happen:
        - This file overwrites the ArgumentParser.cs
        - The 3 variables notably identified by the "${}" will be replaced.
            - This happens with another program called "ReplaceTextWithVariables" (found in project dir)
            - This helps the program identify git tracking info for the auto-updater. 
        - Project is built
        - Project is published for every architecture 
        - All builds are move to a /bin/Releases/export folder by default.
        
*/
namespace ConfigFileParser;

public class VersionInfo
{
    public const string CommitHash = "4e289f18";
    public const string CommitBranch = "master";
    public const string CommitVersion = "v1.0.3-beta";    
    public const string Architecture = "{BUILD_ARCH}";
    public static DateTime BuildTime = DateTime.Parse("2023-08-11T03:28:20");
}