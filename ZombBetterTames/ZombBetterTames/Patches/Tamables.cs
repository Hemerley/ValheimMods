using HarmonyLib;
using UnityEngine;
using static Common.Util.Monsters;
using static Common.Util.Tools;
using System.Collections.Generic;

namespace ZombBetterTames.Patches
{
    class Tamables
    {
        
        public static List<MonsterConfig> monsters = new List<MonsterConfig>();
        public static MonsterConfig monster = new MonsterConfig();

        [HarmonyPatch(typeof(Tameable), "Awake")]
        public static class Interact_Patch
        {
            public static void Postfix(ref Tameable __instance)
            {
                if (__instance.m_character.IsTamed())
                {
                    monster = new MonsterConfig();

                    monster.critter = __instance.m_character;
                    monster.monsterID = __instance;
                    monster.experience = 0;

                    if (FindMonster(monster, ref monsters))
                    {
                        Debug.LogWarning("Found Monster.");
                    }
                    else
                    {
                        Debug.LogWarning("Added Monster.");
                        monsters.Add(monster);
                    }
                }
            }
        }

        //[HarmonyPatch(typeof(Tameable), "Interact")]
        //public static class Interact_Patch
        //{
        //    public static bool Prefix(ref Tameable __instance, ref Humanoid user, bool hold)
        //    {
        //        if (hold)
        //        {
        //            return false;
        //        }

        //        if (!__instance.m_nview.IsValid())
        //        {
        //            return false;
        //        }

        //        string hoverName = __instance.m_character.GetHoverName();

        //        if (!__instance.m_character.IsTamed())
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            monster.critter = __instance.m_character;
        //            monster.monsterID = __instance;
        //            monster.experience = 0;
        //            if (FindMonster(monster, ref monsters))
        //            {
        //                Debug.LogWarning("Found Monster.");
        //            }
        //            else
        //            {
        //                Debug.LogWarning("Added Monster.");
        //                monsters.Add(monster);
        //            }
        //        }
        //        if (Time.time - __instance.m_lastPetTime > 1f)
        //        {
        //            __instance.m_lastPetTime = Time.time;
        //            __instance.m_petEffect.Create(__instance.m_character.GetCenterPoint(), Quaternion.identity, null, 1f);
        //            if (__instance.m_commandable)
        //            {
        //                __instance.Command(user);
        //            }
        //            else
        //            {
        //                user.Message(MessageHud.MessageType.Center, hoverName + " $hud_tamelove", 0, null);
        //            }
        //            return true;
        //        }
        //        return false;
        //    }
        //}

    }
}
