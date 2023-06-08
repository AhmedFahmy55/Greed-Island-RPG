using UnityEngine;
using RPG.Inventories;

namespace RPG.Combat{

    [CreateAssetMenu(fileName ="newProgression" ,menuName = "RPG/Weapon", order = 1)]
    public class WeaponSo : EquipableItem
    {
        [SerializeField]  Weapon weaponPrefap;
        [SerializeField]  AnimatorOverrideController weaponAnim = null ;
        [SerializeField]  float weaponRange ;
        [SerializeField]  float attackCallDown ;
        [SerializeField]  bool isRightHanded = true ;
        [SerializeField]  bool isProjectile = false ;
        [SerializeField]  bool isProjectileRightHand;
        [SerializeField]  Projectile projectilePrefap = null ;
        Weapon lastInstantiatedWep = null ;

        private const string weaponName = "Weapon" ;

        public void SpwanWeapon(Transform rightHand,Transform leftHand , Animator anim)
        {
            DestroyOldWeapon(rightHand, leftHand);
        
            Transform pivot = IsRighthand(rightHand,leftHand,isRightHanded);
            if (weaponPrefap != null)
            {   
               Weapon weapon = Instantiate( weaponPrefap , pivot);
               weapon.gameObject.name = weaponName ;
               lastInstantiatedWep = weapon ;

            } 
            AnimatorOverrideController currentanim = anim.runtimeAnimatorController as AnimatorOverrideController;
            if (weaponAnim != null) 
            {
                anim.runtimeAnimatorController = weaponAnim ;
            }  
            else if (currentanim != null)
            {
               anim.runtimeAnimatorController = currentanim.runtimeAnimatorController;
            }  
        }
        private Transform IsRighthand(Transform right , Transform left , bool rHand)
        {
            return rHand ? right : left ;
        }

        public void DestroyOldWeapon(Transform rightHand , Transform leftHand)
        {
            Weapon[] oldWepR = rightHand.GetComponentsInChildren<Weapon>();
            Weapon[] oldWepL = leftHand.GetComponentsInChildren<Weapon>();

            DestroyChilds(oldWepR);
            DestroyChilds(oldWepL);

        }

        private void DestroyChilds(Weapon[] childs)
        {
            if (childs.Length >= 1)
            {
                foreach (var item in childs)
                {
                    Destroy(item.gameObject);
                }
            }
        }

        public void Spwanprojectile(Transform RightHand,Transform leftHand,Health target,GameObject attacker,float damage)
        {
            Transform pivot = IsRighthand(RightHand,leftHand,isProjectileRightHand);
            Projectile projectile = Instantiate(projectilePrefap ,pivot.position,Quaternion.identity);
            projectile.SetTarget(target, damage,attacker);
        }

        public Weapon GetWeapon()
        {
            return lastInstantiatedWep;
            
        }
        
        public float GetWeaponRange()
        {
            return weaponRange ;
        }

        public float GetAttackCallDown()
        {
            return attackCallDown ;
        }

        public bool HasProjectile()
        {
            return isProjectile;
        }
       
    }
}
