using BepInEx;
using BepInEx.Configuration;
using static Common.Util.AssetHelper;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;

namespace ZombMoreArrows
{
    [BepInPlugin(MODID, MODNAME, MODVER)]
    [BepInDependency(ValheimLib.ValheimLib.ModGuid)]
    class ZombMoreArrows : BaseUnityPlugin
    {
        // mod information
        public const string MODID = "zombehian.MoreArrows";
        public const string MODNAME = "Zomb More Arrows";
        public const string MODVER = "0.0.6";

        // harmony project settings
        private Harmony _harmony;
        internal static ZombMoreArrows Instance { get; private set; }

        // configs for recollecting arrows
        public static ConfigEntry<bool> recollectArrowsEnabled;

        // configs for arrow physics
        public static ConfigEntry<bool> betterArrowPhysicsEnabled;
        public static ConfigEntry<float> arrowVelocity;
        public static ConfigEntry<float> arrowGravity;
        public static ConfigEntry<Vector3> arrowAimDirection;

        // configs for arrow damages
        public static ConfigEntry<bool> modifiedArrowDamageEnabled;

        // configs for weight/amount
        public static ConfigEntry<bool> modifiedWeightStackEnabled;

        // configs for crafting
        public static ConfigEntry<bool> bypassCraftingStationEnabled;

        private void Awake()
        {

            // configs for recollecting arrows
            recollectArrowsEnabled = Config.Bind("General", "Recollect Arrows Toggle", true, "Allows for the chance at recollecting arrows.");

            // configs for arrow physics
            betterArrowPhysicsEnabled = Config.Bind("ArrowPhysics", "Better Arrow Physics Toggle", true, "Allows for new arrow physics to be used.");
            arrowVelocity = Config.Bind("ArrowPhysics", "Arrow Velecity", 7f, "Change the arrows velocity.");
            arrowGravity = Config.Bind("ArrowPhysics", "Arrow Gravity", 15f, "Change the arrows gravity.");
            arrowAimDirection = Config.Bind<Vector3>("ArrowPhysics", "Aim Direction", new Vector3(0.01f, 0.08f, 0f), "Change the aim direction. Vanilla is 'x:0.0, y:0.0, z: 0.0'.");

            // configs for arrow damages
            modifiedArrowDamageEnabled = Config.Bind("ModifiedDamage", "Modified Damage Toggle", false, "Allows for modified damage to be used.");

            // configs for weight and stack
            modifiedWeightStackEnabled = Config.Bind("ModifiedStack", "Modified Stack/Amount Toggle", true, "Allow for modified stack/weight to be used.");

            // configs for bypassing crafting
            bypassCraftingStationEnabled = Config.Bind("ModifiedCrafting", "Bypass Crafting Station Requirement", false, "Allow for no crafting station needed.");

            Instance = this;
            Init("arrows", "arrow_recipes.json", bypassCraftingStationEnabled.Value);

            // harmony patcher
            this._harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);

            // damage patcher

            if (modifiedArrowDamageEnabled.Value)
            {
                ObjectDBHelper.OnAfterInit += delegate()
                {
                    foreach (var rec in recipes.recipes)
                    {
                        if (rec.enabled && rec.projectilePrefab != null)
                        {
                            var projectile = Prefab.Cache.GetPrefab<Projectile>(rec.projectilePrefab);
                            if (projectile)
                            {
                                var prefab = assetBundle.LoadAsset<GameObject>(rec.item);
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_damage = rec.damage;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_blunt = rec.blunt;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_slash = rec.slash;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_pierce = rec.pierce;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_chop = rec.chop;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_pickaxe = rec.pickaxe;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_fire = rec.fire;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_frost = rec.frost;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_lightning = rec.lightning;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_poison = rec.poison;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_spirit = rec.spirit;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_attackForce = rec.attackForce;
                                prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_backstabBonus = rec.backstabBonus;

                                if(modifiedWeightStackEnabled.Value)
                                {
                                    prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_weight = (float)rec.weight;
                                    prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_maxStackSize = rec.maxStackSize;
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}
