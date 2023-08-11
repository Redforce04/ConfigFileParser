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

        public static ColorSchemes ExportedColorScheme = ColorSchemes.Console;
        public Config()
        {
            Singleton = this;
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
        public bool Silent = false;
        public bool SuperSilent = false;
        public bool Debug = false;
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
        Console,
        Pterodactyl,
        Linux
    }
}
