using UnityEngine;
using RPG.Core;
using RPG.Control;
using UnityEngine.AI;
using RPG.Saving;
using RPG.States;
using System;
using RPG.Utils;
using RPG.Inventories;
using UnityEngine.Events;

namespace RPG.Combat
{

    public class Health : MonoBehaviour,ISaveable
    {
        [SerializeField] TakeDamageEvent OnTakeDamage;
        [SerializeField] UnityEvent OnDie; 

        LazyValue<float> _maxHeath,_currentHealth;
        bool _isDead = false;
        BaseStates states;
        Equipment equipment;




        private void Awake()
        {
            equipment = GetComponent<Equipment>();
            states = GetComponent<BaseStates>();
            _maxHeath = new LazyValue<float>(InitMaxHealth);
            _currentHealth = new LazyValue<float>(InitCurrentHealth);

        }

        private void OnEnable()
        {
            states.OnLevelUP += UpdateHeath;
            if(equipment != null)
            equipment.equipmentUpdated += UpdateHeath;
        }
         private void OnDisable()
        {
            states.OnLevelUP -= UpdateHeath;
            if(equipment != null)
            equipment.equipmentUpdated -= UpdateHeath;
        }

        
       

        private void Start() 
        {
            _maxHeath.ForceInit();
            _currentHealth.ForceInit();
        }

        
        private float InitMaxHealth()
        {
            return  states.GetState(State.Heatlh); 
           
        }

        private float InitCurrentHealth()
        {
           return _maxHeath.value; 
        }
       

        public bool IsDead()
        {
            return _isDead;
        }
        
         private void UpdateHeath()
        {   
            
            float nextLevHealth = states.GetState(State.Heatlh);
            // TODO >> for now will just add the diff betwwen current max health and next level max health
            float healthDiff = nextLevHealth - _maxHeath.value;
            _maxHeath.value = nextLevHealth ;
            _currentHealth.value += healthDiff ;
        }
        public void TakeDamage(float amount,GameObject attaker)
        {
            _currentHealth.value = Mathf.Max(_currentHealth.value - amount,0);
            if(_currentHealth.value == 0) 
            {
                OnDie?.Invoke();
                Die();
                AwardExpPoints(attaker);

            }
            else
            {
                OnTakeDamage?.Invoke(amount);
            }
        }

        private void AwardExpPoints(GameObject attaker)
        {
           Experience exp = attaker.GetComponent<Experience>();
            if(exp == null) return;
            exp.GainExp(states.GetState(State.Exp));
        }

        public float GetMaxHeath()
        {
            return _maxHeath.value;
        }
        public float GetCurrentHealth()
        {
            return _currentHealth.value;
        }
        public float GetHealthFraction()
        {
            return _currentHealth.value /_maxHeath.value ;
        }

        void Die()
        {
            
            if(_isDead) return;
            _isDead = true;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = !_isDead;


        }

        

        public object CaptureState()
        {
            HeatlhSave heatlhSave = new HeatlhSave(_maxHeath.value,_currentHealth.value); //{maxHeath = _maxHeath , currentHeath =_currentHealth};
                
            return heatlhSave;
        }

        public void RestoreState(object obj)
        {
            HeatlhSave heatlhSave = (HeatlhSave)obj;
            _maxHeath.value = heatlhSave.maxHeath;
            _currentHealth.value = heatlhSave.currentHeath;
            if(_currentHealth.value <= 0) Die();
        }

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>{}

        [System.Serializable]
         struct HeatlhSave
        {
            public HeatlhSave (float maxH , float currentH)
            {
                maxHeath = maxH ;
                currentHeath = currentH ;
            }
            public float maxHeath;
            public  float currentHeath;
        }
    }
}