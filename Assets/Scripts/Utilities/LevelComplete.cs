using System.Collections;
using Events;
using UnityEngine;

namespace Utilities
{
    public class LevelComplete : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.CompareTag("Player"))
            {
                Debug.Log("Level ended");
                StartCoroutine(Delay());
            }
        }
    
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(1f);
            GameEvents.onLoadNextLevelEvent?.Invoke();
        }
    }
}
