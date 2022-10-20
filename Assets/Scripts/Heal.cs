using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;


public class Heal : MonoBehaviour
{
    bool heal = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player.Attack>())
        {
            heal = true;
            Debug.Log($"Heal was set to {heal}");
        }
    }

    private void OnDestroy()
    {
        if (heal == true)
        {
            Debug.Log("Heal was invoked");
            GameEvents.onPlayerHealedEvent?.Invoke();
        }
    }
}
