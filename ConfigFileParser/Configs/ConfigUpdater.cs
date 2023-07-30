// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         ConfigUpdater.cs
//    Author:           Redforce04#4091
//    Revision Date:    07/29/2023 12:40 PM
//    Created Date:     07/29/2023 12:40 PM
// -----------------------------------------

using System.Net.Http.Json;
using ConfigFileParser.Components;
using Newtonsoft.Json.Serialization;

namespace ConfigFileParser.Configs;

public class ConfigUpdater
{
    public static ConfigUpdater Singleton;

    public ConfigUpdater()
    {
        SerializableConfig.Latest = new SerializableConfig();
        Singleton = this;
        if (!Config.Singleton.UseCache)
        {
            HttpClient client = new HttpClient();
            var result = client
                .GetAsync(@"https://raw.githubusercontent.com/Redforce04/MGHBetterConfigs/master/LatestConfig.json")
                .Result;
            try
            {
                var updatedConfig = result.Content.ReadFromJsonAsync<SerializableConfig>().Result;
                if (updatedConfig is not null)
                {
                    SerializableConfig.Latest = updatedConfig;
                }
                else
                {
                    CustomTextParser.Singleton.Print(
                        "<Warn>An error has occured while fetching the latest config. Using cache input instead.");
                }
            }
            catch (Exception e)
            {
                CustomTextParser.Singleton.Print(
                    "<Warn>An error has occured while fetching the latest config. Using cache input instead.");
                if (Config.Singleton.Debug)
                {
                    Console.WriteLine($"{e}");
                }
            }
        }

        if (!Config.Singleton.AutoUpdate)
        {
            return;
        }

        HttpClient updateClient = new HttpClient();
        var response = updateClient.GetAsync(@"https://api.github.com/repos/Redforce04/MGHBetterConfigs/releases/latest").Result.Content;
        
        Dictionary<string, string>? results = response.ReadFromJsonAsync<Dictionary<string,string>>().Result;
        if (results == null)
        {
            CustomTextParser.Singleton.Print("<Warn>An error has occured while fetching the latest update.");
            return;
        }

        if (results["tags"] != Version)
        {
            //created_at or published_at
            if (DateTime.Parse(results["created_at"]) > BuiltAt.AddHours(1))
            {
                CustomTextParser.Singleton.Print("<Warn>An update is available. Please consider updating the program for the latest config options and support.");
                /*
                "upload_url": "https://uploads.github.com/repos/redforce04/MGHBetterConfigs/releases/1/assets{?name,label}",
                "tarball_url": "https://api.github.com/repos/redforce04/MGHBetterConfigs/tarball/latest",
                "zipball_url": "https://api.github.com/repos/redforce04/MGHBetterConfigs/zipball/latest",
                 */
            }
            else
            {
                // CustomTextParser.Singleton.Print("<Accent>It seems you are using a beta build. Some features may be broken.");
            }
        }
        
    }

    public static string Version = "v1.0.1-beta";
    public static DateTime BuiltAt = new DateTime(7,29,2023,20,55,0);
}