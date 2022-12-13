using Events;
using UnityEngine;

public class InstantDeath : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticle;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<Collider2D>().isTrigger) return;

        
        var t = col.transform;
        Instantiate(deathParticle, t.position, t.rotation);
        
        if(col.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);
        }
        
        if(col.CompareTag("Player")) 
        {
            GameEvents.onKillPlayerEvent.Invoke();
        }
    }
}
