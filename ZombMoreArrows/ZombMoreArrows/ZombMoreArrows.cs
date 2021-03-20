using BepInEx;
using Common.Util;
using System.IO;
using System.Reflection;
using LitJson;
using UnityEngine;

namespace ZombMoreArrows
{
    [BepInPlugin(MODID, MODNAME, MODVER)]
    [BepInDependency(ValheimLib.ValheimLib.ModGuid)]
    class ZombMoreArrows : BaseUnityPlugin
    {
        public const string MODID = "zombehian.MoreArrows";
        public const string MODNAME = "Zomb More Arrows";
        public const string MODVER = "0.0.3";
        internal static ZombMoreArrows Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            AssetHelper.Init("arrows", "arrow_recipes.json");
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
            var assetFileName = Path.Combine(Paths.PluginPath, "ZombMoreArrows", assetName);
            if (!File.Exists(assetFileName))
            {
                Assembly assembly = typeof(ZombMoreArrows).Assembly;
                assetFileName = Path.Combine(Path.GetDirectoryName(assembly.Location), assetName);
                if (!File.Exists(assetFileName))
                {
                    Debug.LogError($"[More Arrows] Could not find asset ({assetName})");
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
            Debug.LogError($"[More Arrows] Could not find asset bubled ({filename})");
            return null;
        }
    }
}
