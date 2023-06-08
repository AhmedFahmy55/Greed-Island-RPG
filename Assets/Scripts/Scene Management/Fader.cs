using UnityEngine;
using System.Collections;
using System;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentFader = null ;


        private void Awake() 
        {

            canvasGroup= GetComponent<CanvasGroup>();
        }


        public IEnumerator FadeOut(float time)
        {            
            return Fade(1,time);   
        }
       public IEnumerator FadeIn(float time)
        {
            return Fade(0,time);
        }

        IEnumerator Fade(float targetAlpha,float time)
        {
            if(currentFader != null)
            {
                StopCoroutine(currentFader);

            }
            currentFader = StartCoroutine(FadeRoutine(targetAlpha,time));
            yield return currentFader;
        }   

        private IEnumerator FadeRoutine(float targetAlpha, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha,targetAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha,targetAlpha,Time.deltaTime / time );
               yield return null ;
            }
        }
}    }