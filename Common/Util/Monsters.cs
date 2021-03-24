using System;


namespace Common.Util
{
    class Monsters
    {
        [Serializable]
        public class MonsterConfig
        {
            public Tameable monsterID;
            public Character critter;
            public int experience;
        }
    }
}
