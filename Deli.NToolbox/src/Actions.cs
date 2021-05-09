using System;
using System.Collections.Generic;
using System.Linq;
using FistVR;
using Deli.H3VR.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using NToolbox.src;

namespace NToolbox
{
    public static class Actions
    {
        public static void GatherButtonClicked(FVRWristMenu wristMenu)
        {
            //Get player pos upon every button press
            Vector3 playerPos = GM.CurrentPlayerBody.Head.position;

            //Whitelisted object gather
            foreach (var physObject in Object.FindObjectsOfType<FVRPhysicalObject>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null && WHITE_TYPES.Contains(physObject.GetType()) && physObject.transform.parent == null)
                    physObject.transform.position = playerPos + 
                        Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1.3f, 0.7f, 1.3f)) - new Vector3(0, 0.5f ,0);

            //Gun gather since its missing from the phys object gather
            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.transform.position = playerPos +
                        Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1.3f, 0.7f, 1.3f)) - new Vector3(0, 0.5f, 0);
        }

        public static void ResetTrapsButtonClicked(FVRWristMenu wristMenu)
        {
            foreach (var beartrap in Object.FindObjectsOfType<MF2_BearTrap>())
                if (!beartrap.IsHeld && beartrap.QuickbeltSlot == null)
                    beartrap.ForceOpen();
        }

        public static void FreezeFireArmsButtonClicked(FVRWristMenu wristMenu)
        {
            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = true;
        }

        public static void UnFreezeFireArmsButtonClicked(FVRWristMenu wristMenu)
        {
            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = false;
        }

        //player

        public static void RestoreHPButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.ResetHealth();
        }

        public static void Restore10PercentHPButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.HarmPercent(-10f);
        }

        public static void ToggleOneHitButtonClicked(FVRWristMenu wristMenu)
        {
            if(GM.CurrentPlayerBody.GetPlayerHealthRaw() != 1)
            {
                lastMax = GM.CurrentPlayerBody.m_startingHealth;
                GM.CurrentPlayerBody.SetHealthThreshold(1f);
            }
            else
            {
                GM.CurrentPlayerBody.SetHealthThreshold(lastMax);
                GM.CurrentPlayerBody.ResetHealth();
            }
        }

        public static void ToggleGodModeButtonClicked(FVRWristMenu wristMenu)//WIP
        {
            //if (GM.CurrentPlayerBody.Hitboxes[0] == true)
            //{
            //    GM.CurrentPlayerBody.DisableHitBoxes();
            //}
            //else
            //{
            //    GM.CurrentPlayerBody.EnableHitBoxes();
            //}

            foreach(var v in GM.CurrentPlayerBody.Hitboxes)
            {
                if (v != null) v.IsActivated = v.IsActivated == true ? false : true;
            }

        }

        //--TNH--//-------//-------//-------//-------//-------

        public static void KillPlayerButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.KillPlayer(true);
        }

        public static void AddTokenButtonClicked(FVRWristMenu wristMenu)
        {
            GM.TNH_Manager.AddTokens(1, true);
        }

        public static void EndHoldButton(FVRWristMenu wristMenu)//WIP
        {
            GM.TNH_Manager.SetPhase_Take();
        }

        //public static void SpawnAmmoReloaderButton(FVRWristMenu wristMenu)
        //{
        //    var headPos = GM.CurrentPlayerBody.Head.position;
        //    GM.TNH_Manager.SpawnAmmoReloader(headPos);
        //}
        //public static void SpawnMagDupeButton(FVRWristMenu wristMenu)
        //{
        //    var headPos = GM.CurrentPlayerBody.Head.position;
        //    GM.TNH_Manager.SpawnMagDuplicator(headPos);
        //}
        //public static void SpawnGunRecylcerButton(FVRWristMenu wristMenu)
        //{
        //    var headPos = GM.CurrentPlayerBody.Head.position;
        //    GM.TNH_Manager.SpawnGunRecycler(headPos);
        //}

        public static void KillPatrolsButtonClicked(FVRWristMenu wristMenu)
        {
            GM.TNH_Manager.KillAllPatrols();
        }

        //--TNH--//-------//-------//-------//-------//-------

        public static void LoadScene(FVRWristMenu wristMenu)
        {
            SteamVR_LoadLevel.Begin("", false, 0.5f, 0f, 0f, 1f);
        }

        private static readonly Type[] WHITE_TYPES =//Stores a list of physical object types for the Gather method
        {
            typeof(FVRFireArm),
            typeof(FVRFireArmMagazine),
            typeof(Speedloader),
            typeof(FVRFireArmRound),
            typeof(FVRFireArmClip),
            typeof(LAPD2019Battery),
            typeof(Molotov),
            typeof(FVRGrenade),
            typeof(PinnedGrenade),
            typeof(FVRKnife),
            typeof(Flashlight),
            typeof(FVRMeleeWeapon),
        };
        //public static IEnumerable<Scene> EnumerateScenes()
        //{
        //    for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        //        yield return SceneManager.GetSceneAt(i);
        //}
        public static Dictionary<string, string> SceneList = new Dictionary<string, string>
        {
            { "MainMenu3" , "Main Menu" },
            { "ArizonaTargets" , "Arizona Range" },
            { "ArizonaTargets_Night" , "Arizona at Night" },
            { "HickockRange" , "Friendly 45 Range" },
            { "IndoorRange" , "Indoor Range" },
            { "ProvingGround" , "Proving Grounds" },
            { "SniperRange" , "Sniper Range" },
            { "TakeAndHold_Lobby_2" , "Take and Hold Lobby" },
        };
        private static float lastMax = 0f;//Stores last maximum health for the toggle 1-hit method 
        
    }
}