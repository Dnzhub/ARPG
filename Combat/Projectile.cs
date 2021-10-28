using UnityEngine;
using RPG.Resources;
namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
{
    Health target = null;
    GameObject instigator = null;
    [SerializeField] float speed = 1f;
    [SerializeField] bool isFollowing = true;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] float maxLifeTİme = 6f;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] float lifeAfterHit = 2f;
    float damage = 0f;
    
    private void Start() 
    {
        transform.LookAt(GetAimLocation()); // Ateş edilen oklar düşmanı takip etmeden ilk pozisyonuna gidicekler.

    }
    private void Update()
    {
        
        if(target == null) return;
        if(isFollowing && !target.IsDead())  // Yalnızca karakter ölü değilken ve isFollowing true iken takip edecek.
        {
            transform.LookAt(GetAimLocation()); // eger isFollowing true yaparsak oklar sürekli hedefi takip edecekler 
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime); 
    }   
    public void SetTarget(Health target,GameObject instigator, float damage)
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;

        Destroy(gameObject,maxLifeTİme); // Mermiyi 6 saniye sonra yok et.
    }
    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if(targetCapsule == null)
        {
            return target.transform.position;
        }                               
        return target.transform.position + Vector3.up * targetCapsule.height / 2;  // // Karakterin transformu ayaklarından basladıgı icin
                                        //Atılan okta ayagına gidiyor bu yüzden capsulecolliderın yüksekliğinin yarı değerini alıp
    }                                   // vücudun orta noktasını hedef gösterdik 

    private void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<Health>() != target) return;
        if(target.IsDead()) return; // Hedef öldüyse ok icinden gecip devam edecek.
        target.TakeDamage(instigator, damage); 
        speed = 0f; // Bunu oklar icin yaptık hedefe carptıgında hızı 0 olup yerinde sabit kalacak.
        if(hitEffect != null)
        {
           Instantiate(hitEffect,GetAimLocation(),transform.rotation);          
        }
        foreach (GameObject toDestroy in destroyOnHit)
        {
            Destroy(toDestroy); // destroyonhit listesindekiler bekelemeden direk olarak yok olacak.
        }
        
        Destroy(gameObject, lifeAfterHit); // destroyOnHit dısındakiler lifeAfterHit süresi sonrasında yok olacak
      
    }
}

}