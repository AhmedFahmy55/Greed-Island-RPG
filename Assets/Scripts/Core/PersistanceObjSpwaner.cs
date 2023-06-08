using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {

    public class PersistanceObjSpwaner : MonoBehaviour
    {
        [SerializeField] private GameObject _persistancePrefap;
        private static bool _isSpawend = false;


                private void Awake() {

                    if(_isSpawend) return;
                    SpwanPersistancePrefap();
                    _isSpawend =true;

                
                }

        private void SpwanPersistancePrefap()
        {
            GameObject persistance = Instantiate(_persistancePrefap);
            DontDestroyOnLoad(persistance);
        }
    }
}
