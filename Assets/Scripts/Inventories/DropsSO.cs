using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "RPG/InventorySystem/Drops")]
    public class DropsSO : ScriptableObject
    {
        

        [SerializeField] DropInfo[] drops;

         // chance to drop item or not per level
        [SerializeField] int[] chance;

        // min / max numb of  drops per level
        [SerializeField] int[] minDrops;
        [SerializeField] int[] maxDrops;


        public IEnumerable<Drop> GetDrops(int level)
        {
            if(!ShouldDrope(level))
            {
                yield break ;
            }
            int dropsNumb = GetDropsNumb(level) ;
            for (int i = 0; i < dropsNumb; i++)
            {
                yield return GetRanddomDrop();
            }
        }

        private Drop GetRanddomDrop()
        {
            var randomDrope = GetRandomeItem();
            Drop drop = new Drop();
            drop.item = randomDrope.item;
            drop.numb = randomDrope.GetRandomNumb();
            return drop;
        }

        private int GetDropsNumb(int level)
        {
            int min = GetByLevel(minDrops,level);
            int max = GetByLevel(maxDrops,level);

            return UnityEngine.Random.Range(min,max + 1);
        }

        private bool ShouldDrope(int level)
        {
            int propability = GetByLevel(chance,level);
            return UnityEngine.Random.Range(0,100) < propability;
        }

        DropInfo GetRandomeItem()
        {
            int totalChances = CalculateTotalChances();
            int roll = UnityEngine.Random.Range(0,totalChances);
            int currentChance = 0 ;
            foreach (var drop in drops)
            {
                currentChance += drop.realtiveChance;
                if(roll <= currentChance)
                {
                    return drop;
                }
            }
            int index = UnityEngine.Random.Range(0,drops.Length) ;
            // return random item if no item found
            return drops[index];
        }

        private int CalculateTotalChances()
        {
            int total = 0;
            foreach (var drop in drops)
            {
                total += drop.realtiveChance;
            }
            return total;
        }

        [System.Serializable]
        public struct DropInfo
        {
            public InventoryItem item;
            public int realtiveChance;
            public int minNumb;
            public int maxNumb;
            public int GetRandomNumb()
            {
                if(!item.IsStackable()) return 1 ;
                return UnityEngine.Random.Range(minNumb,maxNumb + 1);
            }
        }

        public struct Drop
        {
            public InventoryItem item;
            public int numb;
        }

        public static T GetByLevel<T>(T[] LevelArray,int level)
        {
            if(LevelArray.Length == 0 || level <= 0)
            {
                return default;
            }
            if(level > LevelArray.Length)
            {
                return LevelArray[LevelArray.Length - 1];
            }
            return LevelArray[level - 1];

        }
    }
}
