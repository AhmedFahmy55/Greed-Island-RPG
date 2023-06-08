using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.States;

namespace RPG.Inventories
{
    public class RandomItemDropper : ItemDropper
    {   
        [SerializeField] DropsSO itemsToDrop;


        public void DropRandomItems()
        {
            int leve = GetComponent<BaseStates>().GetLevel();
            var drops = itemsToDrop.GetDrops(leve);
            foreach (var drop in drops)
            {
                DropItem(drop.item,drop.numb);
            }
            
        }
    }
}
