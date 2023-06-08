using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
       private const string SAVE_PATH = "save";
        

        SavingSystem saveSys;



        private void Awake()
    
        {
            saveSys = GetComponent<SavingSystem>();
        }

        void Start() 
        {
          // yield return saveSys.LoadLastScene(savePath);
            
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }  
             
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }  
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
                Debug.Log("delted");
            } 
        }
        public void Save()
        {
            saveSys.Save(SAVE_PATH);
            Debug.Log("saved");
        }

        public void Load()
        {
            saveSys.Load(SAVE_PATH);
            Debug.Log("loaded");

        }

        public void  Delete() 
        {
            {
                saveSys.Delete(SAVE_PATH);
            }
        }

    }

     
}
