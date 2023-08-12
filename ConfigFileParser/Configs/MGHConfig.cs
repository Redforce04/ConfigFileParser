using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ConfigFileParser.Components;
using Newtonsoft.Json.Converters;
// ReSharper disable InconsistentNaming

namespace ConfigFileParser.Configs
{
    internal class MGHConfig
    {
        [Name("Custom Damage Mods")]
        [ConfigName("CustomDamageMods")]
        [Description("What damage types should have custom multipliers?")]
        [JsonParser]
        [DefaultValue("Empty")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Dictionary<Enums.DamageMethod, float> CustomDamageMods = new Dictionary<Enums.DamageMethod, float>();

        [Name("Banned Hunter Weapon / Gadgets")]
        [ConfigName("BannedHunterWeaponGadgets")]
        [Description("What hunter weapons / gadgets should be banned from being used.")]
        [JsonParser]
        [DefaultValue("Empty")]
        [JsonConverter(typeof(StringEnumConverter))]
        public List<Enums.HunterGadget> BannedHunterWeaponGadgets = new List<Enums.HunterGadget>();

        [Name("Banned Hunter Perks")]
        [ConfigName("BannedHunterPerks")]
        [Description("What hunter perks should be banned from being used.")]
        [JsonParser]
        [DefaultValue("Empty")]
        [JsonConverter(typeof(StringEnumConverter))]
        public List<Enums.HunterPerk> BannedHunterPerks = new List<Enums.HunterPerk>();

        [Name("Banned Ghost Abilities")]
        [ConfigName("BannedGhostAbilities")]
        [Description("What ghost abilities should be banned from being used.")] 
        [JsonParser]
        [DefaultValue("Empty")]
        [JsonConverter(typeof(StringEnumConverter))]
        public List<Enums.GhostAbility> BannedGhostAbilities = new List<Enums.GhostAbility>();
        
        [Name("Banned Ghost Haunts")]
        [ConfigName("BannedGhostHaunts")]
        [Description("What ghost haunts should be banned from being used.")]
        [JsonParser]
        [DefaultValue("Empty")]
        [JsonConverter(typeof(StringEnumConverter))]
        public List<Enums.GhostHaunt> BannedGhostHaunts = new List<Enums.GhostHaunt>();

        [Name("Banned Ghost Perks")]
        [ConfigName("BannedGhostPerks")]
        [Description("What ghost perks should be banned from being used.")]
        [JsonParser]
        [DefaultValue("Empty")]
        [JsonConverter(typeof(StringEnumConverter))]
        public List<Enums.GhostPerk> BannedGhostPerks = new List<Enums.GhostPerk>();

        [Name("Force Choose Team")]
        [ConfigName("Custom - ForceChooseTeam")]
        [Description("Will players be forced to choose a team?")]
        [CustomParser]
        public bool ForceChooseTeam = true;

        [Name("Pre Match Time")]
        [ConfigName("Custom - PreMatchTime")]
        [Description("How many seconds will the pre-round last.")]
        [CustomParser]
        public int PreMatchTime = 90;

        [Name("Midnight Time")]
        [ConfigName("Custom - MidnightTime")]
        [Description("How many seconds before midnight hits.")]
        [CustomParser]
        public int MidnightTime = 240;

        [Name("Post Midnight Time")]
        [ConfigName("Custom - PostMidnightTime")]
        [Description("How many seconds hunters must survive after midnight to escape.")]
        [CustomParser]
        public int PostMidnightTime = 180;

        [Name("Ghosts Revive After Midnight")]
        [ConfigName("Custom - GhostsReviveAfterMidnight")]
        [Description("Should dead ghosts be revived at midnight?")]
        [CustomParser]
        public bool GhostsReviveAfterMidnight = true;

        [Name("Ghost Ability Cooldown Multiplier")]
        [ConfigName("Custom - Ghost Ability Cooldown Multiplier")]
        [Description("What should the cooldown multiplier be for ghost abilities?")]
        [CustomParser]
        public float GhostAbilityCooldownMultiplier = 1.0f;

        [Name("Ghost Movement Speed Multiplier")]
        [ConfigName("Custom - Ghost Movement Speed Multiplier")]
        [Description("What should the speed multiplier be for ghosts?")]
        [CustomParser]
        public float GhostMovementSpeedMultiplier = 1.0f;

        [Name("Hunter Movement Speed Multiplier")]
        [ConfigName("Custom - Hunter Movement Speed Multiplier")]
        [Description("What should the speed multiplier be for ghosts?")]
        [CustomParser]
        public float HunterMovementSpeedMultiplier = 1.0f;

        [Name("Limited UI Mode")]
        [ConfigName("Custom - Limited UI Mode")]
        [Description("Should limited UI Mode be enabled?")]
        [CustomParser]
        public bool LimitedUIMode = false;

        [Name("Revives Disabled")]
        [ConfigName("Custom - Revives Disabled")]
        [Description("Should revives be disabled?")]
        [CustomParser]
        public bool RevivesDisabled = false;

        [Name("Hunter - Use Max Player Count")]
        [ConfigName("Custom - Hunter - Use Max Player Count")]
        [Description("Should the max player count apply to hunters?")]
        [CustomParser]
        public bool HunterUseMaxPlayerCount = true;

        [Name("Ghost - Use Max Player Count")]
        [ConfigName("Custom - Ghost - Use Max Player Count")]
        [Description("Should the max player count apply to ghosts?")]
        [CustomParser]
        public bool GhostUseMaxPlayerCount = true;

        [Name("Hunter - Max Player Count")]
        [ConfigName("Custom - Hunter - Max Player Count")]
        [Description("What should the max player count be for hunters?")]
        [CustomParser]
        public int HunterMaxPlayerCount = 4;

        [Name("Ghost - Max Player Count")]
        [ConfigName("Custom - Ghost - Max Player Count")]
        [Description("What should the max player count be for ghosts?")]
        [CustomParser]
        public int GhostMaxPlayerCount = 4;

        [Name("All Loadout Unlocked")]
        [ConfigName("Custom - All Loadout Unlocked")]
        [Description("Should all loadouts be unlocked?")]
        [CustomParser]
        public bool AllLoadoutUnlocked = false;

        [Name("Mystery Loadout")]
        [ConfigName("Custom - Mystery Loadout")]
        [Description("Should a mystery loadout be equipped for players automatically?")]
        [CustomParser]
        public bool MysteryLoadout = false;

        [Name("Ghost Slingshot Multiplier")]
        [ConfigName("Custom - Ghost Slingshot Multiplier")]
        [Description("What should the multiplier be for the ghost slingshot attack?")]
        [CustomParser]
        public float GhostSlingshotMultiplier = 1.0f;

        [Name("Allow Ghost Respawns")]
        [ConfigName("Custom - Allow Ghost Respawns")]
        [Description("Should ghosts be able to respawn?")]
        [CustomParser]
        public bool AllowGhostRespawns = true;

        [Name("Ghost - Max Respawn Count")]
        [ConfigName("Custom - Ghost - Max Respawn Count")]
        [Description("How many total respawns should the ghost team have?")]
        [CustomParser]
        public int GhostMaxRespawnCount = 100;

        [Name("Allow Hunter Respawns")]
        [ConfigName("Custom - Allow Hunter Respawns")]
        [Description("Should hunters be able to respawn?")]
        [CustomParser]
        public bool AllowHunterRespawns = true;

        [Name("Hunter - Max Respawn Count")]
        [ConfigName("Custom - Hunter - Max Respawn Count")]
        [Description("How many total respawns should the hunter team have?")]
        [CustomParser]
        public int HunterMaxRespawnCount = 100;

        [Name("Disable Life Reserve System")]
        [ConfigName("Custom - Disable Life Reserve System?")]
        [Description("Should players that join mid-match replace bots that are alive?")]
        [CustomParser]
        public bool DisableLifeReserveSystem = true;

        [Name("Hunter Health Modifier")]
        [ConfigName("Custom - Hunter Health Modifier")]
        [Description("What should the health multiplier be for hunters?")]
        [CustomParser]
        public float HunterHealthModifier = 1.0f;

        [Name("Ghost Health Modifier")]
        [ConfigName("Custom - Ghost Health Modifier")]
        [Description("What should the health multiplier be for ghosts?")]
        [CustomParser]
        public float GhostHealthModifier = 1.0f;

        [Name("Ecto Build Up Speed Multiplier")]
        [ConfigName("Custom - Ecto Build Up Speed Multiplier")]
        [Description("What should the speed multiplier be for ectoplasm buildup?")]
        [CustomParser]
        public float EctoBuildUpSpeedMultiplier = 1.0f;

        [Name("Unpossessed Prop Health Multiplier")]
        [ConfigName("Custom - Unpossessed Prop Health Multiplier")]
        [Description("What should the health multiplier be for props that are not currently possessed?")]
        [CustomParser]
        public float UnpossessedPropHealthMultiplier = 1.5f;

        [Name("Hunter Gadget Count Multiplier")]
        [ConfigName("Custom - Hunter Gadget Count Multiplier")]
        [Description("What should the gadget count multiplier be for hunters?")]
        [CustomParser]
        public float HunterGadgetCountMultiplier;

        [Name("Self Damage When Hit Level-Prop")]
        [ConfigName("Custom - Self Damage When Hit Lvlprop?")]
        [Description("Should hunters take damage when they hit an un-possessed prop?")]
        [CustomParser]
        public bool SelfDamageWhenHitLvlprop = false;

        [Name("Hunter - Self Damage Level-Prop Amount")]
        [ConfigName("Custom - Hunter - Self Damage Lvlprop Amount")]
        [Description("How much damage will hunters take if they hit an un-possessed prop.")]
        [CustomParser]
        public int HunterSelfDamageLvlpropAmount = 0;

        [Name("AdminPUIDs")]
        [ConfigName("AdminPUIDs")]
        [Description("What players should be able to access the admin menu.")]
        [JsonParser]
        [DefaultValue("Empty")]
        public List<string> AdminPUIDs = new List<string>();

    }

    public static class Enums
    {

        public static DamageMethod GetDamageMethod(this string translation)
        {
            return DamageMethodDict[translation];
        }
        public static string ToString(this DamageMethod translation)
        {
            return DamageMethodDict.FirstOrDefault(x => x.Value == translation).Key;
        }
        public static Dictionary<string, DamageMethod> DamageMethodDict = new Dictionary<string, DamageMethod>()
        {
            { "Spectral Cannon", DamageMethod.Spectral_Cannon },
            { "Grenade", DamageMethod.Grenade },
            { "Ghostsmasher", DamageMethod.Ghostsmasher },
            { "Project X", DamageMethod.Project_X },
            { "Trap", DamageMethod.Trap },
            { "Poltergeist", DamageMethod.Poltergeist },
            { "Ghost Attack", DamageMethod.Ghost_Attack },
            { "Doppelganger", DamageMethod.Doppelganger },
            { "Harpoon Bazooka", DamageMethod.Harpoon_Bazooka },
            { "Miasma Burn", DamageMethod.Miasma_Burn },
            { "Sledgehammer", DamageMethod.Sledgehammer },
            { "Reaper", DamageMethod.Reaper },
            { "Reaper Bolt Explosion", DamageMethod.Reaper_Bolt_Explosion },
            { "Salt Shotgun", DamageMethod.Salt_Shotgun },
            { "Ghost Punch", DamageMethod.Ghost_Punch },
            { "Ship Cannon", DamageMethod.Ship_Cannon },
            { "Corruption", DamageMethod.Corruption },
            { "Chest Bite", DamageMethod.Chest_Bite },
            { "Frostbite", DamageMethod.Frostbite },
            { "Telekinesis", DamageMethod.Telekinesis },
            { "C4", DamageMethod.C4 },
            { "Reaper Bolt Zap", DamageMethod.Reaper_Bolt_Zap },
            { "Harpoon Bazooka Zap", DamageMethod.Harpoon_Bazooka_Zap },
            { "Riot Shield Bash", DamageMethod.Riot_Shield_Bash },
            { "Flamethrower", DamageMethod.Flamethrower },
            { "Gargoyle Attack", DamageMethod.Gargoyle_Attack },
            { "Forklift Attack", DamageMethod.Forklift_Attack },
            { "Knight Sword", DamageMethod.Knight_Sword },
            { "Fridge Attack", DamageMethod.Fridge_Attack },
            { "Door Attack", DamageMethod.Door_Attack },
            { "T-Rex Bite", DamageMethod.TRex_Bite },
            { "Skeleton Attack", DamageMethod.Skeleton_Attack },
        };
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public enum DamageMethod
        {
            [EnumMember(Value = "Spectral Cannon")]
            Spectral_Cannon,
            [EnumMember(Value = "Grenade")]
            Grenade,
            [EnumMember(Value = "Ghostsmasher")]
            Ghostsmasher,
            [EnumMember(Value = "Project X")] 
            Project_X, 
            [EnumMember(Value = "Trap")]
            Trap, 
            [EnumMember(Value = "Poltergeist")]
            Poltergeist,
            [EnumMember(Value = "Ghost Attack")]
            Ghost_Attack, 
            [EnumMember(Value = "Doppelganger")]
            Doppelganger, 
            [EnumMember(Value = "Harpoon Bazooka")]
            Harpoon_Bazooka, 
            [EnumMember(Value = "Miasma Burn")]
            Miasma_Burn, 
            [EnumMember(Value = "Sledgehammer")]
            Sledgehammer,
            [EnumMember(Value = "Reaper")]
            Reaper, 
            [EnumMember(Value = "Reaper Bolt Explosion")]
            Reaper_Bolt_Explosion, 
            [EnumMember(Value = "Salt Shotgun")]
            Salt_Shotgun, 
            [EnumMember(Value = "Ghost Punch")]
            Ghost_Punch, 
            [EnumMember(Value = "Ship Cannon")]
            Ship_Cannon,
            [EnumMember(Value = "Corruption")]
            Corruption, 
            [EnumMember(Value = "Chest Bite")]
            Chest_Bite, 
            [EnumMember(Value = "Frostbite")]
            Frostbite, 
            [EnumMember(Value = "Telekinesis")]
            Telekinesis, 
            [EnumMember(Value = "C4")]
            C4, 
            [EnumMember(Value = "Reaper Bolt Zap")]
            Reaper_Bolt_Zap,
            [EnumMember(Value = "Harpoon Bazooka Zap")]
            Harpoon_Bazooka_Zap, 
            [EnumMember(Value = "Riot Shield Bash")]
            Riot_Shield_Bash,
            [EnumMember(Value = "Flamethrower")]
            Flamethrower, 
            [EnumMember(Value = "Gargoyle Attack")]
            Gargoyle_Attack,
            [EnumMember(Value = "Forklift Attack")]
            Forklift_Attack, 
            [EnumMember(Value = "Knight Sword")]
            Knight_Sword, 
            [EnumMember(Value = "Fridge Attack")]
            Fridge_Attack, 
            [EnumMember(Value = "Door Attack")]
            Door_Attack, 
            [EnumMember(Value = "T-Rex Bite")]
            TRex_Bite,
            [EnumMember(Value = "Skeleton Attack")]
            Skeleton_Attack
        }
        public static HunterGadget GetHunterGadget(this string translation)
        {
            return HunterGadgetDict[translation];
        }
        public static string ToString(this HunterGadget translation)
        {
            return HunterGadgetDict.FirstOrDefault(x => x.Value == translation).Key;
        }

        public static Dictionary<string, HunterGadget> HunterGadgetDict = new Dictionary<string, HunterGadget>()
        {
            { "Spectral Cannon", HunterGadget.Spectral_Cannon },
            { "Radar", HunterGadget.Radar },
            { "Pathfinder", HunterGadget.Pathfinder },
            { "Trap", HunterGadget.Trap },
            { "Grenade", HunterGadget.Grenade },
            { "Defibrillator", HunterGadget.Defibrillator },
            { "Project X", HunterGadget.Project_X },
            { "Harpoon Bazooka", HunterGadget.Harpoon_Bazooka },
            { "Sledgehammer", HunterGadget.Sledgehammer },
            { "Reaper", HunterGadget.Reaper },
            { "Grappling Hook", HunterGadget.Grappling_Hook },
            { "Vacuum", HunterGadget.Vacuum },
            { "Salt Shotgun", HunterGadget.Salt_Shotgun },
            { "Spectrophone", HunterGadget.Spectrophone },
            { "Frostbite", HunterGadget.Frostbite },
            { "C4", HunterGadget.C4 },
            { "Riot Shield", HunterGadget.Riot_Shield },
            { "Medic Kit", HunterGadget.Medic_Kit },
            { "Flamethrower", HunterGadget.Flamethrower },
            { "Ghostsmasher", HunterGadget.Ghostsmasher },
            { "CamCorder", HunterGadget.CamCorder },
        };
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public enum HunterGadget
        {
            [EnumMember(Value = "Spectral Cannon")]
            Spectral_Cannon, 
            [EnumMember(Value = "Radar")]
            Radar, 
            [EnumMember(Value = "Pathfinder")]
            Pathfinder, 
            [EnumMember(Value = "Trap")]
            Trap, 
            [EnumMember(Value = "Grenade")]
            Grenade, 
            [EnumMember(Value = "Defibrillator")]
            Defibrillator,
            [EnumMember(Value = "Project X")]
            Project_X, 
            [EnumMember(Value = "Harpoon Bazooka")]
            Harpoon_Bazooka, 
            [EnumMember(Value = "Sledgehammer")]
            Sledgehammer, 
            [EnumMember(Value = "Reaper")]
            Reaper, 
            [EnumMember(Value = "Grappling Hook")]
            Grappling_Hook,
            [EnumMember(Value = "Vacuum")]
            Vacuum, 
            [EnumMember(Value = "Salt Shotgun")]
            Salt_Shotgun, 
            [EnumMember(Value = "Spectrophone")]
            Spectrophone, 
            [EnumMember(Value = "Frostbite")]
            Frostbite, 
            [EnumMember(Value = "C4")]
            C4, 
            [EnumMember(Value = "Riot Shield")]
            Riot_Shield,
            [EnumMember(Value = "Medic Kit")]
            Medic_Kit, 
            [EnumMember(Value = "Flamethrower")]
            Flamethrower, 
            [EnumMember(Value = "Ghostsmasher")]
            Ghostsmasher, 
            [EnumMember(Value = "CamCorder")]
            CamCorder
        }
        public static HunterPerk GetHunterPerk(this string translation)
        {
            return HunterPerkDict[translation];
        }
        public static string ToString(this HunterPerk translation)
        {
            return HunterPerkDict.FirstOrDefault(x => x.Value == translation).Key;
        }

        public static Dictionary<string, HunterPerk> HunterPerkDict = new Dictionary<string, HunterPerk>()
        {
            { "Lightweight", HunterPerk.Lightweight },
            { "Juggernaut", HunterPerk.Juggernaut },
            { "Coldblooded", HunterPerk.Coldblooded },
            { "Healing Aura", HunterPerk.Healing_Aura },
            { "Quick Reload", HunterPerk.Quick_Reload },
            { "Extended Mag", HunterPerk.Extended_Mag },
            { "Extra Gear", HunterPerk.Extra_Gear },
            { "Gadgeteer", HunterPerk.Gadgeteer },
            { "Overkill", HunterPerk.Overkill },
        };

        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public enum HunterPerk
        {
            [EnumMember(Value = "Lightweight")]
            Lightweight, 
            [EnumMember(Value = "Juggernaut")]
            Juggernaut, 
            [EnumMember(Value = "Coldblooded")]
            Coldblooded, 
            [EnumMember(Value = "Healing Aura")]
            Healing_Aura, 
            [EnumMember(Value = "Quick Reload")]
            Quick_Reload,
            [EnumMember(Value = "Extended Mag")]
            Extended_Mag, 
            [EnumMember(Value = "Extra Gear")]
            Extra_Gear, 
            [EnumMember(Value = "Gadgeteer")]
            Gadgeteer, 
            [EnumMember(Value = "Overkill")]
            Overkill
        }

        public static GhostAbility GetGhostAbility(this string translation)
        {
            return GhostAbilityDict[translation];
        }
        public static string ToString(this GhostAbility translation)
        {
            return GhostAbilityDict.FirstOrDefault(x => x.Value == translation).Key;
        }

        public static Dictionary<string, GhostAbility> GhostAbilityDict = new Dictionary<string, GhostAbility>()
        {
            { "Doppelganger", GhostAbility.Doppelganger },
            { "Phantom", GhostAbility.Phantom },
            { "Telekinesis", GhostAbility.Telekinesis },
            { "Corruptor", GhostAbility.Corruptor },
            { "Spirit", GhostAbility.Spirit },
            { "Miasma", GhostAbility.Miasma },
            { "Apparition", GhostAbility.Apparition },
            { "Death Grip", GhostAbility.Death_Grip },
            { "Poltergeist", GhostAbility.Poltergeist },
            { "Trickster", GhostAbility.Trickster },
            { "Deflector", GhostAbility.Deflector  },
        };

        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public enum GhostAbility
        {
            [EnumMember(Value = "Doppelganger")]
            Doppelganger, 
            [EnumMember(Value = "Phantom")]
            Phantom, 
            [EnumMember(Value = "Telekinesis")]
            Telekinesis, 
            [EnumMember(Value = "Corruptor")]
            Corruptor, 
            [EnumMember(Value = "Spirit")]
            Spirit, 
            [EnumMember(Value = "Miasma")]
            Miasma, 
            [EnumMember(Value = "Apparition")]
            Apparition, 
            [EnumMember(Value = "Death Grip")]
            Death_Grip, 
            [EnumMember(Value = "Poltergeist")]
            Poltergeist, 
            [EnumMember(Value = "Trickster")]
            Trickster, 
            [EnumMember(Value = "Deflector")]
            Deflector
        }

        public static GhostHaunt GetGhostHaunt(this string translation)
        {
            return GhostHauntDict[translation];
        }
        public static string ToString(this GhostHaunt translation)
        {
            return GhostHauntDict.FirstOrDefault(x => x.Value == translation).Key;
        }

        public static Dictionary<string, GhostHaunt> GhostHauntDict = new Dictionary<string, GhostHaunt>()
        {
            { "Chill", GhostHaunt.Chill },
            { "Cold Spot", GhostHaunt.Cold_Spot },
            { "False Trail", GhostHaunt.False_Trail },
            { "Push Object", GhostHaunt.Push_Object },
            { "Medium", GhostHaunt.Medium },
            { "Health Orb", GhostHaunt.Health_Orb },
            { "Shove Hunter", GhostHaunt.Shove_Hunter },
            { "Hallucinate", GhostHaunt.Hallucinate },

        };
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public enum GhostHaunt
        {
            [EnumMember(Value = "Chill")]
            Chill, 
            [EnumMember(Value = "Cold Spot")]
            Cold_Spot, 
            [EnumMember(Value = "False Trail")]
            False_Trail, 
            [EnumMember(Value = "Push Object")]
            Push_Object, 
            [EnumMember(Value = "Medium")]
            Medium, 
            [EnumMember(Value = "Health Orb")]
            Health_Orb, 
            [EnumMember(Value = "Shove Hunter")]
            Shove_Hunter, 
            [EnumMember(Value = "Hallucinate")]
            Hallucinate
        }
        public static GhostPerk GetGhostPerk(this string translation)
        {
            return GhostPerkDict[translation];
        }
        public static string ToString(this GhostPerk translation)
        {
            return GhostPerkDict.FirstOrDefault(x => x.Value == translation).Key;
        }
        public static Dictionary<string, GhostPerk> GhostPerkDict = new Dictionary<string, GhostPerk>()
        {
            { "Heavyweight", GhostPerk.Heavyweight },
            { "Ghostly Reach", GhostPerk.Ghostly_Reach },
            { "Shatterproof", GhostPerk.Shatterproof },
            { "Ghostly Focus", GhostPerk.Ghostly_Focus },
            { "Foreseer", GhostPerk.Foreseer },
            { "Perception", GhostPerk.Perception },
            { "Untrappable", GhostPerk.Untrappable },
            { "Ecto-slow", GhostPerk.Ectoslow },
            { "Glutton", GhostPerk.Glutton },
            { "Blast Resistant", GhostPerk.Blast_Resistant },
            { "Quickcharge", GhostPerk.Quickcharge },

        };
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public enum GhostPerk
        {
            [EnumMember(Value = "Heavyweight")]
            Heavyweight, 
            [EnumMember(Value = "Ghostly Reach")]
            Ghostly_Reach,
            [EnumMember(Value = "Shatterproof")]
            Shatterproof, 
            [EnumMember(Value = "Ghostly Focus")]
            Ghostly_Focus, 
            [EnumMember(Value = "Foreseer")]
            Foreseer, 
            [EnumMember(Value = "Perception")]
            Perception, 
            [EnumMember(Value = "Untrappable")]
            Untrappable, 
            [EnumMember(Value = "Ecto-slow")]
            Ectoslow, 
            [EnumMember(Value = "Glutton")]
            Glutton, 
            [EnumMember(Value = "Blast Resistant")]
            Blast_Resistant, 
            [EnumMember(Value = "Quickcharge")]
            Quickcharge
        }
    }


}


/*
 1. CustomDamageMods - {DamageMethod: float}:  | Key: Spectral Cannon, Grenade, Ghostsmasher, Project X, Trap, Poltergeist, Ghost Attack, Doppelganger, Harpoon Bazooka, Miasma Burn, Sledgehammer, Reaper, Reaper Bolt Explosion, Salt Shotgun, Ghost Punch, Ship Cannon, Corruption, Chest Bite, Frostbite, Telekinesis, C4, Reaper Bolt Zap, Harpoon Bazooka Zap, Riot Shield Bash, Flamethrower, Gargoyle Attack, Forklift Attack, Knight Sword, Fridge Attack, Door Attack, T-Rex Bite, Skeleton Attack
 2. BannedHunterWeaponGadgets - [HunterGadgets]: Spectral Cannon, Radar, Pathfinder, Trap, Grenade, Defibrillator, Project X, Harpoon Bazooka, Sledgehammer, Reaper, Grappling Hook, Vacuum, Salt Shotgun, Spectrophone, Frostbite, C4, Riot Shield, Medic Kit, Flamethrower, Ghostsmasher, CamCorder
 3. BannedHunterPerks - [HunterPerks]: Lightweight, Juggernaut, Coldblooded, Healing Aura, Quick Reload, Extended Mag, Extra Gear, Gadgeteer, Overkill
 4. BannedGhostAbilities - [GhostAbility]: Doppelganger, Phantom, Telekinesis, Corruptor, Spirit, Miasma, Apparition, Death Grip, Poltergeist, Trickster, Deflector
 5. BannedGhostHaunts - [GhostHaunts]: Chill, Cold Spot, False Trail, Push Object, Medium, Health Orb, Shove Hunter, Hallucinate
 6. BannedGhostPerks - [GhostPerks]: Heavyweight, Ghostly Reach, Shatterproof, Ghostly Focus, Foreseer, Perception, Untrappable, Ecto-slow, Glutton, Blast Resistant, Quickcharge
 7. Custom - ForceChooseTeam - boolean: [False;True]
 8. Custom - PreMatchTime - integer
 9. Custom - MidnightTime - integer
 10. Custom - PostMidnightTime - integer
 11. Custom - GhostsReviveAfterMidnight? - boolean: [False;True]
 12. Custom - Ghost Ability Cooldown Multiplier - float
 13. Custom - Ghost Movement Speed Multiplier - float
 14. Custom - Hunter Movement Speed Multiplier - float
 15. Custom - Limited UI Mode - boolean: [False;True]
 16. Custom - Revives Disabled - boolean: [False;True]
 17. Custom - Hunter - Use Max Player Count - boolean: [False;True]
 18. Custom - Ghost - Use Max Player Count - boolean: [False;True]
 19. Custom - Hunter - Max Player Count - integer
 20. Custom - Ghost - Max Player Count - integer
 21. Custom - All Loadout Unlocked - boolean: [False;True]
 22. Custom - Mystery Loadout - boolean: [False;True]
 23. Custom - Ghost Slingshot Multiplier - float
 24. Custom - Allow Ghost Respawns - boolean: [False;True]
 25. Custom - Ghost - Max Respawn Count - integer
 26. Custom - Allow Hunter Respawns - boolean: [False;True]
 27. Custom - Hunter - Max Respawn Count - integer
 28. Custom - Disable Life Reserve System? - boolean: [False;True]
 29. Custom - Hunter Health Modifier - float
 30. Custom - Ghost Health Modifier - float
 31. Custom - Ecto Build Up Speed Multiplier - float
 32. Custom - Unpossessed Prop Health Multiplier - float
 33. Custom - Hunter Gadget Count Multiplier - float
 34. Custom - Self-Damage When Hit Lvlprop? - boolean: [False;True]
 35. Custom - Hunter - Self-Damage Lvlprop Amount - integer
 36. AdminPUIds - [string]
 */