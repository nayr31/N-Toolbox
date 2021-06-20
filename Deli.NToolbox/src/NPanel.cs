using FistVR;
using Sodalite;
using Sodalite.Api;
using Sodalite.UiWidgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BepInEx;
using BepInEx.Configuration;
using System.Reflection;
using Sodalite.Utilities;

namespace NToolbox.src
{
    class NPanel
    {
        private LockablePanel _NPanel;
        GridLayoutWidget menu;
        GridLayoutWidget itemTools;
        GridLayoutWidget playerTools;
        GridLayoutWidget tnhTools;
        GridLayoutWidget sceneTools;


        public readonly Dictionary<string, Action> ItemToolsDict = new()
        {
            //{ "Back", Actions.Empty },

            //Item interactions
            { "Gather Items", Actions.GatherButtonClicked },
            { "Reset Traps", Actions.ResetTrapsButtonClicked },
            { "Freeze Guns/Melee", Actions.FreezeFireArmsMeleeButtonClicked },
            { "Freeze Ammo/Mags", Actions.FreezeAmmoMagButtonClicked },
            { "Freeze Attachments", Actions.FreezeAttachmentsButtonClicked },
            { "Unfreeze All", Actions.UnFreezeAllClicked },
            { "Ammo Panel", Actions.SpawnAmmoPanelButtonClicked },
            { "Ammo Weenie", Actions.SpawnAmmoWeenieButtonClicked },
            //trash bin
            //quickbelt fast?
            //sosig spawner
            //delete all objects
            //toggle controller geo
        };

        public readonly Dictionary<string, Action> PlayerToolsDict = new()
        {
            //{ Common.SEPARATOR, Actions.Empty },

            //Player body interactions
            { "Restore Full", Actions.RestoreHPButtonClicked },
            { "Restore 10%", Actions.Restore10PercentHPButtonClicked },//testing
            { "Toggle 1-hit", Actions.ToggleOneHitButtonClicked },
            { "Toggle God Mode", Actions.ToggleGodModeButtonClicked },//doesn't work against melee? its toggling hitboxes, so maybe some are disabled?
            { "Kill yourself", Actions.KillPlayerButtonClicked },
            //{ "Toggle Invisibility", Actions.ToggleInvisButtonClicked },//Broken? Test for flat IFF = -1 to see if the check is broken
        };

        public readonly Dictionary<string, Action> TnHToolsDict = new()
        {
            //{ Common.SEPARATOR, Actions.Empty },

            //Take and Hold interactions
            { "Add token", Actions.AddTokenButtonClicked },
            //{ "End hold", Actions.EndHoldButton },//BUG - Bad things, doesn't mesh well with TnHTweaker
            { "SP - Ammo Reloader", Actions.SpawnAmmoReloaderButton },
            { "SP - Magazine Duplicator", Actions.SpawnMagDupeButton },
            { "SP - Recycler", Actions.SpawnGunRecylcerButton },
            { "Kill patrols", Actions.KillPatrolsButtonClicked },
        };

        public NPanel()
        {
            _NPanel = new LockablePanel();
            _NPanel.Configure += ConfigureForTool;
            _NPanel.TextureOverride = SodaliteUtils.LoadTextureFromBytes(Assembly.GetExecutingAssembly().GetResource("panel.png"));
        }

        public void ConfigureForTool(GameObject panel)
        {
            GameObject canvas = panel.transform.Find("OptionsCanvas_0_Main/Canvas").gameObject;


            menu = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                // Fill our parent and set pivot to top middle
                widget.RectTransform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                widget.RectTransform.localPosition = Vector3.zero;
                widget.RectTransform.localRotation = Quaternion.identity;
                widget.RectTransform.anchoredPosition = Vector2.zero;
                widget.RectTransform.sizeDelta = new Vector2(37f / 0.07f, 24f / 0.07f);
                widget.RectTransform.pivot = new Vector2(0.5f, 1f);
                // Adjust our grid settings
                widget.LayoutGroup.cellSize = new Vector2(171, 50);
                widget.LayoutGroup.spacing = Vector2.one * 4;
                widget.LayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
                widget.LayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
                widget.LayoutGroup.childAlignment = TextAnchor.UpperCenter;
                widget.LayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                widget.LayoutGroup.constraintCount = 3;

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Item interactions";
                    button.AddButtonListener(switchToItem);
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Player interactions";
                    button.AddButtonListener(switchToPlayer);
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "TnH Stuff";
                    button.AddButtonListener(SwitchToTnH);
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Scene Selection";
                    button.AddButtonListener(switchToScene);
                    button.RectTransform.localRotation = Quaternion.identity;
                });
            });

            itemTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                // Fill our parent and set pivot to top middle
                widget.RectTransform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                widget.RectTransform.localPosition = Vector3.zero;
                widget.RectTransform.localRotation = Quaternion.identity;
                widget.RectTransform.anchoredPosition = Vector2.zero;
                widget.RectTransform.sizeDelta = new Vector2(37f / 0.07f, 24f / 0.07f);
                widget.RectTransform.pivot = new Vector2(0.5f, 1f);
                // Adjust our grid settings
                widget.LayoutGroup.cellSize = new Vector2(171, 50);
                widget.LayoutGroup.spacing = Vector2.one * 4;
                widget.LayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
                widget.LayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
                widget.LayoutGroup.childAlignment = TextAnchor.UpperCenter;
                widget.LayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                widget.LayoutGroup.constraintCount = 3;

                AddBack(widget);
                AddBatch(widget, ItemToolsDict);
            });
            itemTools.gameObject.SetActive(false);

            playerTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                // Fill our parent and set pivot to top middle
                widget.RectTransform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                widget.RectTransform.localPosition = Vector3.zero;
                widget.RectTransform.localRotation = Quaternion.identity;
                widget.RectTransform.anchoredPosition = Vector2.zero;
                widget.RectTransform.sizeDelta = new Vector2(37f / 0.07f, 24f / 0.07f);
                widget.RectTransform.pivot = new Vector2(0.5f, 1f);
                // Adjust our grid settings
                widget.LayoutGroup.cellSize = new Vector2(171, 50);
                widget.LayoutGroup.spacing = Vector2.one * 4;
                widget.LayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
                widget.LayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
                widget.LayoutGroup.childAlignment = TextAnchor.UpperCenter;
                widget.LayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                widget.LayoutGroup.constraintCount = 3;

                AddBack(widget);
                AddBatch(widget, PlayerToolsDict);
            });
            playerTools.gameObject.SetActive(false);

            tnhTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                // Fill our parent and set pivot to top middle
                widget.RectTransform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                widget.RectTransform.localPosition = Vector3.zero;
                widget.RectTransform.localRotation = Quaternion.identity;
                widget.RectTransform.anchoredPosition = Vector2.zero;
                widget.RectTransform.sizeDelta = new Vector2(37f / 0.07f, 24f / 0.07f);
                widget.RectTransform.pivot = new Vector2(0.5f, 1f);
                // Adjust our grid settings
                widget.LayoutGroup.cellSize = new Vector2(171, 50);
                widget.LayoutGroup.spacing = Vector2.one * 4;
                widget.LayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
                widget.LayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
                widget.LayoutGroup.childAlignment = TextAnchor.UpperCenter;
                widget.LayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                widget.LayoutGroup.constraintCount = 3;

                AddBack(widget);
                AddBatch(widget, TnHToolsDict);
            });
            tnhTools.gameObject.SetActive(false);

            sceneTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                // Fill our parent and set pivot to top middle
                widget.RectTransform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                widget.RectTransform.localPosition = Vector3.zero;
                widget.RectTransform.localRotation = Quaternion.identity;
                widget.RectTransform.anchoredPosition = Vector2.zero;
                widget.RectTransform.sizeDelta = new Vector2(37f / 0.07f, 24f / 0.07f);
                widget.RectTransform.pivot = new Vector2(0.5f, 1f);
                // Adjust our grid settings
                widget.LayoutGroup.cellSize = new Vector2(171, 50);
                widget.LayoutGroup.spacing = Vector2.one * 4;
                widget.LayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
                widget.LayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
                widget.LayoutGroup.childAlignment = TextAnchor.UpperCenter;
                widget.LayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                widget.LayoutGroup.constraintCount = 3;

                AddBack(widget);
                foreach (var kvp in Actions.SCENE_LIST)
                {
                    widget.AddChild((ButtonWidget button) => {
                        button.ButtonText.text = kvp.Value;
                        button.AddButtonListener(() =>
                        {
                            SteamVR_LoadLevel.Begin(kvp.Value, false, 0.5f, 0f, 0f, 1f);
                            foreach (var quitReceiver in GM.CurrentSceneSettings.QuitReceivers)
                                quitReceiver.BroadcastMessage("QUIT", SendMessageOptions.DontRequireReceiver);
                        });
                    button.RectTransform.localRotation = Quaternion.identity;
                    });
                }
            });
            sceneTools.gameObject.SetActive(false);

        }

        private void switchToItem()
        {
            menu.gameObject.SetActive(false);
            itemTools.gameObject.SetActive(true);
        }

        private void switchToPlayer()
        {
            menu.gameObject.SetActive(false);
            playerTools.gameObject.SetActive(true);
        }

        private void SwitchToTnH()
        {
            menu.gameObject.SetActive(false);
            tnhTools.gameObject.SetActive(true);
        }

        private void switchToScene()
        {
            menu.gameObject.SetActive(false);
            sceneTools.gameObject.SetActive(true);
        }

        private void AddBatch(GridLayoutWidget widget, Dictionary<string, Action> dict)
        {
            foreach (var kvp in dict)
            {
                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = kvp.Key;
                    button.AddButtonListener(kvp.Value);
                    button.RectTransform.localRotation = Quaternion.identity;
                });
            }
        }

        private void AddBack(GridLayoutWidget widget)
        {
            widget.AddChild((ButtonWidget button) => {
                button.ButtonText.text = "----Back----";
                button.AddButtonListener(() => 
                {
                    widget.gameObject.SetActive(false);
                    menu.gameObject.SetActive(true);
                });
                button.RectTransform.localRotation = Quaternion.identity;
            });
        }

        public void SpawnNPanel()
        {
            FVRWristMenu wristMenu = WristMenuAPI.Instance;
            if (wristMenu is null || !wristMenu) return;
            GameObject panel = _NPanel.GetOrCreatePanel();
            wristMenu.m_currentHand.RetrieveObject(panel.GetComponent<FVRPhysicalObject>());
        }

        public void LoadWristmenu()//legacy stuff for reasons i guess, doesnt work
        {
            Dictionary<string, string> SceneList = Actions.SCENE_LIST;
            foreach (var scene in SceneList.Reverse())
            {
                WristMenuAPI.Buttons.Add(new WristMenuButton(scene.Value, () =>
                {
                    SteamVR_LoadLevel.Begin(scene.Key, false, 0.5f, 0f, 0f, 1f);
                    foreach (var quitReceiver in GM.CurrentSceneSettings.QuitReceivers)
                        quitReceiver.BroadcastMessage("QUIT", SendMessageOptions.DontRequireReceiver);
                }));
            }
            //Add in a header thing for the tnh list
            WristMenuAPI.Buttons.Add(new WristMenuButton(Common.SEPARATOR, Actions.Empty));

            //Wristmenu actions----------------------------------------------------------------------------------------
            //Take and Hold
            foreach (var kvp in TnHToolsDict.Reverse())
                WristMenuAPI.Buttons.Add(new WristMenuButton(kvp.Key, kvp.Value));

            WristMenuAPI.Buttons.Add(new WristMenuButton(Common.SEPARATOR, Actions.Empty));

            //Player
            foreach (var kvp in PlayerToolsDict.Reverse())
                WristMenuAPI.Buttons.Add(new WristMenuButton(kvp.Key, kvp.Value));

            WristMenuAPI.Buttons.Add(new WristMenuButton(Common.SEPARATOR, Actions.Empty));

            //Item
            foreach (var kvp in ItemToolsDict.Reverse())
                WristMenuAPI.Buttons.Add(new WristMenuButton(kvp.Key, kvp.Value));

            WristMenuAPI.Buttons.Add(new WristMenuButton(Common.SEPARATOR, Actions.Empty));
        }
    }
}
