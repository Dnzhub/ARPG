using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using RPG.Resources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
{   
    Figther figther;
    Health health;
    private void Start()
    {
        health = GetComponent<Health>();
    }
    void Update()
        {
            if(health.IsDead()) return; //Player öldü ise assağıdaki kodları durdur.
            if(interactWithCombat())  return;
            if(interactWithMovement()) return;
            
        }

        private bool interactWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {

                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;

                
                if(!GetComponent<Figther>().canAttack(target.gameObject))
                { 
                    continue; 
                }
                
                if(Input.GetMouseButton(1))
                {
                    GetComponent<Figther>().Attack(target.gameObject);
                   
                }
                return true;
            }
            return false;
        }

        private bool interactWithMovement()
    {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if(Input.GetMouseButton(1))
                {
                    GetComponent<Move>().StartMoveAction(hit.point, 1f);
                    
                }
                return true;
            }
            return false;
    }

    

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
