using FistVR;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using Sodalite.Api;
using NToolbox.src;

namespace NToolbox
{
    [BepInPlugin(Common.PluginInfo.GUID, Common.PluginInfo.NAME, Common.PluginInfo.VERSION)]
    [BepInDependency("nrgill28.Sodalite")]
    public class NToolbox : BaseUnityPlugin
    {
        public ConfigEntry<bool> LoadWristMenu;

        public NToolbox() 
        {
            //Set config option
            LoadWristMenu = Config.Bind("WristMenu Options", "LoadWristMenu", false, "If set to true, will load all wristmenu actions. I don't recommend using this, please don't.");
        }

        public void Start()
        {
            //Diable TnH leaderboard scoring
            LeaderboardAPI.GetLeaderboardDisableLock();

            NPanel _NPanel = new NPanel();
            WristMenuAPI.Buttons.Add(new WristMenuButton("NTool Panel", _NPanel.SpawnNPanel));

            if (LoadWristMenu.Value)
                _NPanel.LoadWristmenu();
        }
    }
}