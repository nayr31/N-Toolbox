using FistVR;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using Sodalite.Api;
using UnityEngine.SceneManagement;
using Gizmos = Popcron.Gizmos;
using System;
using BepInEx.Logging;

namespace NToolbox
{
    [BepInPlugin(Common.PluginInfo.GUID, Common.PluginInfo.NAME, Common.PluginInfo.VERSION)]
    [BepInDependency(Common.PluginInfo.SODALITE_GUID)]
    public class NToolbox : BaseUnityPlugin
    {
        public static ObjectIDList ObjectIDs { get; private set; }
        public readonly ConfigEntry<bool> LoadWristMenu;
        //public static ConfigEntry<bool> EnableHandColliders;
        public static ConfigEntry<bool> EnableDebugSpheres;
        private FVRViveHand LeftHandComp = new FVRViveHand();
        private FVRViveHand RightHandComp = new FVRViveHand();

        public NToolbox() 
        {
            //Set config option
            LoadWristMenu = Config.Bind("WristMenu Options", "LoadWristMenu", false, "If set to true, will load all wristmenu actions. I don't recommend using this, please don't.");
            //EnableHandColliders = Config.Bind("Other Options", "EnableHandColliders", false, "If set to true, will automatically add collision to the player's hands.");
            EnableDebugSpheres = Config.Bind("Other Options", "EnableDebugSpheres", true, "If set to true, will show colored spheres where the colliders are when hands are enabled.");
        }

        public void Start()
        {
            LeaderboardAPI.GetLeaderboardDisableLock();

            //ObjectIDs = new ObjectIDList(Common.OBJECT_ID_LIST_FILENAME);
            
            NPanel nPanel = new NPanel();
            WristMenuAPI.Buttons.Add(new WristMenuButton("NTool Panel", nPanel.Spawn));

            if (LoadWristMenu.Value) nPanel.LoadWristMenu();

            SceneManager.sceneLoaded += SceneLoadHook;

            ResetHandObjects();
        }

        public void SceneLoadHook(Scene scene, LoadSceneMode mode)
        {
            ResetHandObjects();

            Debug.Log("NToolbox is printing scene data...");
            Debug.Log(scene.name);
        }

        private void Update()
        {
            try
            {
                //Hand collider interaction
                //Disable/enable based on Grip interaction
                if (LeftHandComp.m_state.Equals(FVRViveHand.HandState.GripInteracting))
                    Actions.LeftCollider.SetActive(false);
                else if (!Actions.LeftCollider.activeSelf)
                    Actions.LeftCollider.SetActive(true);

                if (RightHandComp.m_state.Equals(FVRViveHand.HandState.GripInteracting))
                    Actions.RightCollider.SetActive(false);
                else if (!Actions.RightCollider.activeSelf)
                    Actions.RightCollider.SetActive(true);

                //Show/hide spheres
                if (Actions.LeftCollider.activeSelf && Actions.LeftCollider.transform.parent != null && EnableDebugSpheres.Value)
                    Gizmos.Sphere(GM.CurrentPlayerBody.LeftHand.position, Actions.HAND_SIZE, Color.red);
                if (Actions.RightCollider.activeSelf && Actions.RightCollider.transform.parent != null && EnableDebugSpheres.Value)
                    Gizmos.Sphere(GM.CurrentPlayerBody.RightHand.position, Actions.HAND_SIZE, Color.blue);
            } catch (NullReferenceException e)
            {
                Debug.Log("---NToolbox caught null reference exception---");
                Debug.Log("(If you are seeing this, report it to the mod author)");
                Debug.Log("LeftHandComp: " + LeftHandComp != null ? "not null" : "null");
                Debug.Log("Actions.LeftCollider: " + Actions.LeftCollider != null ? "not null" : "null");
                Debug.Log("Error: " + e);
                Debug.Log("Attempting to reset...");
                ResetHandObjects();
            }
        }

        private void ResetHandObjects()
        {
            LeftHandComp = GM.CurrentPlayerBody.LeftHand.GetComponent<FVRViveHand>();
            RightHandComp = GM.CurrentPlayerBody.RightHand.GetComponent<FVRViveHand>();
            Actions.SetColliderObjects();
        }
    }
}