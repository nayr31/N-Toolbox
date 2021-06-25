using System;
using System.Collections.Generic;
using System.Linq;
using FistVR;
using UnityEngine;
using UnityEngine.UI;
using Sodalite.Api;
using Object = UnityEngine.Object;
using Sodalite.UiWidgets;
using Sodalite;

namespace NToolbox
{
    public class Actions
    {
        private static float lastMax = 0f;//Stores last maximum health for the toggle 1-hit method 
        private static float lastIFF = 0f;//Store last IFF for use in toggle invis method
        private static bool isMortal = true;
        
        private static readonly Type[] TYPE_WHITELIST =//Stores a list of physical object types for the Gather method
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
        

        public static void GatherButtonClicked()
        {
            //Get player pos upon every button press
            Vector3 playerPos = GM.CurrentPlayerBody.Head.position;

            //Whitelisted object gather
            foreach (var physObject in Object.FindObjectsOfType<FVRPhysicalObject>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null && TYPE_WHITELIST.Contains(physObject.GetType()) && physObject.transform.parent == null)
                    physObject.transform.position = playerPos +
                        Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1.3f, 0.7f, 1.3f)) - new Vector3(0, 0.5f, 0);

            //Gun gather since its missing from the phys object gather
            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.transform.position = playerPos +
                        Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1.3f, 0.7f, 1.3f)) - new Vector3(0, 0.5f, 0);
        }

        public static void DeleteButtonClicked()
        {
            foreach (var physObject in Object.FindObjectsOfType<FVRPhysicalObject>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null && physObject.transform.parent == null)
                    Object.Destroy(physObject.gameObject);

            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    Object.Destroy(physObject.gameObject);
        }

        public static void ResetTrapsButtonClicked()
        {
            foreach (var beartrap in Object.FindObjectsOfType<MF2_BearTrap>())
                if (!beartrap.IsHeld && beartrap.QuickbeltSlot == null)
                    beartrap.ForceOpen();
        }

        public static void FreezeFireArmsMeleeButtonClicked()
        {
            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)        
                    physObject.IsKinematicLocked = true;
            foreach (var physObject in Object.FindObjectsOfType<FVRMeleeWeapon>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = true;
        }

        public static void FreezeAmmoMagButtonClicked()
        {
            foreach (var obj in Object.FindObjectsOfType<FVRPhysicalObject>())
                if (!obj.IsHeld && obj.QuickbeltSlot == null && (obj.GetType() == typeof(FVRFireArmRound) || obj.GetType() == typeof(FVRFireArmMagazine)))
                    obj.IsKinematicLocked = true;
        }

        public static void FreezeAttachmentsButtonClicked()
        {
            foreach (var att in Object.FindObjectsOfType<FVRFireArmAttachment>())
                if (!att.IsHeld && att.QuickbeltSlot == null)
                    att.IsKinematicLocked = true;
        }

        public static void UnFreezeAllClicked()
        {
            foreach (var physObject in Object.FindObjectsOfType<FVRPhysicalObject>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = false;
            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = false;
        }

        public static void SpawnAmmoPanelButtonClicked() => SpawnItemByItemIdLeftHand("AmmoPanel", true);

        public static void SpawnItemByItemIdLeftHand(String itemId, bool kinLoc)
        {
            var obj = IM.OD[itemId];
            FVRPhysicalObject physObj = Object.Instantiate(obj.GetGameObject()).GetComponent<FVRPhysicalObject>();
            physObj.transform.position = GM.CurrentPlayerBody.LeftHand.transform.position;
            if(kinLoc) physObj.SetIsKinematicLocked(true);

        }

        public static void DeleteQuickbelt()
        {
            foreach (var physObject in Object.FindObjectsOfType<FVRPhysicalObject>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot != null && physObject.transform.parent == null)
                    Object.Destroy(physObject.gameObject);

            foreach (var physObject in Object.FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot != null)
                    Object.Destroy(physObject.gameObject);
        }

        //--Player---------------------------------------------------
        //--Player---------------------------------------------------

        public static void RestoreHPButtonClicked() => GM.CurrentPlayerBody.ResetHealth();

        public static void ToggleOneHitButtonClicked()
        {
            //IM.OD.Remove("That ugly schristmas suppressor");
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

        public static void ToggleGodModeButtonClicked()
        {
            //default hitboxes are true
            foreach (var v in GM.CurrentPlayerBody.Hitboxes)
            {
                if (v != null)  v.IsActivated = !isMortal;
            }
            isMortal = !isMortal;
        }

        public static void ToggleInvisButtonClicked()//-1 doesnt work lmao
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

        public static void ToggleControllerGeo() => 
            GM.Options.QuickbeltOptions.HideControllerGeoWhenObjectHeld = !GM.Options.QuickbeltOptions.HideControllerGeoWhenObjectHeld;

        public static void ToggleHealthBar() =>
            GM.CurrentPlayerBody.HealthBar.gameObject.SetActive(!GM.CurrentPlayerBody.HealthBar.gameObject.activeSelf);

        public static void HandCollision()
        {
            var collider = GM.CurrentPlayerBody.LeftHand.gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(0.5f, 0.5f, 0.5f);
            GM.CurrentPlayerBody.LeftHand.gameObject.SetActive(true);
        }

        //--TNH---------------------------------------------------------
        //--TNH---------------------------------------------------------

        public static void KillPlayerButtonClicked() => GM.CurrentPlayerBody.KillPlayer(true);

        public static void AddTokenButtonClicked() => GM.TNH_Manager.AddTokens(1, true);

        private static void SpawnButton(Func<Transform, GameObject> spawnerFunc)
        {
            var spawnPos = GM.CurrentPlayerBody.Torso;
            spawnPos.rotation = Quaternion.identity;
            spawnPos.position = new Vector3(spawnPos.position.x, spawnPos.position.y - 1.5f, spawnPos.position.z);
            spawnerFunc.Invoke(spawnPos);
        }

        public static void SpawnAmmoReloaderButton() => SpawnButton(GM.TNH_Manager.SpawnAmmoReloader);
        public static void SpawnMagDupeButton() => SpawnButton(GM.TNH_Manager.SpawnMagDuplicator);
        public static void SpawnGunRecyclerButton() => SpawnButton(GM.TNH_Manager.SpawnGunRecycler);

        public static void KillPatrolsButtonClicked() => GM.TNH_Manager.KillAllPatrols();
        
        public static void Empty() {  }
    }
}