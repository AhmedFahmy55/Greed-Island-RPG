using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.States;

namespace RPG.Inventories
{
    
    public class Equipment : MonoBehaviour, ISaveable , IModifireProvider
    {
        // STATE
        Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();

        // PUBLIC

        public event Action equipmentUpdated;




       
        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return equippedItems[equipLocation];
        }

       
        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            Debug.Assert(item.GetAllowedEquipLocation() == slot);

            equippedItems[slot] = item;

            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

       
        public void RemoveItem(EquipLocation slot)
        {
            equippedItems.Remove(slot);
            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

                public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            return equippedItems.Keys;
        }


        IEnumerable<EquipLocation> GetAllEquipLocation()
        {
          return equippedItems.Keys;
        }

        public IEnumerable<float> GetAddtiveModifires(State state)
        {
            foreach (var location in GetAllEquipLocation())
            {
                var item = GetItemInSlot(location);
                foreach (var iteMod in item.GetAddtiveModifires(state))
                {
                    yield return iteMod;
                }
            }
        }

        public IEnumerable<float> GetPercentagesModifires(State state)
        {
            foreach (var location in GetAllEquipLocation())
            {
                var item = GetItemInSlot(location);
                foreach (var iteMod in item.GetPercentagesModifires(state))
                {
                    yield return iteMod;
                }
            }
        }
        

        object ISaveable.CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (var pair in equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.GetItemID();
            }
            return equippedItemsForSerialization;
        }

        void ISaveable.RestoreState(object state)
        {
            equippedItems = new Dictionary<EquipLocation, EquipableItem>();

            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                var item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
                if (item != null)
                {
                    equippedItems[pair.Key] = item;
                }
            }
        }

        
    }
}