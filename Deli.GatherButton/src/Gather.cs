using Deli.Immediate;
using Deli.Setup;
using Deli.H3VR.Api;
using FistVR;
using UnityEngine;
using System;

namespace Deli.GatherButton
{
    public class Gather : DeliBehaviour
    {
        public Gather()
        {
            WristMenu.RegisterWristMenuButton("Gather Items", WristMenuButtonClicked);
        }

        private void WristMenuButtonClicked(FVRWristMenu wristMenu)
        {
            //Get array of objects 
            FVRPhysicalObject[] objectArray = UnityEngine.Object.FindObjectsOfType<FVRPhysicalObject>();
            Vector3 playerPos = GM.CurrentPlayerBody.Head.position;
            //string[] whitelist =
            //{
            //    "FVRFireArmMagazine",
            //    "FVRFireArmRound",
            //    "FVRFireArmClip",
            //    "FVRFireArm",
            //    "LAPD2019Battery",
            //    "Molotov",
            //    "Flashlight",
            //    "FVRGrenade",
            //    "FVRKnife",
            //    "Speedloader",
            //};


            //foreach(var gatheredObject in FindObjectsOfType<FVRPhysicalObject>())
            //{
            //    if (refList(whitelist, gatheredObject.GetType().Name))
            //        gatheredObject.transform.position = playerPos;
            //}

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRFireArm>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    v.transform.position = playerPos;

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRFireArmMagazine>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    v.transform.position = playerPos;

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRFireArmRound>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    v.transform.position = playerPos;

            foreach (var v in UnityEngine.Object.FindObjectsOfType<Speedloader>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    v.transform.position = playerPos;

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRFireArmClip>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    v.transform.position = playerPos;

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRMeleeWeapon>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    v.transform.position = playerPos;

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRGrenade>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    v.transform.position = playerPos;

            foreach (var v in UnityEngine.Object.FindObjectsOfType<PinnedGrenade>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    v.transform.position = playerPos;
        }

        //private bool refList(string[] list, string regex)
        //{
        //    foreach (string v in list)
        //        if (v.Contains(regex))
        //            return true;
        //    return false;
        //}
    }
}
//Logger.LogMessage("Gathering items...");
            //try
            //{
            //    for (int k = objectArray.Length - 1; k >= 0; k--)
            //    {
            //        if (!objectArray[k].IsHeld && objectArray[k].QuickbeltSlot == null)
            //        {
            //            //UnityEngine.Object.Instantiate(magArray[k].gameObject, playerPos, Quaternion.identity);
            //            objectArray[k].transform.position = playerPos;
            //        }
            //    }
            //    Logger.LogMessage("Gathering completed");
            //}
            //catch (Exception e)
            //{
            //    Logger.LogMessage("Something happened while gatherings:\n-- " + e);
            //}