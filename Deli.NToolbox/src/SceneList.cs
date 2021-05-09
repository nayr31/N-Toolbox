using System.Collections.Generic;
using System.Linq;
using FistVR;
using UnityEngine;
using UnityEditor;

namespace NToolbox
{
    public static class SceneList
    {
        /// <summary>
        /// Every scenedef in h3vr
        /// </summary>
        public static IEnumerable<MainMenuSceneDef> Scenes => GetAllInstances<MainMenuSceneDef>();

        /// <summary>
        /// Function to get all instances of a ScriptableObject (Stolen off stackoverflow)
        /// </summary>
        /// <typeparam name="T">Scriptable object</typeparam>
        /// <returns>All instances of the given scriptable object</returns>
        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets("t:"+ typeof(T).Name);  //FindAssets uses tags check documentation for more info
            var a = new T[guids.Length];
            for(var i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return a;
 
        }
    }
}