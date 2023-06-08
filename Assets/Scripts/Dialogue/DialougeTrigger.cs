using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogues
{
    public class DialougeTrigger : MonoBehaviour
    {
        [SerializeField] string action;
        public UnityEvent OnTrigger;


        public void Trigger(string actiontrigger)
        {
            if(actiontrigger == action) OnTrigger?.Invoke();
        }
    }
}
