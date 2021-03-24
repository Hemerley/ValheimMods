using HarmonyLib;
using static Common.Util.Tools;
using static ZombBetterTames.Tames.Leveling;

namespace ZombBetterTames.Patches
{
    class Characters
    {
        //[HarmonyPatch(typeof(Character), "Damage")]
        //public static class Damage_Patch
        //{
        //    public static void Postfix(ref Character __instance, ref HitData hit)
        //    {
        //        var item = GetMonster(__instance, ref Tamables.monsters);
        //        if (item != null)
        //        {
        //            HandleLeveling(ref item);
        //        }
        //    }
        //}
    }
}
