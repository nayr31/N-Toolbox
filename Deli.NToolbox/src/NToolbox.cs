using FistVR;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using Sodalite.Api;
using UnityEngine.SceneManagement;
using Gizmos = Popcron.Gizmos;

namespace NToolbox
{
    [BepInPlugin(Common.PluginInfo.GUID, Common.PluginInfo.NAME, Common.PluginInfo.VERSION)]
    [BepInDependency(Common.PluginInfo.SODALITE_GUID)]
    public class NToolbox : BaseUnityPlugin
    {
        public static ObjectIDList ObjectIDs { get; private set; }
        public readonly ConfigEntry<bool> LoadWristMenu;
        public readonly ConfigEntry<bool> EnableHandColliders;
        private FVRViveHand LeftHandComp = new FVRViveHand();
        private FVRViveHand RightHandComp = new FVRViveHand();

        public NToolbox() 
        {
            //Set config option
            LoadWristMenu = Config.Bind("WristMenu Options", "LoadWristMenu", false, "If set to true, will load all wristmenu actions. I don't recommend using this, please don't.");
            EnableHandColliders = Config.Bind("Other Options", "EnableHandColliders", false, "If set to true, will automatically add collision to the player's hands.");
        }

        public void Start()
        {
            //Diable TnH leaderboard scoring
            LeaderboardAPI.GetLeaderboardDisableLock();

            ObjectIDs = new ObjectIDList(Common.OBJECT_ID_LIST_FILENAME);
            
            NPanel nPanel = new NPanel();
            WristMenuAPI.Buttons.Add(new WristMenuButton("NTool Panel", nPanel.Spawn));

            if (LoadWristMenu.Value) nPanel.LoadWristMenu();

            SceneManager.sceneLoaded += SceneLoadHook;
        }

        public void SceneLoadHook(Scene scene, LoadSceneMode mode)
        {
            if (EnableHandColliders.Value) Actions.ToggleHandCollision();

            LeftHandComp = GM.CurrentPlayerBody.LeftHand.GetComponent<FVRViveHand>();
            RightHandComp = GM.CurrentPlayerBody.RightHand.GetComponent<FVRViveHand>();
        }

        private void Update()
        {
            //If the collider object has a parent (ie, the collider is attached to the hands and enabled
            if (Actions.LeftCollider.gameObject.transform.parent != null)
            {
                //If the collider is currently active
                if (Actions.LeftCollider.activeSelf)
                    //Display the collision sphere
                    Gizmos.Sphere(GM.CurrentPlayerBody.LeftHand.position, Actions.HAND_SIZE, Color.red);

                //If the hands are holding something
                if (LeftHandComp.m_state.Equals(FVRViveHand.HandState.GripInteracting))
                    //Disable hand collision
                    Actions.LeftCollider.SetActive(false);
                //If the hands were holding something
                else if (!Actions.LeftCollider.activeSelf)
                    //Enable the collider
                    Actions.LeftCollider.SetActive(true);
            }
                
            if (Actions.RightCollider.gameObject.transform.parent != null)
            {
                if (Actions.RightCollider.activeSelf)
                    Gizmos.Sphere(GM.CurrentPlayerBody.RightHand.position, Actions.HAND_SIZE, Color.blue);

                if (RightHandComp.m_state.Equals(FVRViveHand.HandState.GripInteracting))
                    Actions.RightCollider.SetActive(false);
                else if (!Actions.RightCollider.activeSelf)
                    Actions.RightCollider.SetActive(true);
            }
        }
    }
}