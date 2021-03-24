using HarmonyLib;
using static Common.Util.Tools;
using static ZombBetterTames.Tames.Leveling;

namespace ZombBetterTames.Patches
{
    class MonsterAIs
    {
        [HarmonyPatch(typeof(MonsterAI), "DoAttack")]
        public static class DoAttack_Patch
        {
            public static void Postfix(ref MonsterAI __instance, ref Character target, ref bool isFriend)
            {
                var item = GetMonster(__instance, ref Tamables.monsters);
                if (item != null)
                {
                    HandleLeveling(ref item);
                }
            }
        }
    }
}
