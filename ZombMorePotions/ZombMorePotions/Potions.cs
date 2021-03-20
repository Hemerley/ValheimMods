using BepInEx;
using Common.Util;
using System.IO;
using System.Reflection;
using LitJson;
using UnityEngine;

namespace ZombMorePotions
{
    [BepInPlugin(MODID, MODNAME, MODVER)]
    [BepInDependency(ValheimLib.ValheimLib.ModGuid)]
    public class Potions : BaseUnityPlugin
    {
        public const string MODID = "zombehian.MorePotions";
        public const string MODNAME = "Zomb More Potions";
        public const string MODVER = "0.0.1";
        internal static Potions Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            AssetHelper.Init("potions", "potions_recipes.json");
        }
    }
}
