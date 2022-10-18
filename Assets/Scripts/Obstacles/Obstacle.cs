using System;
using UnityEngine;

namespace Obstacles
{
    public class Obstacle : MonoBehaviour
    { 
        [SerializeField] float speed = .25f;
        
        private ParticleSystem _particleSystem;
        void Update()
        {
            transform.position += -transform.right * (Time.deltaTime * speed);
            if (transform.position.x <= -8f)
            {
                Destroy(gameObject);
            }
        }

        
        
    }
}
