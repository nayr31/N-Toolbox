using Deli.Immediate;
using Deli.Setup;
using Deli.H3VR.Api;
using Deli.H3VR;
using FistVR;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deli.NToolbox
{
    public class NToolbox : DeliBehaviour
    {
        public NToolbox()
        {
            //Item interaction stuff
            WristMenu.RegisterWristMenuButton("Gather Items", GatherButtonClicked);
            WristMenu.RegisterWristMenuButton("Reset Traps", ResetTrapsButtonClicked);
            WristMenu.RegisterWristMenuButton("Freeze Guns", FreezeFireArmsButtonClicked);
            WristMenu.RegisterWristMenuButton("Unfreeze Guns", UnFreezeFireArmsButtonClicked);

            //Player body stuff
            WristMenu.RegisterWristMenuButton("Restore Full", RestoreHPButtonClicked);
            WristMenu.RegisterWristMenuButton("Restore 10%", Restore10PercentHPButtonClicked);
            WristMenu.RegisterWristMenuButton("Toggle 1-hit", ToggleOneHitButtonClicked);
            WristMenu.RegisterWristMenuButton("Toggle God Mode", ToggleGodModeButtonClicked);
            WristMenu.RegisterWristMenuButton("Kill yourself", KillPlayerButtonClicked);
            //WristMenu.RegisterWristMenuButton("Blind yourself", BlindButtonClicked);

            //Tnh stuff
            WristMenu.RegisterWristMenuButton("Add token", AddTokenButton);
            WristMenu.RegisterWristMenuButton("End hold", EndHoldButton);
            WristMenu.RegisterWristMenuButton("Kill patrols", KillPatrolsButton);
            
            //Quick spawn stuff
            WristMenu.RegisterWristMenuButton("TnH Lobby", TnHLobbyButton);
            WristMenu.RegisterWristMenuButton("Indoor Range", IndoorRangeButton);
        }

        private void GatherButtonClicked(FVRWristMenu wristMenu)
        {
            //Get player pos upon every button press
            Vector3 playerPos = GM.CurrentPlayerBody.Head.position;

            //Whitelisted object gather
            foreach (var physObject in FindObjectsOfType<FVRPhysicalObject>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null && whiteTypes.Contains(physObject.GetType()) && physObject.transform.parent == null)
                    physObject.transform.position = playerPos + 
                        Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1.3f, 0.7f, 1.3f)) - new Vector3(0, 0.5f ,0);

            //Gun gather since its missing from the phys object gather
            foreach (var physObject in FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.transform.position = playerPos +
                        Vector3.Scale(UnityEngine.Random.insideUnitSphere, new Vector3(1.3f, 0.7f, 1.3f)) - new Vector3(0, 0.5f, 0);
        }

        private void ResetTrapsButtonClicked(FVRWristMenu wristMenu)
        {
            foreach (var beartrap in FindObjectsOfType<MF2_BearTrap>())
                if (!beartrap.IsHeld && beartrap.QuickbeltSlot == null)
                    beartrap.ForceOpen();
        }

        private void FreezeFireArmsButtonClicked(FVRWristMenu wristMenu)
        {
            foreach (var physObject in FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = false;
        }

        private void UnFreezeFireArmsButtonClicked(FVRWristMenu wristMenu)
        {
            foreach (var physObject in FindObjectsOfType<FVRFireArm>())
                if (!physObject.IsHeld && physObject.QuickbeltSlot == null)
                    physObject.IsKinematicLocked = false;
        }

        //player

        private void RestoreHPButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.ResetHealth();
        }

        private void Restore10PercentHPButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.HarmPercent(-10f);
        }

        private void ToggleOneHitButtonClicked(FVRWristMenu wristMenu)
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

        private void ToggleGodModeButtonClicked(FVRWristMenu wristMenu)
        {
            //BUG - Seems to not re-enable hitboxes
            if(GM.CurrentPlayerBody.Hitboxes[0] == true)
            {
                GM.CurrentPlayerBody.DisableHitBoxes();
            }
            else
            {
                GM.CurrentPlayerBody.EnableHitBoxes();
            }
        }

        //private void BlindButtonClicked(FVRWristMenu wristMenu)
        //{
        //    GM.CurrentPlayerBody.BlindPlayer(50000f);
        //}


        //--TNH--//-------//-------//-------//-------//-------

        private void KillPlayerButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.KillPlayer(true);
        }

        private void AddTokenButton(FVRWristMenu wristMenu)
        {
            GM.TNH_Manager.AddTokens(1, true);
        }

        private void EndHoldButton(FVRWristMenu wristMenu)
        {
            //BUG - You lose
            GM.TNH_Manager.SetPhase_Completed();
        }

        //private void SpawnAmmoReloaderButton(FVRWristMenu wristMenu)
        //{
        //    var headPos = GM.CurrentPlayerBody.Head.position;
        //    GM.TNH_Manager.SpawnAmmoReloader(headPos);
        //}
        //private void SpawnMagDupeButton(FVRWristMenu wristMenu)
        //{
        //    var headPos = GM.CurrentPlayerBody.Head.position;
        //    GM.TNH_Manager.SpawnMagDuplicator(headPos);
        //}
        //private void SpawnGunRecylcerButton(FVRWristMenu wristMenu)
        //{
        //    var headPos = GM.CurrentPlayerBody.Head.position;
        //    GM.TNH_Manager.SpawnGunRecycler(headPos);
        //}

        private void KillPatrolsButton(FVRWristMenu wristMenu)
        {
            GM.TNH_Manager.KillAllPatrols();
        }

        //--TNH--//-------//-------//-------//-------//-------

        private void TnHLobbyButton(FVRWristMenu wristMenu)
        {
            SteamVR_LoadLevel.Begin("TakeAndHold_Lobby_2", false, 0.5f, 0f, 0f, 1f);
        }

        private void IndoorRangeButton(FVRWristMenu wristMenu)
        {
            SteamVR_LoadLevel.Begin("IndoorRange", false, 0.5f, 0f, 0f, 1f);
        }

        static readonly Type[] whiteTypes =//Stores a list of physical object types for the Gather method
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
        float lastMax = 0f;//Stores last maximum health for the toggle 1-hit method 
    }
}