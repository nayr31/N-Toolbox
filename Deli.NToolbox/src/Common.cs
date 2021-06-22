using System;
using System.Collections.Generic;

namespace NToolbox
{
    public class Common
    {
        public const string SEPARATOR = "-----------------------------------------------------------------------";
        public static readonly Dictionary<string, string> SCENE_LIST = new Dictionary<string, string>
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
        public static class PluginInfo
        {
            public const string GUID = "bepinex.ntools";
            public const string NAME = "NToolbox";
            public const string VERSION = "1.4.0";
        }
    }
}