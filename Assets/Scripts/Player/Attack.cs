using Interfaces;
using Obstacles;
using UnityEngine;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            IAttackable attackable = col.GetComponent<Attackable>();
            if (attackable != null)
            {
                attackable.Action();
            }
        }
    }
}
