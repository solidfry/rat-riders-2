using UnityEngine;
using Events;

namespace Obstacles
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private float timeBetweenSpawn;
        [SerializeField] private float startTimeBetweenSpawn;
        [SerializeField] private float spawnTimeIncrement = 0.1f;
        [SerializeField] private float minTimeBetweenSpawn = 1f;
        [SerializeField] private int spawnTimeMod = 5;
        [SerializeField] private Obstacle[] obstaclesList;
        [SerializeField] private bool isSpawning;
        public bool IsSpawning => isSpawning;

        public float StartTimeBetweenSpawn
        {
            get => startTimeBetweenSpawn;
            set
            {
                startTimeBetweenSpawn = Mathf.Clamp(value, minTimeBetweenSpawn, startTimeBetweenSpawn);
            }
        }

        [SerializeField][ReadOnly] private int obstacleSpawnCount;

        void Update()
        {
            if (isSpawning)
            {
                if (timeBetweenSpawn <= 0)
                {
                    Instantiate(obstaclesList[Random.Range(0, obstaclesList.Length)], transform.position, Quaternion.identity, transform);
                    timeBetweenSpawn = StartTimeBetweenSpawn;

                    if (obstacleSpawnCount % spawnTimeMod == 0)
                        StartTimeBetweenSpawn -= spawnTimeIncrement;

                    obstacleSpawnCount++;
                    GameEvents.onObstacleSpawnedEvent?.Invoke(obstacleSpawnCount);
                }
                else
                {
                    timeBetweenSpawn -= Time.deltaTime;
                }
            }
        }
    }
}
