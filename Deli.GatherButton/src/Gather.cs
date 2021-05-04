using Deli.Immediate;
using Deli.Setup;
using Deli.H3VR.Api;
using FistVR;
using UnityEngine;
using System;
using System.Collections.Generic;

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
            //Array of all objects
            FVRPhysicalObject[] objectArray = UnityEngine.Object.FindObjectsOfType<FVRPhysicalObject>();
            //Current player position to root random area
            Vector3 playerPos = GM.CurrentPlayerBody.Head.position;
            //Moving vector between item spawns
            Vector3 transformPos = playerPos;
            //Whitelist of item types
            Type[] whiteTypes =
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

            //Iterate through all objetcs, and transform them to an area around the player
            foreach(var v in objectArray)
                if(!v.IsHeld && v.QuickbeltSlot == null && refList(whiteTypes, v.GetType()))
                    transformPos = action(v, transformPos, playerPos);

            
            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRFireArm>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    transformPos = action(v, transformPos, playerPos);
            /*
            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRFireArmMagazine>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    transformPos = action(v, transformPos, playerPos);

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRFireArmRound>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    transformPos = action(v, transformPos, playerPos);

            foreach (var v in UnityEngine.Object.FindObjectsOfType<Speedloader>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    transformPos = action(v, transformPos, playerPos);

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRFireArmClip>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    transformPos = action(v, transformPos, playerPos);

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRMeleeWeapon>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    transformPos = action(v, transformPos, playerPos);

            foreach (var v in UnityEngine.Object.FindObjectsOfType<FVRGrenade>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    transformPos = action(v, transformPos, playerPos);

            foreach (var v in UnityEngine.Object.FindObjectsOfType<PinnedGrenade>())
                if (!v.IsHeld && v.QuickbeltSlot == null)
                    transformPos = action(v, transformPos, playerPos);*/
        }

        //Preforms the move action on the object, then returns an updated position for a random space around the player
        private Vector3 action(FVRPhysicalObject v, Vector3 transformPos, Vector3 playerPos)
        {
            v.transform.position = transformPos;
            return rotTrans(transformPos, playerPos);
        }

        //Changes a vector to a random space around a second vector
        private Vector3 rotTrans(Vector3 transformPos, Vector3 playerPos)
        {
            var tempVect3 = transformPos;
            var offsetX = UnityEngine.Random.Range(-1f, 1f);
            var offsetY = UnityEngine.Random.Range(-1f, 0f);//up-down
            var offsetZ = UnityEngine.Random.Range(-1f, 1f);

            tempVect3 = new Vector3(playerPos.x + offsetX, playerPos.y + offsetY, playerPos.z + offsetZ);

            return tempVect3;
        }


        //private bool refList(string[] list, string regex)
        //{
        //    foreach (string v in list)
        //        if (v.Contains(regex))
        //            return true;
        //    return false;
        //}

        //Compares a list of types to see if a type is contained within it
        private bool refList(Type[] list, Type regex)
        {
            foreach (Type v in list)
                if (v.Equals(regex))
                    return true;
            return false;
        }
    }


}
//Whitelist code:



//foreach(var gatheredObject in FindObjectsOfType<FVRPhysicalObject>())
//{
//    if (refList(whitelist, gatheredObject.GetType().Name))
//        gatheredObject.transform.position = playerPos;
//}