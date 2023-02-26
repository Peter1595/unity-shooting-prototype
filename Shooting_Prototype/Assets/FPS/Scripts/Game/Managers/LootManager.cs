using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    [System.Serializable]
    public struct LootLevel
    {
        public string levelName;

        public int level;

        public GameObject[] Loot;
    }

    public class LootManager : MonoBehaviour
    {
        public LootLevel[] LootLevels;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public LootLevel GetLevel(int level)
        {
            if (level < 1)
            {
                level = 1;
            }
            else if (level > LootLevels.Length)
            {
                level = LootLevels.Length - 1;
            }

            return LootLevels[level - 1];
        }

        public LootLevel ChooseLevel(int level)
        {
            int choosenLevel = level;

            float chance = Random.RandomRange(0, 100);

            if (chance > 60 && chance <= 80)
            {
                choosenLevel = Random.RandomRange(choosenLevel - 1, choosenLevel + 1);
            }
            else if (choosenLevel > 80)
            {
                choosenLevel = Random.RandomRange(choosenLevel - 2, choosenLevel + 2);
            }

            return GetLevel(choosenLevel);
        }

        public GameObject ChooseBuff(LootLevel level)
        {
            if (level.Loot.Length < 1)
            {
                return null;
            }

            return level.Loot[Random.RandomRange(0, level.Loot.Length)];
        }
    }
}
