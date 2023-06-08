using UnityEngine;
using RPG.Movement;
using RPG.Core;
using UnityEngine.AI;
using RPG.Saving;
using RPG.States;
using RPG.Utils;
using RPG.Inventories;
using System.Collections.Generic;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour,IAction,ISaveable
    {

        
        [SerializeField] private Transform _rightHandTransform;
        [SerializeField] private Transform _leftHandTransform;

        [SerializeField] private WeaponSo _defaultWeaponSO = null ;
        
        

        float _timeSinceLastAttack = Mathf.Infinity;
        Mover mover;
        Animator anim;
        LazyValue<WeaponSo> _currentWeaponSO ;
        ActionScheduler _scheduler;
        Equipment equipment;
        Health _targetHealth;
        BaseStates _states;

        public string ActionName { get => "Fighter"; }

        private void Awake() {
            _states = GetComponent<BaseStates>();
            _scheduler=GetComponent<ActionScheduler>();
            mover=GetComponent<Mover>();
            anim=GetComponent<Animator>();
            _currentWeaponSO = new LazyValue<WeaponSo>(InitDefaultWeapon);
            equipment = GetComponent<Equipment>();
        }

          private void OnEnable() 
        {
            if(equipment == null) return;
            equipment.equipmentUpdated += UpdateWeapon;
        }

        private void OnDisable() 
        {
            if(equipment == null) return;
            equipment.equipmentUpdated -= UpdateWeapon;

        }

        private void Start() 
        {
            _currentWeaponSO.ForceInit();
            
        }

        private void Update()
        {
         
            _timeSinceLastAttack += Time.deltaTime;

            if(_targetHealth == null) return;
            if(_targetHealth.IsDead())
            {
                _targetHealth = null;
                return;
            }

            if (!IsTargetInRange(_targetHealth.transform) && mover.CanMoveToPoint(_targetHealth.transform.position))
            {
                mover.MoveToPoint(_targetHealth.transform.position,1f);
            }
            else
            {
                AttackBehaviour();
            }

        }


        void UpdateWeapon()
        {
            Debug.Log("update weapon");
            WeaponSo weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponSo;
            if(weapon != null)
            {
                Debug.Log($"equiupment has {weapon.name} equipped");
                EquiepWeapon(weapon);
            }
            else
            {
                Debug.Log($"equiupment has no weapon  equipping default");
                EquiepWeapon(_defaultWeaponSO);
            }
            
        }

        private WeaponSo InitDefaultWeapon()
        {
            AttachWeapon(_defaultWeaponSO);
            return _defaultWeaponSO;
        }

        public void EquiepWeapon(WeaponSo equipedWeapon)
        {
            _currentWeaponSO.value = equipedWeapon;
            AttachWeapon(equipedWeapon);
        }

        private void AttachWeapon(WeaponSo equipedWeapon)
        {
            equipedWeapon.SpwanWeapon(_rightHandTransform, _leftHandTransform, anim);
        }

        public Weapon GetWeapon()
        {
            return _currentWeaponSO.value.GetWeapon();
        }


        public bool CanAttack(Health enemyHealth)
        {
           if(enemyHealth == null) return false; 
           if(!mover.CanMoveToPoint(enemyHealth.transform.position) && !IsTargetInRange(enemyHealth.transform)) return false;
           return !enemyHealth.IsDead();
            
        }

        public Health GetTargetHealth()
        {
            return _targetHealth ;
        }

        private bool IsTargetInRange(Transform target)
        {
            
            return Vector3.Distance(transform.position, target.position) <= _currentWeaponSO.value.GetWeaponRange();
        }
        void AttackBehaviour()
        {
            GetComponent<NavMeshAgent>().isStopped = true ;
            transform.LookAt(_targetHealth.transform);

            if(_timeSinceLastAttack > _currentWeaponSO.value.GetAttackCallDown())
            {
                // this will trigger the AttackHit()
                // TODO fix a bug where the animation triggers one more time after last animation which will kill the enemy
                anim.ResetTrigger("CancelAttack");
                anim.SetTrigger("Attack");
                _timeSinceLastAttack = 0;
            }
            
        }
          //for animation event
        public void AttackHit()
        {
            if(_targetHealth == null) return;
            if(_currentWeaponSO.value.HasProjectile())
            {
                _currentWeaponSO.value.Spwanprojectile(_rightHandTransform,_leftHandTransform, _targetHealth,gameObject
                ,_states.GetState(State.Damage));
            } 
            else
            {
                _targetHealth.TakeDamage(_states.GetState(State.Damage),gameObject);
            }

            Weapon weapon = GetWeapon();
            if(weapon != null) weapon.OnWeaponHit();
            
        }

        public void Attack(Health enemyHealth)
        {
            
            _scheduler.StartAction(this);
            
            _targetHealth = enemyHealth;
        }
       

        public void Cancel()
        {
            _targetHealth = null;
            mover.Cancel();
            anim.ResetTrigger("Attack");
            anim.SetTrigger("CancelAttack");
            
        }
        

        public object CaptureState()
        {
            return _currentWeaponSO.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName =(string)state;
            WeaponSo wep = Resources.Load<WeaponSo>(weaponName);
            if(wep != null)
            EquiepWeapon(wep);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
           if(_defaultWeaponSO != null) UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up,_defaultWeaponSO.GetWeaponRange());
        }

        
    }
    
}