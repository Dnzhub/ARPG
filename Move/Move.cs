using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    public class Move : MonoBehaviour,IAction, ISaveable
{   
    NavMeshAgent navMeshAgent;
    Health health;

    [SerializeField] float maxSpeed = 5f; //Karakterin ulasabilecegi max hız bu şekilde artık speedi Navmeshten değiştiremeyeceğiz.
    
    
    private void Awake() 
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
       
    }
    void Update()
    {
        navMeshAgent.enabled = !health.IsDead(); //Navmesh eğer karakter ölü değilse aktif aksi halde de aktif olacak.
        updateAnimator();  
      
    }
    
    public void StartMoveAction(Vector3 destination, float speedFraction) //Speed fraction devriye sırasında düsmanın hızını yavaslatacak.
    {
        GetComponent<ActionScheduler>().startAction(this);
        
        moveTo(destination,speedFraction);
    }
    public void moveTo(Vector3 destination,float speedFraction)
    {
       navMeshAgent.destination = destination;
       navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction); //Clamp01 : 0 ve 1 değerlerinin dısına cıkma demek.
       navMeshAgent.isStopped = false;
    }
    public void Cancel()
    {
        navMeshAgent.isStopped = true;
    }

   
    private void updateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity); // karakterin hızı sadece z ekseninde değerlendirilip animasyonu harekete gecirecek yani sağ sol farketmez adım attığı an animator oynat
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("Speed", speed);
    }

        public object CaptureState() //Mevcut durumu kaydet
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)//Mevcut durumu Yükle
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

