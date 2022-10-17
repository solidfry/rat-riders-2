using Interfaces;
using UnityEngine;

namespace Obstacles
{
    public class Attackable : MonoBehaviour, IAttackable
    {
        public void Action()
        {
            Destroy(this.gameObject);
        }
    }
}