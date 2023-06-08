using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogues;
using TMPro;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI npcText;
        [SerializeField] Transform choicesRoot;
        [SerializeField] GameObject choicePrefap;
        [SerializeField] Button nextButton;
        [SerializeField] Button QuitButton;

        PlayerDialogueDirector playerDialogueDirector;


        private void Awake() 
        {
            playerDialogueDirector = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueDirector>();
        }
         void Start() 
        {
           playerDialogueDirector.OnUpdateUI += UpdateUI;
           nextButton.onClick.AddListener(()=> playerDialogueDirector.GoNext() ); 
           QuitButton.onClick.AddListener(()=> playerDialogueDirector.QuitDialogue());
           UpdateUI();
        }  

        public void UpdateUI()
        {
            gameObject.SetActive(playerDialogueDirector.IsActive());
            if(!playerDialogueDirector.IsActive()) return ;
           

           npcText.text = playerDialogueDirector.GetText();

           if(!playerDialogueDirector.HasNext())
           {
                nextButton.gameObject.SetActive(false);
                
           } 
           else
           {
                nextButton.gameObject.SetActive(true);
           }

           SetUpResponses();

        }
        private void SetUpResponses()
        {
            foreach (Transform item in choicesRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (DialogueNode node in playerDialogueDirector.GetPlayerRsponses())
            {
                Button choice = Instantiate(choicePrefap,choicesRoot).GetComponent<Button>();
                var choiceText = choice.GetComponent<TextMeshProUGUI>();
                choiceText.text = node.GetText();
                choice.onClick.AddListener(()=>
                {
                    playerDialogueDirector.SelectNode(node);
                });
            }

        }
    }
}
