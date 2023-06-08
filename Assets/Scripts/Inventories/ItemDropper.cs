using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Inventories
{
  
    public class ItemDropper : MonoBehaviour, ISaveable
    {

        [SerializeField] protected float dropDistance = 1;


        protected const int numberOfattempts = 30;
        private List<Pickup> droppedItems = new List<Pickup>();
        private List<DropRecord> otherSceneDrops = new List<DropRecord>();

        
        public void DropItem(InventoryItem item, int number)
        {
            SpawnPickup(item, GetDropLocation(), number);
        }



        protected virtual Vector3 GetDropLocation()
        {
            NavMeshHit hit ;

            for (int i = 0; i < numberOfattempts; i++)
            {
                var position = transform.position + Random.insideUnitSphere * dropDistance ;
                if(NavMesh.SamplePosition(position,out hit,.2f,NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position ;
        }

        private void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            var pickup = item.SpawnPickup(spawnLocation, number);
            droppedItems.Add(pickup);
        }


        

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
            public int sceneIndex;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new List<DropRecord>();
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            foreach (Pickup record in droppedItems)
            {
                Debug.Log(record.GetItem().name);
                var dropedPickup = new DropRecord();
                dropedPickup.itemID = record.GetItem().GetItemID();
                dropedPickup.position = new SerializableVector3(record.transform.position);
                dropedPickup.number = record.GetNumber();
                dropedPickup.sceneIndex = sceneIndex;
                droppedItemsList.Add(dropedPickup);
            }
            droppedItemsList.AddRange(otherSceneDrops);
            return droppedItemsList;
        }

        void ISaveable.RestoreState(object state)
        {
            var droppedItemsList = (List<DropRecord>)state ;
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            otherSceneDrops.Clear();
            foreach (DropRecord drope in droppedItemsList)
            {
                if(drope.sceneIndex != sceneIndex )
                {
                    otherSceneDrops.Add(drope);
                    continue;
                }
                var pickupItem = InventoryItem.GetFromID(drope.itemID);
                Vector3 position = drope.position.ToVector();
                int number = drope.number;
                SpawnPickup(pickupItem, position, number);
            }
        }

    
        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            droppedItems = newList;
        }
    }
}