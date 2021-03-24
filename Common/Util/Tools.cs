using BepInEx;
using System.IO;
using System.Reflection;
using LitJson;
using UnityEngine;
using System.Collections.Generic;

namespace Common.Util
{
    class Tools
    {
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
            var assetFileName = Path.Combine(Paths.PluginPath, "Tools", assetName);
            if (!File.Exists(assetFileName))
            {
                Assembly assembly = typeof(Tools).Assembly;
                assetFileName = Path.Combine(Path.GetDirectoryName(assembly.Location), assetName);
                if (!File.Exists(assetFileName))
                {
                    Debug.LogError($"[Zomb Library] Could not find asset ({assetName})");
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
            Debug.LogError($"[Zomb Library] Could not find asset bubled ({filename})");
            return null;
        }

        public static bool FindMonster(Monsters.MonsterConfig monster, ref List<Monsters.MonsterConfig>monsters)
        {
            foreach (var item in monsters)
            {
                if (item.monsterID == monster.monsterID)
                {
                    return true;
                }
            }
            return false;
        }

        public static Monsters.MonsterConfig GetMonster(MonsterAI monster, ref List<Monsters.MonsterConfig>monsters)
        {
            foreach (var item in monsters)
            {

                if (monster.m_tamable == item.monsterID)
                {
                    Debug.LogWarning("Found Our Monster");
                    return item;
                }
            }   
            return null;
        }
    }
}
