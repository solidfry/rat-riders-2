using UnityEngine;

namespace Obstacles
{
    public class ObstacleMovement : MonoBehaviour
    { 
        [SerializeField]float speed = .25f;

    
        void Update()
        {
            transform.position += -transform.right * (Time.deltaTime * speed);
        }
    }
}
