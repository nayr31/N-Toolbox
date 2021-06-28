using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FistVR;
using HarmonyLib;

namespace NToolbox
{
    public static class Common
    {
        public const string SEPARATOR = "-----------------------------------------------------------------------";
        public const string OBJECT_ID_LIST_FILENAME = "ObjectIDs.txt";
        public static readonly Dictionary<string, string> SCENES = new Dictionary<string, string>
        {
            { "MainMenu3" , "Main Menu" },
            { "ArizonaTargets" , "Arizona Range" },
            { "ArizonaTargets_Night" , "Arizona at Night" },
            { "Boomskee", "Boomskee" },
            { "HickockRangeNew" , "Friendly 45 Range" },
            { "IndoorRange" , "Indoor Range" },
            { "ProvingGround" , "Proving Grounds" },
            { "SniperRange" , "Sniper Range" },
            { "TakeAndHold_Lobby_2" , "Take and Hold Lobby" },
        };
        public static readonly Dictionary<string, string> POWERUPS = new Dictionary<string, string>
        {
            { "PowerUpMeat_Blort" , "Blort" },
            { "PowerUpMeat_Cyclops" , "Cyclops" },
            { "PowerUpMeat_FarOut" , "Far out" },
            { "PowerUpMeat_Ghosted", "Ghosted" },
            { "PowerUpMeat_Health" , "Health" },
            { "PowerUpMeat_HomeTown" , "Hometown" },
            { "PowerUpMeat_InfiniteAmmo" , "Infinite Ammo" },
            { "PowerUpMeat_Invincibility" , "Invincibility" },
            { "PowerUpMeat_MuscleMeat" , "MuscleMeat" },
            { "PowerUpMeat_QuadDamage" , "Quad Damage" },
            { "PowerUpMeat_Regen" , "Regen" },
            { "PowerUpMeat_SnakeEye" , "SnakeEye" },
            { "PowerUpMeat_UnCooked" , "UnCooked" },
            { "PowerUpMeat_WheredIGo" , "WheredIGo" },
        };
        public static readonly Dictionary<string, string> MISC = new Dictionary<string, string>
        {
            { "CharcoalGrill" , "CharcoalGrill" },
            { "CharcoalBag" , "CharcoalBag" },
            { "Charcoal" , "Charcoal" },
        };
        public static class PluginInfo
        {
            public const string GUID = "bepinex.ntools";
            public const string NAME = "NToolbox";
            public const string VERSION = "1.4.0";

            public const string SODALITE_GUID = "nrgill28.Sodalite";
        }
    }
}