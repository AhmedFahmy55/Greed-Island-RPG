using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.States
{
    public class BaseStates : MonoBehaviour
    {
        [Range(1,120)]
        [SerializeField] int _startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] ProgressionSO progression = null;
        [SerializeField] GameObject levelUpEffect = null;


        public event Action OnLevelUP;
        int _currentLevel = 0;
        Experience xp ;





        private void OnEnable() 
        {
            if(xp != null )
            xp.OnGainXP += UpdateLevel;
        }

        private void OnDisable() 
        {
            if(xp != null )
            xp.OnGainXP -= UpdateLevel;
        }
        private void Awake() 
        {
            xp = GetComponent<Experience>();    
        }
        private void Start() 
        {
            _currentLevel = CalculateLevel();    
        }



        public float GetState(State state)
        {
            return (GetBaseState(state) + GetAddtimeModifires(state)) * (1 + GetPercentageModifires(state)/100);
        }

       
        private float GetBaseState(State state)
        {
            return progression.GetState(state, characterClass, GetLevel());
        }

        private float GetAddtimeModifires(State state)
        {
            float total = 0 ;
            foreach(IModifireProvider provider in GetComponents<IModifireProvider>())
            {
                foreach (float modifire in provider.GetAddtiveModifires(state))
                {
                    total += modifire ;
                }
            }
            return total;
        }

         private float GetPercentageModifires(State state)
        {
            float total = 0 ;
            foreach(IModifireProvider provider in GetComponents<IModifireProvider>())
            {
                foreach (float modifire in provider.GetPercentagesModifires(state))
                {
                    total += modifire ;
                }
            }
            return total;       
        }

        public int GetLevel()
        {
            if(_currentLevel < 1)
            {
                _currentLevel = CalculateLevel();
            }
            return _currentLevel;
        }
        void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel != _currentLevel)
            {
                _currentLevel = newLevel;
                DoLevelUpEffects();
                OnLevelUP?.Invoke();
            }
        }

        private void DoLevelUpEffects()
        {
            if(levelUpEffect == null) return;
            Instantiate(levelUpEffect,gameObject.transform,false);
        }

        private int CalculateLevel()
        {
            
            if(xp == null) return _startingLevel;
            float currentExp = xp.GetExperience();
            float[] expToLevel = progression.GetStateArray(State.ExpToLevel,characterClass);
            for (int i = 0; i < expToLevel.Length - 1; i++)
            {
                if(expToLevel[i] >= currentExp)
                {
                    return i + 1 ;
                }
                
            }
            return expToLevel.Length  ;
        }

    }
}
