using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;

namespace RPG.Control
{
    public class EnemyAI : MonoBehaviour
{
   [SerializeField] float chaseDistance = 5f;
   [SerializeField] float suspicionTime = 5f;
   [SerializeField] float WaitAtPoint = 5f;
   [SerializeField] PatrolPath patrolPath;
   [SerializeField] float wayPointTollerance = 1f;
   [Range(0,1)] // assagıdaki patrolspeed değeri 0 ve 1 arasında olacak.
   [SerializeField] float patrolSpeedFraction = 0.5f; //Patrol sırasında max speedin % 50 sine düsecek
   
   Figther figther;
   GameObject player;
   Move move;
   Health health;
   
   Vector3 guardPosition;
   

   float timeSinceLastSawPlayer = Mathf.Infinity;
   float timeSinceCurrentPoint = Mathf.Infinity;
   int getCurrentWayPointIndex = 0;
   
   
    private void Start()
    {
       figther = GetComponent<Figther>();
       player = GameObject.FindWithTag("Player"); 
       health = GetComponent<Health>();
       move = GetComponent<Move>();
       

       guardPosition = transform.position;

    }
   private void Update()
        {
            if (health.IsDead()) return; //Eger Enemy öldüyse assağıdaki kodlara devam etme.
            if (InAttackRangeOfPlayer() && figther.canAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime) // Düsmandan uzaklasınca 5 saniye bekleyip daha sonra yerine gidicek
            {                                               // Yani 5 saniyelik süphelenme zamanı verdik.
                SuspiciousBehaviour();
            }
            else
            {
                PatrolBehaviour(); // Eger player düsmandan uzaklasır ise düsman Devriye alanına geri dönecek.
            }
            UpdateTimer();

        }

        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceCurrentPoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if(patrolPath != null)
            {
                
                if(AtWayPoint())
                {   
                    timeSinceCurrentPoint = 0;            
                    CycleWayPoint();   
                   
                }
                nextPosition = GetCurrentWayPoint();
            }
            if(timeSinceCurrentPoint > WaitAtPoint) //Eger düsman yeni waypointte 5 saniyeden fazla durursa diğer waypointe haraket edecek
            {
                move.StartMoveAction(nextPosition,patrolSpeedFraction);
                
            }
            
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPoint(getCurrentWayPointIndex); //Mevcut Konum
        }

        private void CycleWayPoint()
        {
            getCurrentWayPointIndex = patrolPath.NextIndex(getCurrentWayPointIndex); //bir sonraki waypointe hareket et.
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position,GetCurrentWayPoint());
            return distanceToWayPoint < wayPointTollerance; //Enemy waypointe 1 metreden daha yakınsa true cevir.
        }

        private void SuspiciousBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            figther.Attack(player);
            
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer =  Vector3.Distance(player.transform.position,transform.position);
            return  distanceToPlayer < chaseDistance;
        }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,chaseDistance);
    }

    }
}