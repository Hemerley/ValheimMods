using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;
using Pipakin.SkillInjectorMod;
using System;
using System.Collections.Generic;

namespace ZombHearty
{
    // Setup Base Configuration
    [BepInPlugin("zombeh.hearty", "Zomb Hearty", "0.0.1")]
    [BepInDependency("com.pipakin.SkillInjectorMod")]

    public class ZombHearty : BaseUnityPlugin
    {

        private readonly Harmony harmony = new Harmony("zombeh.hearty");
        private static ConfigEntry<float> configSkillIncrease;
        private static ConfigEntry<float> maxHealthMultiplier;
        private static ConfigEntry<float> skillGainIncrement;
        public const int SKILL_TYPE = 600;

        void Awake()
        {
            harmony.PatchAll();

            configSkillIncrease = Config.Bind("Progression", "LevelingIncrement", 1.0f, "Increment to increase skill per use of a full bar of stamina");
            maxHealthMultiplier = Config.Bind("General", "MaxMultiplier", 200.0f, "Maximum health multiplier (at level 100). Minimum 1");
            skillGainIncrement = Config.Bind("General", "GainIncrement", 25f, "Amount of stamina used before any skill is gained");

            SkillInjector.RegisterNewSkill(SKILL_TYPE, "Hearty", "Affects maximum health level", 1.0f, null, Skills.SkillType.Run);

        }

        [HarmonyPatch(typeof(Player), "SetMaxHealth")]
        public static class ApplyHeartyEffects
        { 
            public static void Prefix(ref float health, Player __instance)
            {
                try
                {
                    var factor = __instance.GetSkillFactor((Skills.SkillType)SKILL_TYPE);

                    var amount = (float)Math.Ceiling(factor * maxHealthMultiplier.Value);

                    health += amount;

                }
                catch (Exception e)
                {
                    Debug.LogError("[Zomb Hearty] Failed to augment health.");
                    Debug.LogError(e.ToString());
                }
            }
        }

        [HarmonyPatch(typeof(Player), "RPC_UseStamina")]
        public static class IncreaseHeartySkill
        {
            public static void Prefix(float v, Player __instance, ZNetView ___m_nview)
            {
                try
                {
                    var progress = ___m_nview.GetZDO().GetFloat("hearty_progress", 0f);
                    //adjust the amount by the multiplier
                    progress += v;

                    if (progress > skillGainIncrement.Value)
                    {
                        var ratio = progress / __instance.GetMaxStamina();
                        __instance.RaiseSkill((Skills.SkillType)SKILL_TYPE, ratio * configSkillIncrease.Value);
                        ___m_nview.GetZDO().Set("hearty_progress", 0f);
                    }
                    else
                    {
                        ___m_nview.GetZDO().Set("hearty_progress", progress);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("[Zomb Hearty] Failed to augement hearty skill.");
                    Debug.LogError(e.ToString());
                }
            }
        }
    }
}
