using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.States
{
    [CreateAssetMenu(fileName = "new ProgressionSO", menuName = "RPG/Progression", order = 0)]
    public class ProgressionSO : ScriptableObject 
    {
        
        [SerializeField] CharacterProgression [] progressions;

        Dictionary<CharacterClass,Dictionary<State,float[]>> lookupTable = null;



        public float GetState(State stat,CharacterClass charClass,int level)
        {
            // TODO guard agains progressionSO = null >> give default value to state
            BuildlookupTable();
            float[] levels = lookupTable[charClass][stat];
            int stateMaxLevel = levels.Length;
            // retruhng the max leve in the stat if the level higher than its max level
            if(stateMaxLevel < level) return levels[stateMaxLevel - 1];
            return levels[level - 1];
           
        }

        public float [] GetStateArray(State stat , CharacterClass charClass)
        {
            BuildlookupTable();
            return lookupTable[charClass][stat];
        }

        private void BuildlookupTable()
        {
            if(lookupTable != null) return;
            lookupTable = new Dictionary<CharacterClass, Dictionary<State, float[]>>();
            foreach (CharacterProgression charProgression in progressions)
            {
                var stateDic = new Dictionary<State,float[]>();

                foreach (ProgressionStates progressionStates in charProgression.states)
                {
                    stateDic[progressionStates.state] = progressionStates.levels;
                }
                lookupTable[charProgression.characterClass] = stateDic;
            }
        }

        [System.Serializable]
        struct CharacterProgression
        {
            public CharacterClass characterClass;
            public ProgressionStates [] states;
            
        }

        [System.Serializable]
        struct ProgressionStates 
        {
            public State state;
            public float [] levels;
        }
    }
}