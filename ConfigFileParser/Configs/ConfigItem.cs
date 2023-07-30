// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         ConfigItem.cs
//    Author:           Redforce04#4091
//    Revision Date:    07/26/2023 1:03 PM
//    Created Date:     07/26/2023 1:03 PM
// -----------------------------------------

using System.Text.Json.Serialization;

namespace ConfigFileParser.Configs;

public class ConfigItem
{
    public string Name { get; set; }
    public string ConfigName { get; set; }
    public string InternalFieldName { get; set; }
    public string Description { get; set; }
    public object?DefaultValue { get; set; }
    public object? Value { get; set; }
    public string Type { get; set; }
}

public class SerializableConfig
{
    public static SerializableConfig Latest;
    public List<ConfigItem> Items = new List<ConfigItem>();
}