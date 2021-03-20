using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;
using Common.Util;

namespace ZombMorePotions
{
    class AssetHelper
    {
        public static Items.RecipesConfig recipes;
        public static AssetBundle assetBundle;

        public static void Init(string assetbundle, string recipejson)
        {
            assetBundle = Potions.LoadAssetBundle(assetbundle);
            recipes = Potions.LoadJsonFile<Items.RecipesConfig>(recipejson);

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
        }
    }
}
