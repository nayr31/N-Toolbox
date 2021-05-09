using Deli.H3VR.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NToolbox.src
{
    class ScenePanel
    {
        public readonly LockablePanel _scenePanel;

        ScenePanel()
        {
            _scenePanel = new LockablePanel();
            _scenePanel.Configure += panel =>
            {
                Transform canvasTransform = panel.transform.Find("OptionsCanvas_0_Main/Canvas");
            };
        }
    }
}
