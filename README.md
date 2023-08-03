# MGH BetterConfigs
A better configuration system for Midnight Ghost Hunt Servers.

## How It Works
On the first startup, MGH BetterConfigs runs through a prompt allowing for users to easily configure server options.

<br>
The specified config is then written to a user-friendly config.json file, and the CustomRules.ini is replaced with an updated version. 

<br>
<br>
Everytime  MGH BetterConfigs is run, the Config.json is used to automatically replace CustomRules.ini. (This allows users to just update the config.json)

![Demo 1](https://cdn.peanutworshipers.net/demo-1.gif) 
> A simple and easy method for managing lists.

![Demo 2](https://cdn.peanutworshipers.net/demo-2.gif)
> Quickly use default values, or set your own value.

<br>

Advantages:
- Simplifies configuration
- Easy first time server configuration
- Provides more detailed config explanations
- Identifies new and old configs that are missing or unnecessary
- Takes only 1 line to integrate into pre-existing startup files

![Demo 3](https://cdn.peanutworshipers.net/demo-3.png)


## Better Config File
As mentioned prior, MGH BetterConfigs creates a separate config.json file that provides a detailed yet simple way to manage configs. 
```json
"Items": [
        {
            "Name": "Custom Damage Mods", // Better names
            "ConfigName": "CustomDamageMods", 
            "InternalFieldName": "CustomDamageMods",
            "Description": "What damage types should have custom multipliers?",
            "DefaultValue": "Empty",
            // informs users of what the default value is
            "Value": {},
            "Type": "Dictionary[DamageMethod, Single]" 
            // information about what type of values the config uses
        },
        {
            "Name": "Banned Hunter Weapon / Gadgets",
            "ConfigName": "BannedHunterWeaponGadgets",
            "InternalFieldName": "BannedHunterWeaponGadgets",
            "Description": "What hunter weapons / gadgets should be banned from being used.",
            "DefaultValue": "Empty",
            "Value": [
              "C4",
              "Trap""
            ], // Normal json parsing.
            "Type": "List[HunterGadget]"
        },
]
```
If a config.json file is detected on startup, the user will not be prompt to re-enter every value. (This can be forced with the  `--force-reconfigure` flag.)

The program will always replace the CustomRules.ini with the Config.json values.

## Installation & Usage
MGH BetterConfigs can be run independently, or incorporated into a startup script. <br><br>To install MGH BetterConfigs, just drop the three files into the server directory:
- MGHBetterConfigs.exe
- MGHBetterConfigs.dll
- MGHBetterConfigs.runtimeconfig.json

Then include it in the startup script.

Example Startup Script (.bat):
```bat
MGHBetterConfigs.exe
MidnightGhostHuntServer.exe -log -Map=Mansion -Gamename="My MGH Server" -Gamemode="4v4 Hunt" -Region="North America Central" -password=""
```

## Arguments:
###  `MGHBetterConfigs.exe [args] [input config] [output config]`
- `input config` - 
- `-r` or `--force-reconfigure`: Triggers the initial installation configuration prompts.
- `-q` or `--quiet`: Removes the banner and unnecessary formatting options.
- `-qq` or `--quietquiet`: Removes user input from the script and makes any output minimal. (headless / non-interactive operation)



## Building:
MGH Better Configs has a slightly complicated system for building. I have laid out the process for updating and rebuilding it for all architectures here.


> Most of the tools for building this are designed for windows but can be built for linux (by default it should already work via dotnets multi-architecture design).

During the build the following will happen:
- The `BlankVersionInfo.txt` file overwrites the `ArgumentParser.cs`.
- - This file is used for variables that are filled in during the build process. 
- -  The 3 variables notably identified by the "${}" will be replaced.
- - This happens with another program called "ReplaceTextWithVariables" (found in project directory)
- - This helps the program identify git tracking info for the auto-updater.
- The project is built.
- The project is published for all architectures. 
- All builds are renamed, then moved to a /bin/Releases/export folder by default.
-  - This makes it easy to find all the different builds.


#### To easily build the program, use the `Build.bat` or `Build.sh` that is included in the root of the script, and it should output all build files to ConfigFileParser/bin/Release/export, neatly labeled with architectures.