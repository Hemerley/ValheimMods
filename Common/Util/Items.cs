using System;
using System.Collections.Generic;

namespace Common.Util
{
    class Items
    {
        [Serializable]
        public class RecipeRequirementConfig
        {
            public string item;
            public int amount;
        }

        [Serializable]
        public class RecipeConfig
        {
            public string name;
            public string item;
            public int amount;
            public string craftingStation;
            public string tokenName;
            public string tokenValue;
            public string tokenDescription;
            public string tokenDescriptionValue;
            public string projectilePrefab;
            public string projectilePrefabName;
            public int retrieveChance;
            public int minStationLevel;
            public bool enabled;
            public string repairStation;
            public int maxStackSize;
            public double weight;
            public int damage;
            public int blunt;
            public int slash;
            public int pierce;
            public int chop;
            public int pickaxe;
            public int fire;
            public int frost;
            public int lightning;
            public int poison;
            public int spirit;
            public int attackForce;
            public int backstabBonus;
            public List<RecipeRequirementConfig> resources = new List<RecipeRequirementConfig>();
        }

        [Serializable]
        public class RecipesConfig
        {
            public List<RecipeConfig> recipes = new List<RecipeConfig>();
        }
    }
}
