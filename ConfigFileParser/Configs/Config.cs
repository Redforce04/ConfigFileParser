using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace ConfigFileParser.Configs
{
    internal class Config
    {
        public static Config Singleton;

        public static ColorSchemes ExportedColorScheme = ColorSchemes.Windows;
        public Config()
        {
            Singleton = this;
            switch (VersionInfo.Architecture)
            {
                case "win-x86" or "win-x64" or "win-arm" or "win-arm64":
                    ExportedColorScheme = ColorSchemes.Windows;
                    break;
                case "osx-x64" or "linux-x64" or "linux-musl-x64" or "linux-arm" or "linux-arm64":
                    ExportedColorScheme = ColorSchemes.Linux;
                    break;
            }
            
            /*var pteroVariable = Environment.GetEnvironmentVariable("");
            if (pteroVariable is not null && pteroVariable != "")
            {
                ExportedColorScheme = ColorSchemes.Pterodactyl;
            }*/
        }

        public string[] Args = new string[] { };
        public ConfigStyle OutputConfigStyle { get; set; } = ConfigStyle.MGH;
        public string GithubApiKey { get; set; } = "";
        public string InputFileLoc { get; set; } = "";
        public string OutputFileLoc { get; set; } = "";
        public bool AutoUpdate { get; set; } = true;
        public bool UseCache { get; set; } = false;
        public ParserType InputParserType { get; set; } = ParserType.None;
        internal string AttemptedInputParser = "";
        public ParserType OutputParserType { get; set; } = ParserType.None;
        internal string AttemptedOutputParser = "";
        public bool InteractiveStartup = false;
        public bool Silent = false;
        public bool SuperSilent = false;
        public bool Debug = false;
        public bool AttachDebugger = false;
        public bool DevelopmentMode = false;
    }

    public enum ConfigStyle
    {
        MGH,
        Generic
    }
    public enum ParserType
    {
        None,
        Json,
        Ini,
        txt,

    }

    public enum ColorSchemes
    {
        Windows,
        Pterodactyl,
        Linux
    }
}
