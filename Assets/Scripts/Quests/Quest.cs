using RPG.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Quests
{
    
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 4)]
    public class Quest : ScriptableObject 
    {

        [SerializeField] List<Objectives> objectives = new List<Objectives>();


        [System.Serializable]

        //TODO turn it to class to handle difrrent objectives types
        public struct Objectives 
        {
            public string objectiveReference;
            public string objectiveTitle;
        }
        public string GetTitle()
        {
            return name;;
        }

        public int GetObjectivesCount()
        {
           return objectives.Count;
        }

        public IEnumerable<Objectives> GetObjectives()
        {
            return objectives;
        }

        public bool HasObjective(string objectiveRef)
        {
            foreach (var item in objectives)
            {
                if(item.objectiveReference == objectiveRef)
                return true;
            }
            return false;
        }

        public static Quest GetByName(string quest)
        {
            return Resources.Load<Quest>(quest);
        }
    }
}
