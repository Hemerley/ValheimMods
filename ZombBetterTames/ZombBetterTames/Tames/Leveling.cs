using static Common.Util.Monsters;
using static ZombBetterTames.BetterTames;
using UnityEngine;

namespace ZombBetterTames.Tames
{
    class Leveling
    {

        public static void HandleLeveling(ref MonsterConfig monster)
        {
            monster.experience += 1;
            var monsterLevel = monster.critter.GetLevel();
            Debug.LogWarning($"[{MODNAME}] Monster Level: {monsterLevel}");
            Debug.LogWarning($"[{MODNAME}] Monster Experience: {monster.experience}");
        }

    }
}
