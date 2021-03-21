using BepInEx;
using Common.Util;
using System.IO;
using System.Reflection;
using LitJson;
using UnityEngine;
using HarmonyLib;

namespace ZombMoreArrows
{
    [BepInPlugin(MODID, MODNAME, MODVER)]
    [BepInDependency(ValheimLib.ValheimLib.ModGuid)]
    class ZombMoreArrows : BaseUnityPlugin
    {
        public const string MODID = "zombehian.MoreArrows";
        public const string MODNAME = "Zomb More Arrows";
        public const string MODVER = "0.0.5";
        private Harmony _harmony;
        internal static ZombMoreArrows Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            AssetHelper.Init("arrows", "arrow_recipes.json");
            this._harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }
    }
}
