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

        public static T LoadJsonFile<T>(string filename) where T : class
        {
            var jsonFileName = GetAssetPath(filename);
            if (!string.IsNullOrEmpty(jsonFileName))
            {
                var jsonFile = File.ReadAllText(jsonFileName);
                return JsonMapper.ToObject<T>(jsonFile);
            }

            return null;
        }

        private static string GetAssetPath(string assetName)
        {
            var assetFileName = Path.Combine(Paths.PluginPath, "ZombMorePotions", assetName);
            if (!File.Exists(assetFileName))
            {
                Assembly assembly = typeof(Potions).Assembly;
                assetFileName = Path.Combine(Path.GetDirectoryName(assembly.Location), assetName);
                if (!File.Exists(assetFileName))
                {
                    Debug.LogError($"[More Potions] Could not find asset ({assetName})");
                    return null;
                }
            }

            return assetFileName;
        }

        public static AssetBundle LoadAssetBundle(string filename)
        {
            var assetBundlePath = GetAssetPath(filename);
            if (!string.IsNullOrEmpty(assetBundlePath))
            {
                return AssetBundle.LoadFromFile(assetBundlePath);
            }
            Debug.LogError($"[More Potions] Could not find asset bubled ({filename})");
            return null;
        }

    }
}
