
using UnityEngine;
using TMPro;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        
        TextMeshProUGUI text = null ;


        private void Awake() 
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
        public void DestroyOnEnd()
        {
            Destroy(gameObject);
        }

        public void SetTextValue(float value)
        {
            text.text = value.ToString();
        }
    }
}
