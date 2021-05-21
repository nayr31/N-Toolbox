using Deli.Immediate;
using Deli.Setup;
using Deli.H3VR.Api;
using Deli.H3VR;
using FistVR;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NToolbox
{
    public class NToolbox : DeliBehaviour
    {

        /// <summary>
        /// Every button to be on the wrist menu. The scene buttons are seperate 
        /// </summary>
        public readonly Dictionary<string, WristMenuButton.WristMenuButtonOnClick> WristMenuButtons = new()
        {
            { "-----------------------------------------------------------------------", Actions.Empty },

            //Item interactions
            { "Gather Items", Actions.GatherButtonClicked },
            { "Reset Traps", Actions.ResetTrapsButtonClicked },
            { "Freeze Guns/Melee", Actions.FreezeFireArmsMeleeButtonClicked },
            { "Freeze Ammo/Mags", Actions.FreezeAmmoMagButtonClicked },
            { "Freeze Attachments", Actions.FreezeAttachmentsButtonClicked },
            { "Unfreeze All", Actions.UnFreezeAllClicked },
            { "Ammo Panel", Actions.SpawnAmmoPanelButtonClicked },
            { "Ammo Weenie", Actions.SpawnAmmoWeenieButtonClicked },
            //trash bin
            //quickbelt fast?
            //sosig spawner

            { "--------------------------------------------------------------------", Actions.Empty},

            //Player body interactions
            { "Restore Full", Actions.RestoreHPButtonClicked },
            { "Restore 10%", Actions.Restore10PercentHPButtonClicked },
            { "Toggle 1-hit", Actions.ToggleOneHitButtonClicked },
            { "Toggle God Mode", Actions.ToggleGodModeButtonClicked },
            { "Kill yourself", Actions.KillPlayerButtonClicked },
            //{ "Toggle Invisibility", Actions.ToggleInvisButtonClicked },//Broken? Test for flat IFF = -1 to see if the check is broken

            { "--------------------------------------------------------------------------", Actions.Empty },

            //Take and Hold interactions
            { "Add token", Actions.AddTokenButtonClicked },
            //{ "End hold", Actions.EndHoldButton },//BUG - Bad things, doesn't mesh well with TnHTweaker
            { "SP - Ammo Reloader", Actions.SpawnAmmoReloaderButton },
            { "SP - Magazine Duplicator", Actions.SpawnMagDupeButton },
            { "SP - Recycler", Actions.SpawnGunRecylcerButton },
            { "Kill patrols", Actions.KillPatrolsButtonClicked },

            { "------------------------------------------------------------------------------", Actions.Empty },
        };

        public NToolbox()
        {
            _api.RequestLeaderboardDisable(Source, true);
            Dictionary<string, string> SceneList = Actions.SceneList;

            Logger.LogInfo($"Loading {WristMenuButtons.Count + Actions.SceneList.Count - 4} WristMenu actions");

            foreach (var scene in SceneList.Reverse())
            {
                _api.WristMenuButtons.Add(new WristMenuButton(scene.Value, (x, y) =>
                {
                    SteamVR_LoadLevel.Begin(scene.Key, false, 0.5f, 0f, 0f, 1f);
                    foreach (var quitReceiver in GM.CurrentSceneSettings.QuitReceivers)
                        quitReceiver.BroadcastMessage("QUIT", SendMessageOptions.DontRequireReceiver);
                }));
                Logger.LogDebug($"Loaded scene action {scene.Key}");
            }

            foreach (var kvp in WristMenuButtons.Reverse())
            {
                _api.WristMenuButtons.Add(new WristMenuButton(kvp.Key, kvp.Value));
                Logger.LogDebug($"Loaded action {kvp.Key}");
            }

            

            Logger.LogInfo("Fully loaded NToolbox!");
        }

        private readonly H3Api _api = H3Api.Instance;
    }
}