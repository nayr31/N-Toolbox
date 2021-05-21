using System;
using System.Collections.Generic;
using System.Linq;
using FistVR;
using Deli.H3VR.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace NToolbox
{
    public static class Actions
    {
        public static void GatherButtonClicked(H3Api api, WristMenuButton caller)
        {
            //Get player pos upon every button press
            Vector3 playerPos = GM.CurrentPlayerBody.Head.position;

            //Whitelisted object gather
            foreach (var physObject in Object.FindObjectsOfType<FVRPhysicalObject>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null && WHITE_TYPES.Contains(physObject.GetType()) && physObject.transform.parent == null)
                    physObject.transform.position = playerPos +
                        Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1.3f, 0.7f, 1.3f)) - new Vector3(0, 0.5f, 0);

            //Gun gather since its missing from the phys object gather
            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.transform.position = playerPos +
                        Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1.3f, 0.7f, 1.3f)) - new Vector3(0, 0.5f, 0);
        }

        public static void ResetTrapsButtonClicked(H3Api api, WristMenuButton caller)
        {
            foreach (var beartrap in Object.FindObjectsOfType<MF2_BearTrap>())
                if (!beartrap.IsHeld && beartrap.QuickbeltSlot == null)
                    beartrap.ForceOpen();
        }

        public static void FreezeFireArmsMeleeButtonClicked(H3Api api, WristMenuButton caller)
        {
            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = true;
            foreach (var physObject in Object.FindObjectsOfType<FVRMeleeWeapon>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = true;
        }

        public static void FreezeAmmoMagButtonClicked(H3Api api, WristMenuButton caller)
        {
            foreach (var obj in Object.FindObjectsOfType<FVRPhysicalObject>())
                if (!obj.IsHeld && obj.QuickbeltSlot == null && (obj.GetType().Equals(typeof(FVRFireArmRound)) || obj.GetType().Equals(typeof(FVRFireArmMagazine))))
                    obj.IsKinematicLocked = true;
        }

        public static void FreezeAttachmentsButtonClicked(H3Api api, WristMenuButton caller)
        {
            foreach (var att in Object.FindObjectsOfType<FVRFireArmAttachment>())
                if (!att.IsHeld && att.QuickbeltSlot == null)
                    att.IsKinematicLocked = true;
        }

        public static void UnFreezeAllClicked(H3Api api, WristMenuButton caller)
        {
            foreach (var physObject in Object.FindObjectsOfType<FVRPhysicalObject>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = false;
            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = false;
        }

        public static void SpawnAmmoPanelButtonClicked(H3Api api, WristMenuButton caller)
        {
            var obj = IM.OD["AmmoPanel"];
            FVRPhysicalObject physObj = Object.Instantiate(obj.GetGameObject()).GetComponent<FVRPhysicalObject>();
            api.WristMenu.m_currentHand.RetrieveObject(physObj);
        }
        
        public static void SpawnAmmoWeenieButtonClicked(H3Api api, WristMenuButton caller)
        {
            var obj = IM.OD["PowerUpMeat_InfiniteAmmo"];
            FVRPhysicalObject physObj = Object.Instantiate(obj.GetGameObject()).GetComponent<FVRPhysicalObject>();
            api.WristMenu.m_currentHand.RetrieveObject(physObj);
        }

        //--Player---------------------------------------------------
        //--Player---------------------------------------------------

        public static void RestoreHPButtonClicked(H3Api api, WristMenuButton caller) => GM.CurrentPlayerBody.ResetHealth();

        public static void Restore10PercentHPButtonClicked(H3Api api, WristMenuButton callerF) => GM.CurrentPlayerBody.HarmPercent(-10f);

        public static void ToggleOneHitButtonClicked(H3Api api, WristMenuButton caller)
        {
            if (GM.CurrentPlayerBody.GetPlayerHealthRaw() != 1)
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

        public static void ToggleGodModeButtonClicked(H3Api api, WristMenuButton caller)
        {
            foreach (var v in GM.CurrentPlayerBody.Hitboxes)
            {
                if (v != null) v.IsActivated = v.IsActivated == true ? false : true;
            }
        }

        public static void ToggleInvisButtonClicked(H3Api api, WristMenuButton caller)
        {
            if (GM.CurrentPlayerBody.GetPlayerIFF() != -1)
            {
                lastIFF = GM.CurrentPlayerBody.GetPlayerIFF();
                GM.CurrentPlayerBody.m_playerIFF = -1;
            }
            else
            {
                GM.CurrentPlayerBody.m_playerIFF = Convert.ToInt32(lastIFF);
            }
        }

        //--TNH---------------------------------------------------------
        //--TNH---------------------------------------------------------

        public static void KillPlayerButtonClicked(H3Api api, WristMenuButton caller) => GM.CurrentPlayerBody.KillPlayer(true);

        public static void AddTokenButtonClicked(H3Api api, WristMenuButton caller) => GM.TNH_Manager.AddTokens(1, true);

        //public static void EndHoldButton(FVRWristMenu wristMenu)//WIP - tnhtweaker issues
        //{
        //    GM.TNH_Manager.SetPhase_Take();
        //}

        public static void SpawnAmmoReloaderButton(H3Api api, WristMenuButton caller)
        {
            var spawnPos = GM.CurrentPlayerBody.Torso;
            spawnPos.rotation = Quaternion.identity;
            spawnPos.position = new Vector3(spawnPos.position.x, spawnPos.position.y - 1.5f, spawnPos.position.z);
            GM.TNH_Manager.SpawnAmmoReloader(spawnPos);
        }
        public static void SpawnMagDupeButton(H3Api api, WristMenuButton caller)
        {
            var spawnPos = GM.CurrentPlayerBody.Torso;
            spawnPos.rotation = Quaternion.identity;
            spawnPos.position = new Vector3(spawnPos.position.x, spawnPos.position.y - 1.5f, spawnPos.position.z);
            GM.TNH_Manager.SpawnMagDuplicator(spawnPos);
        }
        public static void SpawnGunRecylcerButton(H3Api api, WristMenuButton caller)
        {
            var spawnPos = GM.CurrentPlayerBody.Torso;
            spawnPos.rotation = Quaternion.identity;
            spawnPos.position = new Vector3(spawnPos.position.x, spawnPos.position.y - 1.5f, spawnPos.position.z);
            GM.TNH_Manager.SpawnGunRecycler(spawnPos);
        }

        public static void KillPatrolsButtonClicked(H3Api api, WristMenuButton caller)
        {
            GM.TNH_Manager.KillAllPatrols();
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
            typeof(FVRFireArmAttachment),
        };
        public static Dictionary<string, string> SceneList = new Dictionary<string, string>
        {
            { "MainMenu3" , "Main Menu" },
            { "ArizonaTargets" , "Arizona Range" },
            { "ArizonaTargets_Night" , "Arizona at Night" },
            { "HickockRangeNew" , "Friendly 45 Range" },//should probably test this new name lmao
            { "IndoorRange" , "Indoor Range" },
            { "ProvingGround" , "Proving Grounds" },
            { "SniperRange" , "Sniper Range" },
            { "TakeAndHold_Lobby_2" , "Take and Hold Lobby" },
        };
        public static void Empty(H3Api api, WristMenuButton caller) { }
        private static float lastMax = 0f;//Stores last maximum health for the toggle 1-hit method 
        private static float lastIFF = 0f;//Store last IFF for use in toggle invis method
    }
}