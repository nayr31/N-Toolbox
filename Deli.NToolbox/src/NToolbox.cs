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
        public static ConfigEntry<bool> EnableDebugSpheres;
        private FVRViveHand LeftHandComp = new FVRViveHand();
        private FVRViveHand RightHandComp = new FVRViveHand();

        public NToolbox() 
        {
            //Set config option
            LoadWristMenu = Config.Bind("WristMenu Options", "LoadWristMenu", false, "If set to true, will load all wristmenu actions. I don't recommend using this, please don't.");
            EnableHandColliders = Config.Bind("Other Options", "EnableHandColliders", false, "If set to true, will automatically add collision to the player's hands.");
            EnableDebugSpheres = Config.Bind("Other Options", "EnableDebugSpheres", true, "If set to true, will show colored spheres where the colliders are when hands are enabled.");
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

            //loading the game at the start doesnt count as loading a scene??
            GetHandComps();
        }

        private void GetHandComps()
        {
            LeftHandComp = GM.CurrentPlayerBody.LeftHand.GetComponent<FVRViveHand>();
            RightHandComp = GM.CurrentPlayerBody.RightHand.GetComponent<FVRViveHand>();
        }

        public void SceneLoadHook(Scene scene, LoadSceneMode mode)
        {
            GetHandComps();

            if (EnableHandColliders.Value) Actions.ToggleHandCollision();

            if (Actions.LeftCollider == null || Actions.RightCollider == null)
                Actions.SetColliderObjects();
        }

        private void Update()
        {
            //updateHandInteractionStateAndDisplay(Actions.LeftCollider, LeftHandComp, GM.CurrentPlayerBody.LeftHand.position, Color.red);
            //updateHandInteractionStateAndDisplay(Actions.RightCollider, RightHandComp, GM.CurrentPlayerBody.RightHand.position, Color.blue);

            //If the collider object has a parent (ie, the collider is attached to the hands and enabled
            if (Actions.LeftCollider.gameObject.transform.parent != null)
            {
                //If the collider is currently active
                if (Actions.LeftCollider.activeSelf)
                    //Display the collision sphere
                    if (EnableDebugSpheres.Value)
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
                    if (EnableDebugSpheres.Value)
                        Gizmos.Sphere(GM.CurrentPlayerBody.RightHand.position, Actions.HAND_SIZE, Color.blue);

                if (RightHandComp.m_state.Equals(FVRViveHand.HandState.GripInteracting))
                    Actions.RightCollider.SetActive(false);
                else if (!Actions.RightCollider.activeSelf)
                    Actions.RightCollider.SetActive(true);
            }
        }

        //private void updateHandInteractionStateAndDisplay(GameObject sideCollider, FVRViveHand sideComponent, Vector3 handPos, Color color)
        //{
        //    if (sideCollider.gameObject.transform.parent != null)
        //    {
        //        //If the collider is currently active
        //        if (sideCollider.activeSelf)
        //            //Display the collision sphere
        //            if (EnableDebugSpheres.Value)
        //                Gizmos.Sphere(handPos, Actions.HAND_SIZE, color);

        //        //If the hands are holding something
        //        if (sideComponent.m_state.Equals(FVRViveHand.HandState.GripInteracting))
        //            //Disable hand collision
        //            sideCollider.SetActive(false);
        //        //If the hands were holding something
        //        else if (!sideCollider.activeSelf)
        //            //Enable the collider
        //            sideCollider.SetActive(true);
        //    }
        //}
    }
}