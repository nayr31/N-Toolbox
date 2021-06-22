using FistVR;
using Sodalite;
using Sodalite.Api;
using Sodalite.UiWidgets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Sodalite.Utilities;

namespace NToolbox
{
    public class NPanel
    {
        private LockablePanel _NPanel;
        private GridLayoutWidget _menu;
        private GridLayoutWidget _itemTools;
        private GridLayoutWidget _playerTools;
        private GridLayoutWidget _tnhTools;
        private GridLayoutWidget _powerupTools;
        private GridLayoutWidget _sceneTools;
        private GridLayoutWidget _miscTools;


        public static readonly Dictionary<string, Action> ITEM_TOOLS = new()
        {
            { "Gather Items", Actions.GatherButtonClicked },
            { "Delete Items", Actions.DeleteButtonClicked },
            { "Delete Quickbelt Items", Actions.DeleteQuickbelt },
            { "Reset Traps", Actions.ResetTrapsButtonClicked },
            { "Freeze Guns/Melee", Actions.FreezeFireArmsMeleeButtonClicked },
            { "Freeze Ammo/Mags", Actions.FreezeAmmoMagButtonClicked },
            { "Freeze Attachments", Actions.FreezeAttachmentsButtonClicked },
            { "Unfreeze All", Actions.UnFreezeAllClicked },
            { "Ammo Panel", Actions.SpawnAmmoPanelButtonClicked },
            //trash bin
            //quickbelt fast?
            //sosig spawner
        };

        public static readonly Dictionary<string, Action> PLAYER_TOOLS = new()
        {
            { "Kill yourself", Actions.KillPlayerButtonClicked },
            { "Restore Full", Actions.RestoreHPButtonClicked },
            { "Toggle 1-hit", Actions.ToggleOneHitButtonClicked },
            { "Toggle Controller Geo", Actions.ToggleControllerGeo },
            { "Toggle God Mode", Actions.ToggleGodModeButtonClicked },
            //{ "Toggle Invisibility", Actions.ToggleInvisButtonClicked },//Broken? Test for flat IFF = -1 to see if the check is broken
        };

        public static readonly Dictionary<string, Action> TNH_TOOLS = new()
        {
            { "Add token", Actions.AddTokenButtonClicked },
            { "SP - Ammo Reloader", Actions.SpawnAmmoReloaderButton },
            { "SP - Magazine Duplicator", Actions.SpawnMagDupeButton },
            { "SP - Recycler", Actions.SpawnGunRecyclerButton },
            { "Kill patrols", Actions.KillPatrolsButtonClicked },
        };

        public NPanel()
        {
            _NPanel = new LockablePanel();
            _NPanel.Configure += ConfigureTools;
            _NPanel.TextureOverride = SodaliteUtils.LoadTextureFromBytes(Assembly.GetExecutingAssembly().GetResource("panel.png"));
        }

        public void ConfigureTools(GameObject panel)
        {
            GameObject canvas = panel.transform.Find("OptionsCanvas_0_Main/Canvas").gameObject;


            _menu = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
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
                    button.AddButtonListener(SwitchToItem);
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Player interactions";
                    button.AddButtonListener(SwitchToPlayer);
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "TnH Stuff";
                    button.AddButtonListener(SwitchToTnH);
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Powerups";
                    button.AddButtonListener(SwitchToPower);
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Scene Selection";
                    button.AddButtonListener(SwitchToScene);
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Misc Tools";
                    button.AddButtonListener(SwitchToMisc);
                    button.RectTransform.localRotation = Quaternion.identity;
                });
            });

            _itemTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
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
                AddBatch(widget, ITEM_TOOLS);
            });
            _itemTools.gameObject.SetActive(false);

            _playerTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
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
                AddBatch(widget, PLAYER_TOOLS);
            });
            _playerTools.gameObject.SetActive(false);

            _tnhTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
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
                AddBatch(widget, TNH_TOOLS);
            });
            _tnhTools.gameObject.SetActive(false);

            _powerupTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
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
                foreach (var kvp in Common.DOG_LIST)
                {
                    widget.AddChild((ButtonWidget button) => {
                        button.ButtonText.text = kvp.Value;
                        button.AddButtonListener(() => Actions.SpawnItemByItemIdLeftHand(kvp.Key, true));
                        button.RectTransform.localRotation = Quaternion.identity;
                    });
                }
            });
            _powerupTools.gameObject.SetActive(false);

            _sceneTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
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
                foreach (var kvp in Common.SCENE_LIST)
                {
                    widget.AddChild((ButtonWidget button) => {
                        button.ButtonText.text = kvp.Value;
                        button.AddButtonListener(() =>
                        {
                            SteamVR_LoadLevel.Begin(kvp.Key, false, 0.5f, 0f, 0f, 1f);
                            foreach (var quitReceiver in GM.CurrentSceneSettings.QuitReceivers)
                                quitReceiver.BroadcastMessage("QUIT", SendMessageOptions.DontRequireReceiver);
                        });
                    button.RectTransform.localRotation = Quaternion.identity;
                    });
                }
            });
            _sceneTools.gameObject.SetActive(false);

            _miscTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
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
                foreach (var kvp in Common.MISC_LIST)
                {
                    widget.AddChild((ButtonWidget button) => {
                        button.ButtonText.text = kvp.Value;
                        button.AddButtonListener(() => Actions.SpawnItemByItemIdLeftHand(kvp.Key, false));
                        button.RectTransform.localRotation = Quaternion.identity;
                    });
                }
            });
            _miscTools.gameObject.SetActive(false);

        }

        private void SwitchPage(GridLayoutWidget page)
        {
            _menu.gameObject.SetActive(false);
            page.gameObject.SetActive(true);
        }

        private void SwitchToItem() => SwitchPage(_itemTools);
      
        private void SwitchToPlayer() => SwitchPage(_playerTools);

        private void SwitchToTnH() => SwitchPage(_tnhTools);

        private void SwitchToPower() => SwitchPage(_powerupTools);
      
        private void SwitchToScene() => SwitchPage(_sceneTools);

        private void SwitchToMisc() => SwitchPage(_miscTools);

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
                    _menu.gameObject.SetActive(true);
                });
                button.RectTransform.localRotation = Quaternion.identity;
            });
        }

        public void Spawn()
        {
            FVRWristMenu wristMenu = WristMenuAPI.Instance;
            if (wristMenu is null || !wristMenu) return;
            GameObject panel = _NPanel.GetOrCreatePanel();
            wristMenu.m_currentHand.RetrieveObject(panel.GetComponent<FVRPhysicalObject>());
        }

        private void AddSeparator() => WristMenuAPI.Buttons.Add(new WristMenuButton(Common.SEPARATOR, Actions.Empty));

        public void LoadWristMenu()//legacy stuff for reasons i guess
        {
            Dictionary<string, string> SceneList = Common.SCENE_LIST;
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
            AddSeparator();

            //Wristmenu actions----------------------------------------------------------------------------------------
            //Take and Hold
            foreach (var kvp in TNH_TOOLS.Reverse())
                WristMenuAPI.Buttons.Add(new WristMenuButton(kvp.Key, kvp.Value));

            AddSeparator();

            //Player
            foreach (var kvp in PLAYER_TOOLS.Reverse())
                WristMenuAPI.Buttons.Add(new WristMenuButton(kvp.Key, kvp.Value));

            AddSeparator();

            //Item
            foreach (var kvp in ITEM_TOOLS.Reverse())
                WristMenuAPI.Buttons.Add(new WristMenuButton(kvp.Key, kvp.Value));

            AddSeparator();
        }
    }
}
