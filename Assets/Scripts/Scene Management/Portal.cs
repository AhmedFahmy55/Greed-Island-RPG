using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Saving;
using RPG.Control;

namespace RPG.SceneManagement{
public class Portal : MonoBehaviour
    {
        public enum PortalDestination
        {
            
            A,B,C,D,E,F,G
        }

        [SerializeField] Transform spawnPoint;
        [SerializeField] PortalDestination destination;
        [SerializeField] int _targetSceneIndex;
        [SerializeField] float _fadInTime;
        [SerializeField] float _fadeOutTime;   
        [SerializeField] float waitTime;
    

        private SavingWrapper _saveSys;

        private void Start() 
        {
            _saveSys = FindObjectOfType<SavingWrapper>();
        }
       private void OnTriggerEnter(Collider other) 
       {
            if(other.CompareTag("Player"))
            {
                StartCoroutine(LoadTargetScene(_targetSceneIndex));

            }

       }

       IEnumerator LoadTargetScene(int targetScene)
       {
            if(targetScene < 0) yield break;
            
            Fader fader = GameObject.FindObjectOfType<Fader>();
            //maybe we wanna local portals so we check scene index
            if(SceneManager.GetActiveScene().buildIndex != targetScene) 
            {
                DontDestroyOnLoad(gameObject);
                yield return fader.FadeOut(_fadeOutTime);
                PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                playerController.enabled = false ;
                _saveSys.Save();
                yield return SceneManager.LoadSceneAsync(targetScene);
                PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                newPlayerController.enabled = false ;
                 _saveSys.Load();
                Portal portall = GetOtherPortal();
                if(portall != null) UpdatePlayerOriantaion(portall);
                _saveSys.Save();
                yield return new WaitForSeconds(waitTime);
                yield return fader.FadeIn(_fadInTime);
                newPlayerController.enabled = true ;
                Destroy(gameObject);
            }
            else
            {
                yield return fader.FadeOut(2);
                Portal portal = GetOtherPortal();
                UpdatePlayerOriantaion(portal);
                yield return fader.FadeIn(3);

            }

       }

       Portal GetOtherPortal()
       {
            Portal [] portals = FindObjectsOfType<Portal>();
            foreach(Portal portal in portals)
            {
                if(portal == this) continue;
                if(portal.destination != this.destination)continue;
                return portal;
            }
            return null;
       }

        void UpdatePlayerOriantaion(Portal portal)
        {
            Debug.Log("update player");
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.position);
            player.transform.rotation = portal.transform.rotation;

        }
    }

   
}
