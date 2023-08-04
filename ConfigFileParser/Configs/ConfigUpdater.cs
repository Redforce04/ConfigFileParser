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

using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
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

        if (results["tags"] != VersionInfo.CommitVersion)
        {
            //created_at or published_at
            if (DateTime.Parse(results["created_at"]) > VersionInfo.BuildTime.AddHours(1))
            {
                CustomTextParser.Singleton.Print(
                    "<Warn>An update is available. Please consider updating the program for the latest config options and support.");
                /*
                "upload_url": "https://uploads.github.com/repos/redforce04/MGHBetterConfigs/releases/1/assets{?name,label}",
                "tarball_url": "https://api.github.com/repos/redforce04/MGHBetterConfigs/tarball/latest",
                "zipball_url": "https://api.github.com/repos/redforce04/MGHBetterConfigs/zipball/latest",
                 */
                string uploadUrl =
                    "https://uploads.github.com/repos/redforce04/MGHBetterConfigs/releases/latest/assets?name=";
                string uploadId = "MGHBetterConfigs-";
                if (OperatingSystem.IsLinux())
                {
                    uploadId += "linux";
                }
                else if (OperatingSystem.IsWindows())
                {
                    uploadId += "windows";
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
                var args = Config.Singleton.Args;
                args[args.Length] = "--updateNew";
                var process = Process.Start(AppDomain.CurrentDomain.BaseDirectory + newFileName, args);
                Environment.Exit(0);
                //}
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