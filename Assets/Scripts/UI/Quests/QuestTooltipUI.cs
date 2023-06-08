using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;
using UnityEngine.UI;


namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectivesContainer;
        [SerializeField] GameObject objectivePrefap;
        [SerializeField] Sprite finished;
        [SerializeField] Sprite notFinished;




        public void Setup(QuestStatus questStatus)
        {
            Quest quest = questStatus.GetQuest();
            title.text = quest.GetTitle();
            
            foreach (var objectve in quest.GetObjectives())
            {
              
                GameObject objectiveInstance = Instantiate(objectivePrefap,objectivesContainer);
                objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objectve.objectiveTitle;
                if(questStatus.IsObjectiveFinished(objectve.objectiveReference))
                {
                    objectiveInstance.GetComponentInChildren<Image>().sprite = finished;
                }
                else
                {
                    objectiveInstance.GetComponentInChildren<Image>().sprite = notFinished;
                }
            }
        }
    }
}