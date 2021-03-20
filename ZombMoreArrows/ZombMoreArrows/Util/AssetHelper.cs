using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;
using UnityObject = UnityEngine.Object;

namespace ZombMoreArrows.Util

{
    class AssetHelper
    {
        public static Util.Consumables.RecipesConfig recipes;
        public static AssetBundle assetBundle;

        public static void Init()
        {
            assetBundle = Util.Tools.LoadAssetBundle("arrows");
            recipes = Util.Tools.LoadJsonFile<Util.Consumables.RecipesConfig>("arrow_recipes.json");
            
            foreach (var recipe in recipes.recipes)
            {
                if (recipe.enabled)
                {
                    if (assetBundle.Contains(recipe.item))
                    {
                        var prefab = assetBundle.LoadAsset<GameObject>(recipe.item);
                        Items.Items.AddCustomItem(prefab);
                        Items.Items.AddCustomRecipe(prefab);
                    }
                }    
            }
            ObjectDBHelper.OnAfterInit += () =>
            {
                foreach (var rec in recipes.recipes)
                {
                    if (rec.enabled)
                    {
                        var arrow = Prefab.Cache.GetPrefab<Projectile>(rec.projectilePrefab);
                        if (arrow)
                        {
                            var prefab = assetBundle.LoadAsset<GameObject>(rec.item);
                            GameObject projectilePrefab = Prefab.InstantiateClone(arrow.gameObject, rec.projectilePrefabName);
                            prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_attack.m_attackProjectile = projectilePrefab;
                        }
                    }    
                }
            };
        }
    }
}
