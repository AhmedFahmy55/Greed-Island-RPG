using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus 
    {

        Quest quest;
        List<string> finishedObjectives = new List<string>();

        [System.Serializable]
        public class QuestStatusRecord 
        {
            public string quest;
            public List<string> finishedObjectives; 
        }
        public QuestStatus(QuestStatusRecord newQuest) 
        {
            quest = Quest.GetByName(newQuest.quest);
            finishedObjectives = newQuest.finishedObjectives;
        }

        public QuestStatus(Quest newQuest)
        {
            this.quest = newQuest;
        }

        public Quest GetQuest()
        {
            return quest;
        }
        public int GetFinishedObjectivesNumb()
        {
            return finishedObjectives.Count;
        }

        public bool IsObjectiveFinished(string objective)
        {
            return finishedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if(quest.HasObjective(objective))
            {
                finishedObjectives.Add(objective);
            }
        }

        public QuestStatusRecord CaptureState()
        {
            QuestStatusRecord questRecord = new QuestStatusRecord();
            questRecord.quest = quest.name;
            questRecord.finishedObjectives = finishedObjectives;
            return questRecord;
        }
    }
}
