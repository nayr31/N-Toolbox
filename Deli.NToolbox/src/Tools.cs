using System;
using System.Collections.Generic;

namespace NToolbox
{
    public static class Tools
    {
        public static readonly Dictionary<string, Action> ITEM = new()
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

        public static readonly Dictionary<string, Action> PLAYER = new()
        {
            { "Kill yourself", Actions.KillPlayerButtonClicked },
            { "Restore Full", Actions.RestoreHPButtonClicked },
            { "Toggle 1-hit", Actions.ToggleOneHitButtonClicked },
            { "Toggle Controller Geo", Actions.ToggleControllerGeo },
            { "Toggle God Mode", Actions.ToggleGodModeButtonClicked },
            { "Add Hand collision", Actions.AddHandCollision },
            { "Toggle HP bar", Actions.ToggleHealthBar },
            //{ "Toggle Invisibility", Actions.ToggleInvisButtonClicked },//Broken? Test for flat IFF = -1 to see if the check is broken
        };

        public static readonly Dictionary<string, Action> TNH = new()
        {
            { "Add token", Actions.AddTokenButtonClicked },
            { "SP - Ammo Reloader", Actions.SpawnAmmoReloaderButton },
            { "SP - Magazine Duplicator", Actions.SpawnMagDupeButton },
            { "SP - Recycler", Actions.SpawnGunRecyclerButton },
            { "Kill patrols", Actions.KillPatrolsButtonClicked },
        };
    }
}