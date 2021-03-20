using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;
using Common.Util;

namespace Common.Util
{
    class AssetHelper
    {
        public static Items.RecipesConfig recipes;
        public static AssetBundle assetBundle;

        public static void Init(string assetbundle, string recipejson)
        {
            assetBundle = Tools.LoadAssetBundle(assetbundle);
            recipes = Tools.LoadJsonFile<Items.RecipesConfig>(recipejson);

            foreach (var recipe in recipes.recipes)
            {
                if (recipe.enabled)
                {
                    if (assetBundle.Contains(recipe.item))
                    {
                        var prefab = assetBundle.LoadAsset<GameObject>(recipe.item);
                        PrefabHelper.AddCustomItem(prefab, recipes);
                        PrefabHelper.AddCustomRecipe(prefab, recipes);
                    }
                }
            }
            ObjectDBHelper.OnAfterInit += () =>
            {
                foreach (var rec in recipes.recipes)
                {
                    if (rec.enabled && rec.projectilePrefab != null)
                    {
                        var projectile = Prefab.Cache.GetPrefab<Projectile>(rec.projectilePrefab);
                        if (projectile)
                        {
                            var prefab = assetBundle.LoadAsset<GameObject>(rec.item);
                            GameObject projectilePrefab = Prefab.InstantiateClone(projectile.gameObject, rec.projectilePrefabName);
                            prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_attack.m_attackProjectile = projectilePrefab;
                        }
                    }
                }
            };
        }
    }
}
