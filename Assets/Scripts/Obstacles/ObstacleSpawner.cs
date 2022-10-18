using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

namespace Obstacles
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private float timeBetweenSpawn;
        [SerializeField] private float startTimeBetweenSpawn;
        [SerializeField] private Obstacle[] obstaclesList;
        [SerializeField] private bool isSpawning;
        public bool IsSpawning => isSpawning;

        void Update()
        {
            if (isSpawning)
            {
                if (timeBetweenSpawn <= 0)
                {
                    Instantiate(obstaclesList[Random.Range(0, obstaclesList.Length)], transform.position, Quaternion.identity);
                    timeBetweenSpawn = startTimeBetweenSpawn;
                }
                else
                {
                    timeBetweenSpawn -= Time.deltaTime;
                }
            }
        }
    }
}
