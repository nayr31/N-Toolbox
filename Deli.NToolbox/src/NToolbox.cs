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
            WristMenu.RegisterWristMenuButton("Gather Items", GatherButtonClicked);
            WristMenu.RegisterWristMenuButton("Reset Traps", ResetTrapsButtonClicked);
            WristMenu.RegisterWristMenuButton("Restore HP", RestoreHPButtonClicked);
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
                if(!beartrap.IsHeld && beartrap.QuickbeltSlot == null)
                    beartrap.ForceOpen();
        }

        private void RestoreHPButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.ResetHealth();
        }

        private void Restore10PercentHPButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.HarmPercent(-10f);
        }

        private void EnableOneHitButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.SetHealthThreshold(1f);
        }

        private void ToggleGodModeButtonClicked(FVRWristMenu wristMenu)
        {
            if(GM.CurrentPlayerBody.Hitboxes[0] == true)
            {
                GM.CurrentPlayerBody.DisableHitBoxes();
            }
            else
            {
                GM.CurrentPlayerBody.EnableHitBoxes();
            }
        }

        private void BlindButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.BlindPlayer(10000f);
        }

        private void KillPlayerButtonClicked(FVRWristMenu wristMenu)
        {
            GM.CurrentPlayerBody.KillPlayer(true);
        }

        private void AddTokenButton(FVRWristMenu wristMenu)
        {
            GM.TNH_Manager.AddTokens(1, true);
        }

        //private void EndHoldButton(FVRWristMenu wristMenu)
        //{
        //    GM.TNH_Manager.
        //}

        private void SpawnAmmoReloaderButton(FVRWristMenu wristMenu)
        {
            var headPos = GM.CurrentPlayerBody.Head.position;
            //headPos.Set(headPos.x, headPos.y-0.5f, headPos.z);
            GM.TNH_Manager.SpawnAmmoReloader(headPos);
        }

        static readonly Type[] whiteTypes =
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
    }
}