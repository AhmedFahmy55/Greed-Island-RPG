using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
   
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        // CONFIG DATA
        [SerializeField] InventoryItem item = null;
        [SerializeField] int number = 1;

        private void Awake()
        {
            // Spawn in Awake so can be destroyed by save system after.
            SpawnPickup();
        }

        // PUBLIC

        public Pickup GetPickup() 
        { 
            return GetComponentInChildren<Pickup>();
        }

        
        public bool isCollected() 
        { 
            return GetPickup() == null;
        }

        //PRIVATE

        private void SpawnPickup()
        {
            var spawnedPickup = item.SpawnPickup(transform.position, number);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }

        object ISaveable.CaptureState()
        {
            return isCollected();
        }

        void ISaveable.RestoreState(object state)
        {
            bool isCollected = (bool)state;

            if (isCollected && !this.isCollected())
            {
                DestroyPickup();
            }

            if (!isCollected && this.isCollected())
            {
                SpawnPickup();
            }
        }
    }
}