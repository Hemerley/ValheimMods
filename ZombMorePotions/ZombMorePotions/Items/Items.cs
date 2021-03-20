using System.Collections.Generic;
using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;

namespace ZombMorePotions.Items
{
    class Items
    {
        private static void AddCustomToken(string TokenName, string TokenValue, string TokenDescripion, string TokenDescriptionValue)
        {
            Language.AddToken(TokenName, TokenValue);
            Language.AddToken(TokenDescripion, TokenDescriptionValue);
        }

        public static void AddCustomRecipe(GameObject prefab)
        {
            var recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.m_item = prefab.GetComponent<ItemDrop>();
            var neededResources = new List<Piece.Requirement>();
            foreach (var rec in Utils.AssetHelper.recipes.recipes)
            {
                if (rec.name == prefab.name)
                {
                    foreach (var component in rec.resources)
                    {
                        neededResources.Add(MockRequirement.Create(component.item, component.amount));
                    }
                    recipe.m_amount = rec.amount;
                }
            }
            recipe.m_resources = neededResources.ToArray();
            CustomRecipe newRecipe = new CustomRecipe(recipe, false, true);
            ObjectDBHelper.Add(newRecipe);
        }

        public static void AddCustomItem(GameObject prefab)
        {
            CustomItem customItem = new CustomItem(prefab, fixReference: true);

            ObjectDBHelper.Add(customItem);

            foreach (var rec in Utils.AssetHelper.recipes.recipes)
            {
                if (rec.name == prefab.name)
                {
                    AddCustomToken(rec.tokenName, rec.tokenValue, rec.tokenDescription, rec.tokenDescriptionValue);
                }
            }
        }
    }
}
