using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  RPG.Saving;
using System;

namespace RPG.States
{
    public class Experience : MonoBehaviour,ISaveable
    {
        [SerializeField] float _exp;

      
        public  event Action OnGainXP;

        public void GainExp(float xp)
        {
            _exp += xp ;
            OnGainXP?.Invoke();
        }

         public float GetExperience()
        {
            return _exp;
        }
          public object CaptureState()
        {
            return _exp;
        }

        public void RestoreState(object state)
        {
            float experience = (float) state;
            _exp = experience;
        }

       
    }

}