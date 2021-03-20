using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;
using UnityObject = UnityEngine.Object;

namespace ZombMorePotions.Utils
{
    class AssetHelper
    {
        public static Utils.Consumables.RecipesConfig recipes;
        public static AssetBundle assetBundle;

        public static void Init()
        {
            assetBundle = Utils.Tools.LoadAssetBundle("potions");
            recipes = Utils.Tools.LoadJsonFile<Utils.Consumables.RecipesConfig>("potions_recipes.json");

            foreach (var recipe in recipes.recipes)
            {
                if (assetBundle.Contains(recipe.item))
                {
                    var prefab = assetBundle.LoadAsset<GameObject>(recipe.item);
                    Items.Items.AddCustomItem(prefab);
                    Items.Items.AddCustomRecipe(prefab);
                }
            }
        }
    }
}
