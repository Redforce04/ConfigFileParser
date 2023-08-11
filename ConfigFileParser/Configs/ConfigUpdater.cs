﻿// <copyright file="Log.cs" company="Redforce04#4091">
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
// git tag -a [tag name (ex: v1.0.1-beta)] -m [message name: (fix auto updater)]
// git push origin [tag name]

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading;
using ConfigFileParser.Components;
using Newtonsoft.Json;
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

        try
        {

            HttpClient updateClient = new HttpClient();
            if (Config.Singleton.GithubApiKey != "")
            {
                updateClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{Config.Singleton.GithubApiKey}");
            }

            updateClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            updateClient.DefaultRequestHeaders.UserAgent.Add( new ProductInfoHeaderValue("Redforce04-MGHBetterConfigs", VersionInfo.CommitVersion));
            var response = updateClient
                .GetAsync(@"https://api.github.com/repos/Redforce04/MGHBetterConfigs/releases/latest").Result.Content;
            var json = response.ReadAsStringAsync().Result;
            Dictionary<string, object>? results = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (results == null || results.Count < 3 || !results.ContainsKey("tag_name") || !results.ContainsKey("created_at"))
            {
                CustomTextParser.Singleton.Print("<Warn>An error has occured while fetching the latest update.");
                if(Config.Singleton.Debug)
                {
                    Console.WriteLine("Results null or missing entries.");    
                }
                return;
            }
            if ((string)results["tag_name"] != VersionInfo.CommitVersion)
            { 
                // skip this check when debug because if this build isnt the latest tag, it is automatically old (unless debug). 
                
                var curDate = VersionInfo.BuildTime.AddHours(1);
                var updateDate = (DateTime)results["created_at"];
                bool NewUpdate = updateDate > curDate;

                if (Config.Singleton.Debug && NewUpdate)
                {
                    CustomTextParser.Singleton.Print(
                        "<Warn>An update is available. Please consider updating the program for the latest config options and support.");
                    /*
                    "upload_url": "https://uploads.github.com/repos/redforce04/MGHBetterConfigs/releases/1/assets{?name,label}",
                    "tarball_url": "https://api.github.com/repos/redforce04/MGHBetterConfigs/tarball/latest",
                    "zipball_url": "https://api.github.com/repos/redforce04/MGHBetterConfigs/zipball/latest",
                     */
                    string uploadUrl = $"https://github.com/Redforce04/MGHBetterConfigs/releases/download/{(string)results["tag_name"]}/";
                    string uploadId = "MGHBetterConfigs-";
                    if (OperatingSystem.IsLinux())
                    {
                        uploadId += "linux";
                    }
                    else if (OperatingSystem.IsWindows())
                    {
                        uploadId += "win";
                    }
                    else if (OperatingSystem.IsMacOS())
                    {
                        uploadId += "osx";
                    }

                    Architecture arch = RuntimeInformation.OSArchitecture;
                    switch (arch)
                    {
                        case Architecture.Arm:
                            uploadId += "-arm";
                            break;
                        case Architecture.Arm64:
                            uploadId += "-arm64";
                            break;
                        case Architecture.X64:
                            uploadId += "-x64";
                            break;
                        case Architecture.X86:
                            uploadId += "-x86";
                            break;
                        default:
                            // Architecture.Armv6 or Architecture.Ppc64le or Architecture.S390x or Architecture.LoongArch64 or wasm
                            break;
                    }

                    if (OperatingSystem.IsWindows())
                    {
                        uploadId += ".exe";
                    }

                    HttpClient client = new HttpClient();
                    var result = client.GetAsync(uploadUrl + uploadId).Result;
                    if (!result.IsSuccessStatusCode)
                    {
                        if(Config.Singleton.Debug)
                        {
                            Console.WriteLine($"Could not get download file. fileName: {uploadId} Url: {uploadUrl}");    
                        }
                        CustomTextParser.Singleton.Print(
                            $"<Warn>Could not find the proper download for the auto-updater. File: <Primary>'{uploadId}'");
                        return;
                    }

                    var data = result.Content.ReadAsStreamAsync().Result;
                    /*var hash = GetHashCode(data, new MD5CryptoServiceProvider());
                    if (Pending.FileHash != hash)
                    {
                        invalid filehash
                    }
                    else
                    */

                    var data1 = result.Content.ReadAsByteArrayAsync().Result;
                    string newFileName = uploadId.Contains(".exe")
                        ? uploadId.Replace(".exe", "") + "-new.exe"
                        : uploadId + "-new";
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + newFileName))
                    {
                        try
                        {
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + newFileName);
                        }
                        catch (Exception e)
                        {
                            CustomTextParser.Singleton.Print(
                                $"<Warn>Could not auto-update. Failed to replace the old file. Aborting auto-update process.");
                            return;
                        }
                    }

                    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + newFileName, data1);
                    CustomTextParser.Singleton.Print(
                        $"<Accent>AutoUpdater has downloaded the new updated file and will attempt to restart.");
                    Thread.Sleep(3000);
                    List<string> args = Config.Singleton.Args.ToList();
                    args.Add("--updateNew");
                    var process = Process.Start(AppDomain.CurrentDomain.BaseDirectory + newFileName, args);
                    Environment.Exit(0);
                    //}
                }
            }
        }
        catch (Exception e)
        {
            if (Config.Singleton.Debug)
            {
                Console.WriteLine($"Caught an error: {e}");
            }
        }
    }
    internal static string GetHashCode(Stream stream, HashAlgorithm cryptoService)
    {
        using (cryptoService)
        {
            var hash = cryptoService.ComputeHash(stream);
            var hashString = Convert.ToBase64String(hash);
            return hashString.TrimEnd('=');
        }
    }
}
