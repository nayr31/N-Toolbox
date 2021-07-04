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
using BepInEx.Configuration;

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
        private GridLayoutWidget _tracerTools;
        private GridLayoutWidget _sceneTools;
        private GridLayoutWidget _configOptions;
        

        //string[] miscArray = new string[30];
        //ButtonWidget[] buttonArray = new ButtonWidget[9];
        //int miscOffset = 0;

        public NPanel()
        {
            //for (int i = 0; i < 30; i++)
            //    miscArray[i] = i.ToString();

            _NPanel = new LockablePanel();
            _NPanel.Configure += ConfigureTools;
            _NPanel.TextureOverride = SodaliteUtils.LoadTextureFromBytes(Assembly.GetExecutingAssembly().GetResource("panel.png"));
        } 
        

        public void ConfigureTools(GameObject panel)
        {
            GameObject canvas = panel.transform.Find("OptionsCanvas_0_Main/Canvas").gameObject;


            _menu = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                SetSubmenuToDefault(widget);

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
                    button.ButtonText.text = "Tracer Options";
                    button.AddButtonListener(SwitchToTracer);
                    button.RectTransform.localRotation = Quaternion.identity;
                });
                
                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Scene Selection";
                    button.AddButtonListener(SwitchToScene);
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Config Options";
                    button.AddButtonListener(SwitchToConfig);
                    button.RectTransform.localRotation = Quaternion.identity;
                });
            });

            _itemTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                SetSubmenuToDefault(widget);

                AddBack(widget);
                AddBatch(widget, Tools.ITEM);
            });
            _itemTools.gameObject.SetActive(false);

            _playerTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                SetSubmenuToDefault(widget);

                AddBack(widget);
                AddBatch(widget, Tools.PLAYER);
            });
            _playerTools.gameObject.SetActive(false);

            _tnhTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                SetSubmenuToDefault(widget);

                AddBack(widget);
                AddBatch(widget, Tools.TNH);
            });
            _tnhTools.gameObject.SetActive(false);

            _powerupTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                SetSubmenuToDefault(widget);

                AddBack(widget);
                foreach (var kvp in Common.POWERUPS)
                {
                    widget.AddChild((ButtonWidget button) => {
                        button.ButtonText.text = kvp.Value;
                        button.AddButtonListener(() => Actions.SpawnItemByItemIdLeftHand(kvp.Key, true));
                        button.RectTransform.localRotation = Quaternion.identity;
                    });
                }
            });
            _powerupTools.gameObject.SetActive(false);
            
            _tracerTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                SetSubmenuToDefault(widget);

                AddBack(widget);

                ButtonWidget displayButton = new ButtonWidget();

                widget.AddChild((ButtonWidget button) => {
                    
                    button.AddButtonListener(() => { UpdateTracerDisplay(button, 0); });
                    button.RectTransform.localRotation = Quaternion.identity;
                    displayButton = button;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "Reset to default";
                    button.AddButtonListener(() => { UpdateTracerDisplay(displayButton, 1 - GM.Options.QuickbeltOptions.TrailDecayTimes[2]); });
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "+ 0.01";
                    button.AddButtonListener(() => { UpdateTracerDisplay(displayButton, 0.01f); });
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "+ 0.10";
                    button.AddButtonListener(() => { UpdateTracerDisplay(displayButton, 0.1f); });
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "+ 1.00";
                    button.AddButtonListener(() => { UpdateTracerDisplay(displayButton, 1f); });
                    button.RectTransform.localRotation = Quaternion.identity;
                });
                
                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "- 0.01";
                    button.AddButtonListener(() => { UpdateTracerDisplay(displayButton, -0.01f); });
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "- 0.10";
                    button.AddButtonListener(() => { UpdateTracerDisplay(displayButton, -0.1f); });
                    button.RectTransform.localRotation = Quaternion.identity;
                });

                widget.AddChild((ButtonWidget button) => {
                    button.ButtonText.text = "- 1.00";
                    button.AddButtonListener(() => { UpdateTracerDisplay(displayButton, -1f); });
                    button.RectTransform.localRotation = Quaternion.identity;
                });
            });
            _tracerTools.gameObject.SetActive(false);

            _sceneTools = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                SetSubmenuToDefault(widget);

                AddBack(widget);
                foreach (var kvp in Common.SCENES)
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

            _configOptions = UiWidget.CreateAndConfigureWidget(canvas, (GridLayoutWidget widget) =>
            {
                SetSubmenuToDefault(widget);

                AddBack(widget);

                //AddConfigButtonBoolToggle(NToolbox.EnableHandColliders, "Default Hand Collision", widget);
                AddConfigButtonBoolToggle(NToolbox.EnableDebugSpheres, "Enable Debug Spheres", widget);
            });
            _configOptions.gameObject.SetActive(false);
        }

        //This could instead return a button to add to the widget, making it require 1 less arg
        private void AddConfigButtonBoolToggle(ConfigEntry<bool> value, String name, GridLayoutWidget widget)
        {
            widget.AddChild((ButtonWidget button) => {
                String buttonText = name + " [" + value.Value + "]";
                button.ButtonText.text = buttonText;
                button.AddButtonListener(() => {
                    value.Value = !value.Value;
                    button.ButtonText.text = buttonText;
                });
                button.RectTransform.localRotation = Quaternion.identity;
            });
        }

        //this method is used for showing different values inside of an array like a scrolling page
        //private void moveRef(bool isUp)
        //{
        //    if ((isUp && miscOffset - 3 >= 0) || (!isUp && miscOffset + 3 <= miscArray.Length)) { 
        //        miscOffset += isUp ? (-3) : 3;
        //        for(int i = 0; i < 9; i++)
        //            buttonArray[i].ButtonText.text = miscArray[i + miscOffset];
        //    }
        //}

        private void UpdateTracerDisplay(ButtonWidget button, float duration)
        {
            String buttonText = "Current Duration: " + GM.Options.QuickbeltOptions.TrailDecayTimes[2];
            GM.Options.QuickbeltOptions.TrailDecayTimes[2] += duration;
            button.ButtonText.text = buttonText;
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
        
        private void SwitchToTracer() => SwitchPage(_tracerTools);
      
        private void SwitchToScene() => SwitchPage(_sceneTools);

        private void SwitchToConfig() => SwitchPage(_configOptions);

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

        private void SetSubmenuToDefault(GridLayoutWidget widget)
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
            Dictionary<string, string> SceneList = Common.SCENES;
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
            foreach (var kvp in Tools.TNH.Reverse())
                WristMenuAPI.Buttons.Add(new WristMenuButton(kvp.Key, kvp.Value));

            AddSeparator();

            //Player
            foreach (var kvp in Tools.PLAYER.Reverse())
                WristMenuAPI.Buttons.Add(new WristMenuButton(kvp.Key, kvp.Value));

            AddSeparator();

            //Item
            foreach (var kvp in Tools.ITEM.Reverse())
                WristMenuAPI.Buttons.Add(new WristMenuButton(kvp.Key, kvp.Value));

            AddSeparator();
        }
    }
}
