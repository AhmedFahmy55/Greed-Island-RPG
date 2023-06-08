using UnityEngine;
using UnityEditor;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Control{

    public class AIController : MonoBehaviour {

        [SerializeField]  float _chaceDistance;
        [SerializeField]  Transform _path = null;
        [SerializeField]  float _wayPointStayTime;
        [SerializeField]  float _suspicionTime;
        [SerializeField]  float _AggroTime;

        [Range(0,1f)]
        [SerializeField]  float _patrolSpeed;

       
        [SerializeField] float _callDistance = 1f;


         Transform [] _wayPoints;
         int _currentWayPointIndex = 0;
         float _timeSinceArrivedWayPoint = Mathf.Infinity;
         float _timeSinceLastSawPlayer = Mathf.Infinity;
         float _timeSinceAggregated= Mathf.Infinity;
         bool _aggregated = false;

         Fighter _fighter;
         Mover _mover;
         Health _health;
         Health _playerHealth;

        private void Awake() {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Start() 
        {
            _wayPoints = new Transform[_path.childCount];
            for(int i=0 ; i < _path.childCount ; i++)
            {
                _wayPoints[i]=_path.GetChild(i).transform;
            }
        }
        private void Update() {
           
            if(_health.IsDead()) return;

            if(IsAggregated() && _fighter.CanAttack(_playerHealth))
            {
                Attack();
                if(!_aggregated)
                {
                    AggregateAllyes();
                    _aggregated = true ;
                }
            }
            else if (_timeSinceLastSawPlayer<_suspicionTime) 
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
                 _aggregated = false ;

            }
             else
            {
                _aggregated = false ;
                StartPatrol();
            }

            _timeSinceArrivedWayPoint += Time.deltaTime;
            _timeSinceLastSawPlayer +=Time.deltaTime;
            _timeSinceAggregated += Time.deltaTime;
        }

        void AggregateAllyes()
        {
            RaycastHit [] hits = Physics.SphereCastAll(transform.position,_callDistance,Vector3.up,0);
            foreach (var hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if(ai == null) continue;
                ai.Aggro();
            }
        }
        private void Attack()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_playerHealth);
        }

        // used in unity event on hit
        public void Aggro()
        {
            _timeSinceAggregated = 0 ;
        }
        private void StartPatrol()
        {
            if(_wayPoints == null) return;
            if(_currentWayPointIndex == _wayPoints.Length-1) _currentWayPointIndex = 0;
            if(IsAtCurrentWayPoint())
            {
               _currentWayPointIndex +=1 ;
            }

           if(_timeSinceArrivedWayPoint>_wayPointStayTime)
           {
               _mover.StartMove(_wayPoints[_currentWayPointIndex].position,_patrolSpeed);
               _timeSinceArrivedWayPoint=0;
           }
        }

        private bool IsAtCurrentWayPoint()
        {
            return Vector3.Distance(transform.position,_wayPoints[_currentWayPointIndex].position) < 1f;
        }

        private bool IsAggregated()
        {
           
            float distanceToPlayer = Vector3.Distance(transform.position, _playerHealth.transform.position);
            return distanceToPlayer < _chaceDistance || _timeSinceAggregated < _AggroTime; 
        }

        

        private void OnDrawGizmos() 
        {
            Handles.color=Color.red;
            Handles.DrawWireDisc(transform.position,Vector3.up,_chaceDistance);

            Handles.color=Color.yellow;
            Gizmos.DrawWireSphere(transform.position,_callDistance);


            Gizmos.color=Color.blue;
            if(_path == null) return;

            for(int i =0 ; i< _path.childCount ;i++)
            {
                Gizmos.DrawSphere(_path.GetChild(i).position,.2f);
                int j = i+1;
                if(j==_path.childCount) j=0;
                Gizmos.DrawLine(_path.GetChild(i).position, _path.GetChild(j).position);
            }
            

        }
    
    }
}