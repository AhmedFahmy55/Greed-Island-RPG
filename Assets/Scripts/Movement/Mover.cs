using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement{
public class Mover : MonoBehaviour,IAction,ISaveable
    {

        [SerializeField]  float maxSpeed = 6f;
        [SerializeField] float maxTravelDistance = 40 ;

         NavMeshAgent agent;
         Animator anim;
         ActionScheduler scheduler;
        public string ActionName { get => "Mover" ; }


        private void Awake()
        {
            scheduler=GetComponent<ActionScheduler>();
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        
        void Update()
        {
            if(agent.enabled && agent.remainingDistance < .2f)
            {
                agent.isStopped=true;
            }
            UpdateAnimator();
        }

        public bool CanMoveToPoint(Vector3 destinatio)
        {
            NavMeshPath path = new NavMeshPath();
            if(!NavMesh.CalculatePath(transform.position,destinatio,NavMesh.AllAreas,path)) return false;
            if(path.status != NavMeshPathStatus.PathComplete) return false ;
            if(CalculatePathDistance(path) > maxTravelDistance) return false ;
            return true;
        }
        public void StartMove(Vector3 destination,float speedFraction)
        {
            agent.isStopped=false;
            scheduler.StartAction(this);
            MoveToPoint(destination,speedFraction);
        }
        public void MoveToPoint(Vector3 point,float speedFraction)
        {
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.SetDestination(point);
            agent.isStopped=false;
        }
        
        void UpdateAnimator()
        {
            Vector3 realtiveVel = transform.InverseTransformDirection(agent.velocity);
            anim.SetFloat("Speed", realtiveVel.z);

        }

         float CalculatePathDistance(NavMeshPath path)
        {
            float total = 0 ;
            if(path.corners.Length < 2 ) return total;
            for (int i = 0; i < path.corners.Length -1; i++)
            {
                total += Vector3.Distance(path.corners[i],path.corners[i+1]);
            }
            return total;
        }
        public void Cancel()
        {
            agent.isStopped = true;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object obj)
        {
           SerializableVector3 vec = (SerializableVector3)obj;
           agent.Warp(vec.ToVector());
        }
    }

}
