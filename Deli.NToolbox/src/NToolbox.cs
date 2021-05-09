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

namespace NToolbox
{
    public class NToolbox : DeliBehaviour
    {
        /// <summary>
        /// Every button to be on the wrist menu. The scene buttons are seperate 
        /// </summary>
        public readonly Dictionary<string, UnityAction<FVRWristMenu>> WristMenuButtons = new()
        {
            //Item interactions
            { "Gather Items", Actions.GatherButtonClicked },
            { "Reset Traps", Actions.ResetTrapsButtonClicked },
            { "Freeze Guns", Actions.FreezeFireArmsButtonClicked },
            { "Unfreeze Guns", Actions.UnFreezeFireArmsButtonClicked },
            
            //Player body interactions
            { "Restore Full", Actions.RestoreHPButtonClicked },
            { "Restore 10%", Actions.Restore10PercentHPButtonClicked },
            { "Toggle 1-hit", Actions.ToggleOneHitButtonClicked },
            { "Toggle God Mode", Actions.ToggleGodModeButtonClicked },
            { "Kill yourself", Actions.KillPlayerButtonClicked },
            { "Blind yourself", Actions.BlindButtonClicked },
            
            //Take and Hold interactions
            { "Add token", Actions.AddTokenButton },
            { "End hold", Actions.EndHoldButton },
            { "Kill patrols", Actions.KillPatrolsButton },
        };
        
        
        public NToolbox()
        {
            Logger.LogInfo($"Loading {WristMenuButtons.Count} WristMenu actions"); 
            
            foreach (var kvp in WristMenuButtons)
            {
                WristMenu.RegisterWristMenuButton(kvp.Key, kvp.Value);
                
                Logger.LogDebug($"Loaded action {kvp.Key}");
            }

            var scenes = SceneList.Scenes;
            
            foreach (var scene in scenes)
            {
                WristMenu.RegisterWristMenuButton
                (
                    scene.Name, e =>
                    {
                        SteamVR_LoadLevel.Begin
                        (
                            scene.SceneName,
                            false,
                            0.5f,
                            0f,
                            0f,
                            1f
                        );

                        foreach (var quitReceiver in GM.CurrentSceneSettings.QuitReceivers)
                        {
                            quitReceiver.BroadcastMessage("QUIT", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                );
            }
            
            Logger.LogInfo("Fully loaded NToolbox!");
        }
    }
}