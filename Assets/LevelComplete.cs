using Events;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Debug.Log("Level ended");
            GameEvents.onLoadNextLevelEvent?.Invoke();
        }
    }
}
