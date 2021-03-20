using System.Collections.Generic;
using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;

namespace Common.Util
{
    class PrefabHelper
    {
        private static void AddCustomToken(string TokenName, string TokenValue, string TokenDescripion, string TokenDescriptionValue)
        {
            Language.AddToken(TokenName, TokenValue);
            Language.AddToken(TokenDescripion, TokenDescriptionValue);
        }

        public static void AddCustomRecipe(GameObject prefab, Items.RecipesConfig recipes)
        {
            var recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.m_item = prefab.GetComponent<ItemDrop>();
            var neededResources = new List<Piece.Requirement>();
            foreach (var rec in recipes.recipes)
            {
                if (rec.name == prefab.name)
                {
                    foreach (var component in rec.resources)
                    {
                        neededResources.Add(MockRequirement.Create(component.item, component.amount));
                    }
                    if (rec.craftingStation != null)
                    {
                        recipe.m_craftingStation = Mock<CraftingStation>.Create(rec.craftingStation);
                    }
                    if (rec.repairStation != null)
                    {
                        recipe.m_repairStation = Mock<CraftingStation>.Create(rec.repairStation);
                    }
                    recipe.m_amount = rec.amount;
                    recipe.m_minStationLevel = rec.minStationLevel;
                    recipe.m_resources = neededResources.ToArray();
                }
            }
            CustomRecipe newRecipe = new CustomRecipe(recipe, fixReference: true, true);
            ObjectDBHelper.Add(newRecipe);
        }

        public static void AddCustomItem(GameObject prefab, Items.RecipesConfig recipes)
        {
            CustomItem customItem = new CustomItem(prefab, fixReference: true);

            ObjectDBHelper.Add(customItem);

            foreach (var rec in recipes.recipes)
            {
                if (rec.name == prefab.name)
                {
                    AddCustomToken(rec.tokenName, rec.tokenValue, rec.tokenDescription, rec.tokenDescriptionValue);
                }
            }
        }
    }
}
