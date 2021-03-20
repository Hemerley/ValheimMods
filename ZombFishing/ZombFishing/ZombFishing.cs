using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;
using Pipakin.SkillInjectorMod;
using System;

namespace ZombehFishing
{
    [BepInPlugin("zombehian.fishing", "Zomb Fishing", "0.0.1")]
    [BepInDependency("com.pipakin.SkillInjectorMod")]

    public class ZombehFishing : BaseUnityPlugin
    {

        private readonly Harmony harmony = new Harmony("zombeh.hearty");
        private static ConfigEntry<float> configSkillIncrease; //
        private static ConfigEntry<float> pullStaminaUseMod; //
        private static ConfigEntry<float> hookedStaminaPerSecMod;
        private static ConfigEntry<float> pullLineSpeedMod;
        private static ConfigEntry<float> skillGainIncrement; //
        public const int SKILL_TYPE = 601;
        public static Player player;
        public static ZNetView zNetView;

        void Awake()
        {
            harmony.PatchAll();
            pullStaminaUseMod = Config.Bind("General", "PullStaminaUseMod", 10f, "Amount of stamina used to pull line in modifier.");
            hookedStaminaPerSecMod = Config.Bind("General", "HookedStaminaPerSecMod", 1f, "Amount of stamina to drain per second modifier.");
            pullLineSpeedMod = Config.Bind("General", "PullLineSpeedMod", 50f, "How fast you pull in the line modifier.");
            configSkillIncrease = Config.Bind("General", "LevelingIncrement", 1f, "Increment to increase skill per use of a full bar of stamina");
            skillGainIncrement = Config.Bind("General", "GainIncrement", 1f, "Amount of fish caught before any skill is gained");
            SkillInjector.RegisterNewSkill(SKILL_TYPE, "Fishing", "Affects fishing skills", 1.0f, null, Skills.SkillType.Unarmed);
        }

        void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        [HarmonyPatch(typeof(FishingFloat), "Awake")]
        static class Patch_Fish_Variables
        {
            static void Postfix(ref float ___m_pullStaminaUse, ref float ___m_hookedStaminaPerSec, ref float ___m_pullLineSpeed)
            {
                try
                {
                    var factor = player.GetSkillFactor((Skills.SkillType)SKILL_TYPE);
                    Debug.Log("Factor: " + factor.ToString());
                    if (factor > 0)
                    {
                        var pAmount = (float)Math.Ceiling(factor * pullStaminaUseMod.Value);
                        var hAmount = (float)Math.Ceiling(factor * hookedStaminaPerSecMod.Value);
                        var lAmount = (float)Math.Ceiling(factor * pullLineSpeedMod.Value);
                        ___m_hookedStaminaPerSec = hAmount;
                        ___m_pullLineSpeed = lAmount;
                        ___m_pullStaminaUse = pAmount;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("[Zomb Fishing] Failed to augment fishing speeds.");
                    Debug.LogError("[Zomb Fishing] " + e.ToString());
                }
            }
        }

        [HarmonyPatch(typeof(FishingFloat), "SetCatch")]
        public static class IncreaseFishingSkill
        {
            public static void Prefix ()
            {
                try
                {
                    var progress = zNetView.GetZDO().GetFloat("fishing_progress", 0f);
                    //adjust the amount by the multiplier
                    progress += 1;

                    if (progress > skillGainIncrement.Value)
                    {

                        var ratio = progress / player.GetMaxStamina();
                        Debug.Log("[Zomb Fishing] Ratio: " + ratio.ToString());
                        player.RaiseSkill((Skills.SkillType)SKILL_TYPE, ratio * configSkillIncrease.Value);
                        zNetView.GetZDO().Set("fishing_progress", 0f);
                    }
                    else
                    {
                        zNetView.GetZDO().Set("fishing_progress", progress);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("[Zomb Fishing] Failed to augement fishing skill.");
                    Debug.LogError("[Zomb Fishing] " + e.ToString());
                }
            }
        }

        [HarmonyPatch(typeof(Player), "RPC_UseStamina")]
        public static class GetPlayerID
        {
            public static void Prefix(float v, Player __instance, ZNetView ___m_nview)
            {
                try
                {
                    player = __instance;
                    zNetView = ___m_nview;
                }
                catch (Exception e)
                {
                    Debug.LogError("[Zomb Fishing] Failed to set player instance.");
                    Debug.LogError("Zomb Fishing]" + e.ToString());
                }
            }
        }

    }
}
