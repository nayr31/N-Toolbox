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
            Logger.LogMessage("Gathering items...");
            Vector3 playerPos = GM.CurrentPlayerBody.Head.position;
            bool cont = false;//continuing variable to skip object deletion upon not gathering
            //var hand = wristMenu.Hands[0].TouchSphere

            //Guns
            //FVRFireArm[] array = UnityEngine.Object.FindObjectsOfType<FVRFireArm>();
            //for (int i = array.Length - 1; i >= 0; i--)	{
            //	if (!array[i].IsHeld && array[i].QuickbeltSlot == null)	{
            //		//Action
            //		Logger.LogMessage("Guns...");
            //	}
            //}

            //Sosiggun?
            //SosigWeapon[] array2 = UnityEngine.Object.FindObjectsOfType<SosigWeapon>();
            //for (int j = array2.Length - 1; j >= 0; j--)
            //{
            //	if (!array2[j].O.IsHeld && array2[j].O.QuickbeltSlot == null && !array2[j].IsHeldByBot && !array2[j].IsInBotInventory)
            //	{
            //		UnityEngine.Object.Destroy(array2[j].gameObject);
            //	}
            //}

            //Melee weapons
            //FVRMeleeWeapon[] array3 = UnityEngine.Object.FindObjectsOfType<FVRMeleeWeapon>();
            //for (int k = array3.Length - 1; k >= 0; k--) {
            //	if (!array3[k].IsHeld && array3[k].QuickbeltSlot == null) {
            //		UnityEngine.Object.Destroy(array3[k].gameObject);
            //	}
            //}

            //Magazines
            Logger.LogMessage("Mags...");
            cont = false;
            FVRFireArmMagazine[] magArray = UnityEngine.Object.FindObjectsOfType<FVRFireArmMagazine>();
            //--Spawn
            try
            {
                for (int k = magArray.Length - 1; k >= 0; k--)
                {
                    if (!magArray[k].IsHeld && magArray[k].QuickbeltSlot == null)
                    {
                        UnityEngine.Object.Instantiate(magArray[k].gameObject, playerPos, Quaternion.identity);
                    }
                }
                Logger.LogMessage("Mag gather done");
                cont = true;
            }
            catch (Exception e)
            {
                Logger.LogMessage("Something happened when gathering mags:\n-- " + e);
            }

            //--Destroy
            if (cont)
            {
                try
                {
                    for (int k = magArray.Length - 1; k >= 0; k--)
                    {
                        UnityEngine.Object.Destroy(magArray[k].gameObject);
                    }
                    Logger.LogMessage("Mag destroy done");
                }
                catch (Exception e)
                {
                    Logger.LogMessage("Something happened when deleting excess mags:\n-- " + e);
                }
            }
        }
    }
}