using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        
        [SerializeField]  GameObject hitEffect;
        [SerializeField]  List<GameObject> destroyOnImpact;
        [SerializeField]  float speed;
        [SerializeField]  bool isHoming;
        [SerializeField]  float maxLifeTime;
        [SerializeField]  float lifeAfterImpact;
        [SerializeField]  float hitEffectLifeTime;
        [SerializeField] UnityEvent OnImpact;
         Health target;

        
         float damage;
         GameObject attacker;


        private void Start() 
        {
            transform.LookAt(GetTargetDirection());
        }
        private void Update() 
        {
            if (target == null) return ;
            if(isHoming && !target.IsDead() )
            {
                transform.LookAt(GetTargetDirection());
            }
            
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        public void SetTarget(Health targetHealth,float weaponDamage,GameObject attacker)
        {
            target = targetHealth ;
            damage = weaponDamage ;
            this.attacker = attacker;
            Destroy(gameObject,maxLifeTime);
        }
        
        private Vector3 GetTargetDirection()
        {
            
            return target.transform.position + (Vector3.up * target.GetComponent<CapsuleCollider>().height / 2) ;
        }
        private void OnTriggerEnter(Collider other)
        {

            if(other.GetComponent<Health>() !=  target) return ;
            if(target.IsDead()) return ;
            OnImpact?.Invoke();
            if(hitEffect != null)
            {
                GameObject hitEffectobj = Instantiate(hitEffect,GetTargetDirection(),Quaternion.identity,target.transform);
                Destroy(hitEffectobj,hitEffectLifeTime);
            }
            
            speed = 0 ;
            target.TakeDamage(damage,attacker);
            foreach(GameObject obj in destroyOnImpact)
            {
                Destroy(obj);
            }

            Destroy(gameObject,lifeAfterImpact);
            
        }
    }
}
